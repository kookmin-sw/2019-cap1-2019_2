﻿using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(Global.logPath + "/emotion/"))
        {
            Directory.CreateDirectory(Global.logPath + "/emotion/");
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

        File.WriteAllBytes(string.Format("{0}/emotion/{1}_happiness.txt", Global.logPath, name), webRequest.downloadHandler.data);
    }

    public void LoadSynthesizedImage()
    {
        StartCoroutine(RequestToSynthesis());
    }

    IEnumerator RequestToSynthesis()
    {
        string meshPath = Global.logPath + "/mesh";
        string texturePath = Global.logPath + "/texture";

        byte[] targetImage = File.ReadAllBytes(string.Format("{0}/{1}.png", Global.imagePath, Global.targetImageName));
        byte[] targetMeshHeadPose = File.ReadAllBytes(string.Format("{0}/{1}_headPose.txt", meshPath, Global.targetImageName));
        byte[] targetTextureHeadPose = File.ReadAllBytes(string.Format("{0}/{1}_headPose.txt", texturePath, Global.targetImageName));
        byte[] targetTextureVertices = File.ReadAllBytes(string.Format("{0}/{1}_vertices.txt", texturePath, Global.targetImageName));
        byte[] targetLight = File.ReadAllBytes(string.Format("{0}/{1}_light.txt", meshPath, Global.targetImageName));

        byte[] sourceImage = File.ReadAllBytes(string.Format("{0}/{1}.png", Global.imagePath, Global.sourceImageName));
        byte[] sourceMeshHeadPose = File.ReadAllBytes(string.Format("{0}/{1}_headPose.txt", meshPath, Global.sourceImageName));
        byte[] sourceMeshVertices = File.ReadAllBytes(string.Format("{0}/{1}_vertices.txt", meshPath, Global.sourceImageName));
        byte[] sourceTextureVertices = File.ReadAllBytes(string.Format("{0}/{1}_vertices.txt", texturePath, Global.sourceImageName));

        WWWForm form = new WWWForm();

        form.AddBinaryData("targetImage", targetImage, Global.targetImageName + ".png", "image/png");
        form.AddBinaryData("targetMeshHeadPose", targetMeshHeadPose, Global.targetImageName + "_headPose.txt", "text/txt");
        form.AddBinaryData("targetTextureHeadPose", targetTextureHeadPose, Global.targetImageName + "_headPose.txt", "text/txt");
        form.AddBinaryData("targetTextureVertices", targetTextureVertices, Global.targetImageName + "_vertices.txt", "text/txt");
        form.AddBinaryData("targetLight", targetLight, Global.targetImageName + "_light.txt", "text/txt");

        form.AddBinaryData("sourceImage", sourceImage, Global.sourceImageName + ".png", "image/png");
        form.AddBinaryData("sourceMeshHeadPose", sourceMeshHeadPose, Global.sourceImageName + "_headPose.txt", "text/txt");
        form.AddBinaryData("sourceMeshVertices", sourceMeshVertices, Global.sourceImageName + "_vertices.txt", "text/txt");
        form.AddBinaryData("sourceTextureVertices", sourceTextureVertices, Global.sourceImageName + "_vertices.txt", "text/txt");

        UnityWebRequest webRequest = UnityWebRequest.Post(string.Format("http://{0}:8000/server/synthesis", Global.ipAddress), form);
        yield return webRequest.SendWebRequest();

        GameObject.Find("SynthesisController").GetComponent<SynthesisController>().SetSyntheSizedImage(webRequest.downloadHandler.data);
        GameObject.Find("PanelsController").GetComponent<PanelsController>().ChangeActivePanel(2);
        yield break;
    }
}