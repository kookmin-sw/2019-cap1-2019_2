using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectController : MonoBehaviour
{
    private string ipAddress = "";

    // Start is called before the first frame update
    void Start()
    {
        //ipAddress = "192.168.0.101";
        ipAddress = "172.22.151.23";

        if (!Directory.Exists(Global.logPath + "emotion/"))
        {
            Directory.CreateDirectory(Global.logPath + "emotion/");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveHappiness(string name, byte[] data)
    {
        StartCoroutine(RequestToHappiness(name, data));
    }

    IEnumerator RequestToHappiness(string name, byte[] data)
    {
        //GameObject.Find("Text").GetComponent<TextController>().text.text = string.Format("http://{0}:8000/server/happiness", ipAddress);

        WWWForm form = new WWWForm();
        form.AddBinaryData("data", data, name + ".png", "image/png");
        UnityWebRequest webRequest = UnityWebRequest.Post(string.Format("http://{0}:8000/server/happiness", ipAddress), form);
        yield return webRequest.SendWebRequest();

        //if (webRequest.isNetworkError || webRequest.isHttpError)
        //{
        //    GameObject.Find("Text").GetComponent<TextController>().text.text += ("\n" + webRequest.error);
        //}
        //else
        //{
        //    GameObject.Find("Text").GetComponent<TextController>().text.text += "\n" + webRequest.downloadHandler.text;
        //}

        File.WriteAllBytes(string.Format("{0}emotion/{1}_happiness.txt", Global.logPath, name), webRequest.downloadHandler.data);
    }
}
