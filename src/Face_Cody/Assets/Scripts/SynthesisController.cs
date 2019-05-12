using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class SynthesisController : MonoBehaviour
{
    byte[] trgByte;
    byte[] srcByte;
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
            trgByte = System.IO.File.ReadAllBytes(Global.imgPath + Global.targetImageName + ".png");
            Texture2D trg = new Texture2D(0, 0);
            trg.LoadImage(trgByte);

            srcByte = System.IO.File.ReadAllBytes(Global.imgPath + Global.sourceImageName + ".png");
            Texture2D src = new Texture2D(0, 0);
            src.LoadImage(srcByte);

            GameObject.Find("TargetImage").GetComponent<RawImage>().texture = trg;
            GameObject.Find("SourceImage").GetComponent<RawImage>().texture = src;
        }
        else if(Global.sourceImageName == "")
        {
            trgByte = System.IO.File.ReadAllBytes(Global.imgPath + Global.targetImageName + ".png");
            Texture2D trg = new Texture2D(0, 0);
            trg.LoadImage(trgByte);

            GameObject.Find("TargetImage").GetComponent<RawImage>().texture = trg;

        }
        else if(Global.targetImageName == "")
        {
            srcByte = System.IO.File.ReadAllBytes(Global.imgPath + Global.sourceImageName + ".png");
            Texture2D src = new Texture2D(0, 0);
            src.LoadImage(srcByte);

            GameObject.Find("SourceImage").GetComponent<RawImage>().texture = src;

        }
    }
    public void ToTargetMode()
    {
        Global.selectMode = false;
    }
    public void ToSourceMode()
    {
        Global.selectMode = true;
    }
    public void OnClick()
    {
        connector.GetComponent<ConnectController>().UploadImages();
    }

}
