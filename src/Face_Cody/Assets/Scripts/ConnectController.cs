using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(Global.logPath + "emotion/"))
        {
            Directory.CreateDirectory(Global.logPath + "emotion/");
        }
    }

    public void SaveHappiness(string name, byte[] data)
    {
        StartCoroutine(RequestToHappiness(name, data));
    }

    IEnumerator RequestToHappiness(string name, byte[] data)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("data", data, name + ".png", "image/png");
        UnityWebRequest webRequest = UnityWebRequest.Post(string.Format("http://{0}:8000/server/happiness", Global.ipAddress), form);
        yield return webRequest.SendWebRequest();

        File.WriteAllBytes(string.Format("{0}emotion/{1}_happiness.txt", Global.logPath, name), webRequest.downloadHandler.data);
    }

    public void SaveSynthesizedImage()
    {
        StartCoroutine(RequestToSynthesis());
    }
    IEnumerator RequestToSynthesis()
    {
        string meshPath = Global.logPath + "/mesh/";
        string texturePath = Global.logPath + "/texture/";

        byte[] targetTextureVertices = File.ReadAllBytes(texturePath + Global.targetImageName + "_vertices.txt");
        byte[] targetHeadPose = File.ReadAllBytes(meshPath + Global.targetImageName + "_headPose.txt");
        
        byte[] sourceTextureVertices = File.ReadAllBytes(texturePath + Global.sourceImageName + "_vertices.txt");
        byte[] sourceHeadPose = File.ReadAllBytes(meshPath + Global.sourceImageName + "_headPose.txt");
        byte[] sourceMeshVertices = File.ReadAllBytes(meshPath + Global.sourceImageName + "_vertices.txt");
        
        WWWForm form = new WWWForm();

        form.AddBinaryData("targetTextureVertices", targetTextureVertices, Global.targetImageName + "_vertices.txt", "text/txt");
        form.AddBinaryData("targetHeadPose", targetHeadPose, Global.targetImageName + "_headPose.txt", "text/txt");

        form.AddBinaryData("sourceTextureVertices", sourceTextureVertices, Global.sourceImageName + "_vertices.txt", "text/txt");
        form.AddBinaryData("sourceHeadPose", sourceHeadPose, Global.sourceImageName + "_headPose.txt", "text/txt");
        form.AddBinaryData("sourceMeshVertices", sourceMeshVertices, Global.sourceImageName + "_vertices.txt", "text/txt");

        UnityWebRequest webRequest = UnityWebRequest.Post(string.Format("http://{0}:8000/server/synthesis", Global.ipAddress), form);
        yield return webRequest.SendWebRequest();

        //File.WriteAllBytes(string.Format()) save result image
    }
}