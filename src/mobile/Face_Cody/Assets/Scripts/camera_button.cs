using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class camera_button : MonoBehaviour
{
    public Camera camera;
    public GameObject faceOccluder;
    string path;
    byte[] bytes;


    void Start()
    {
        camera = GameObject.Find("First Person Camera").GetComponent<Camera>();
        faceOccluder = GameObject.Find("FaceOccluder");
        path = Application.persistentDataPath + "/My_ScreenShot/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void onClick()
    {
        faceOccluder.SendMessage("SaveMeshInfo", SendMessageOptions.DontRequireReceiver);
        ScreenCapture();
        sendImage();
    }

    public void ScreenCapture()
    {
        string name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        camera.targetTexture = rt;
        Texture2D screen_shot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //Rect rect = new Rect(0, 0, screen_shot.width, screen_shot.height);
        camera.Render();
        RenderTexture.active = rt;
        screen_shot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screen_shot.Apply();

        bytes = screen_shot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
        camera.targetTexture = null;
    }

    public void sendImage()
    {
        StartCoroutine("UploadImage");
    }

    IEnumerator UploadImage()
    {
        WWWForm form = new WWWForm();

        print("form created ");

        form.AddField("action", "upload");

        form.AddField("file", "file");

        form.AddBinaryData("file", bytes, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png", "image/png");

        print("binary data added ");
        
        WWW w = new WWW("http://192.168.0.2:8000/mediatest/upload", form);
        print("www created");

        yield return w;
    }

    }
