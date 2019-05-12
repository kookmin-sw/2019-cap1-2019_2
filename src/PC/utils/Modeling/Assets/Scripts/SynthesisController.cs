using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class SynthesisController : MonoBehaviour
{
    byte[] trgbyte;
    byte[] srcbyte;
    private GameObject connector;

    void Start()
    {
        connector = GameObject.Find("Connector");
        load();
    }
    void Update()
    {
    }

    public void load()
    {
        if(Global.sourceImageName != "" && Global.targetImageName != "")
        {
            trgbyte = System.IO.File.ReadAllBytes(Global.imgPath + Global.targetImageName + ".png");
            Texture2D trg = new Texture2D(0, 0);
            trg.LoadImage(trgbyte);

            srcbyte = System.IO.File.ReadAllBytes(Global.imgPath + Global.sourceImageName + ".png");
            Texture2D src = new Texture2D(0, 0);
            src.LoadImage(srcbyte);

            GameObject.Find("TargetImage").GetComponent<RawImage>().texture = trg;
            GameObject.Find("SourceImage").GetComponent<RawImage>().texture = src;
        }
        else if(Global.sourceImageName == "")
        {
            trgbyte = System.IO.File.ReadAllBytes(Global.imgPath + Global.targetImageName + ".png");
            Texture2D trg = new Texture2D(0, 0);
            trg.LoadImage(trgbyte);

            GameObject.Find("TargetImage").GetComponent<RawImage>().texture = trg;

        }
        else if(Global.targetImageName == "")
        {
            srcbyte = System.IO.File.ReadAllBytes(Global.imgPath + Global.sourceImageName + ".png");
            Texture2D src = new Texture2D(0, 0);
            src.LoadImage(srcbyte);

            GameObject.Find("SourceImage").GetComponent<RawImage>().texture = src;

        }
    }
    public void ToTargetMode()
    {
        Global.mode = 0;
    }
    public void ToSourceMode()
    {
        Global.mode = 1;
    }
    public void onClick()
    {
        connector.GetComponent<ConnectController>().UploadImages();
    }

}
