using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


//class BitmapEncoder
//{
//    public static void WriteBitmap(Stream stream, int width, int height, byte[] bytes)
//    {
//        using (BinaryWriter bw = new BinaryWriter(stream))
//        {
//            bw.Write((UInt16)0x4D42);
//            bw.Write((UInt32)(14 + 40 + (width * height * 4)));
//            bw.Write((UInt16)0);
//            bw.Write((UInt16)0);
//            bw.Write((UInt32)14 + 40);

//            bw.Write((UInt32)40);
//            bw.Write((Int32)width);
//            bw.Write((Int32)height);
//            bw.Write((UInt16)1);
//            bw.Write((UInt16)32);
//            bw.Write((UInt32)0);
//            bw.Write((UInt32)(width * height * 4));
//            bw.Write((Int32)0);
//            bw.Write((Int32)0);
//            bw.Write((UInt32)0);
//            bw.Write((UInt32)0);

//            for (int idx = 0; idx < bytes.Length; idx += 3)
//            {
//                bw.Write(bytes[idx + 2]);
//                bw.Write(bytes[idx + 1]);
//                bw.Write(bytes[idx]);
//                bw.Write((byte)255);
//            }
//        }
//    }
//}

public class ScreenCaptureController : MonoBehaviour
{
    private Camera camera;
    private GameObject connector;
    //private readonly int frameRate = 5;
    //private readonly int maxFrames = 20;
    //private int numFrames;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("First Person Camera").GetComponent<Camera>();
        connector = GameObject.Find("Connector");
        //numFrames = 0;

        if (!Directory.Exists(Global.imagePath))
        {
            Directory.CreateDirectory(Global.imagePath);
        }

        //InvokeRepeating("ScreenCapture", 0.0f, 1.0f / frameRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 단일 이미지 캡쳐
    public void ScreenCapture(string name)
    {
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture.active = rt;

        camera.targetTexture = rt;
        camera.Render();
        camera.targetTexture = null;

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        RenderTexture.active = null;
        screenShot.Compress(true);


        byte[] imageBytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(string.Format("{0}.png",Global.imagePath + name), imageBytes);

        //byte[] imageBytes = screenShot.GetRawTextureData();
        //FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
        //BitmapEncoder.WriteBitmap(fs, Screen.width, Screen.height, imageBytes);
        //fs.Close();

        connector.GetComponent<ConnectController>().SaveHappiness(name, imageBytes);
    }

    // 다중 이미지 캡쳐
    //public void ScreenCapture()
    //{
    //    if(numFrames < maxFrames)
    //    {
    //        string name = path + string.Format("frame#{0}.png", ++numFrames);
    //        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
    //        RenderTexture.active = rt;

    //        camera.targetTexture = rt;
    //        camera.Render();
    //        camera.targetTexture = null;

    //        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    //        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    //        screenShot.Apply();
    //        RenderTexture.active = null;
    //        screenShot.Compress(true);

    //        byte[] imageBytes = screenShot.GetRawTextureData();
    //        FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write);
    //        BitmapEncoder.WriteBitmap(fs, Screen.width, Screen.height, imageBytes);
    //        fs.Close();

    //        DestroyImmediate(screenShot);
    //        DestroyImmediate(rt);
    //    }
    //    else
    //    {
    //        this.enabled = false;
    //    }
    //}
}
