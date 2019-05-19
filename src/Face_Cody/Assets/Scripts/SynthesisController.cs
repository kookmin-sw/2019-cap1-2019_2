using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SynthesisController : MonoBehaviour
{
    private GameObject targetImage;
    private GameObject sourceImage;
    private Text targetText;
    private Text sourceText;

    void Start()
    {
        targetImage = GameObject.Find("TargetImage");
        sourceImage = GameObject.Find("SourceImage");
        targetText = GameObject.Find("TargetText").GetComponent<Text>();
        sourceText = GameObject.Find("SourceText").GetComponent<Text>();
        LoadImages();
    }

    private void LoadImages()
    {
        if(Global.sourceImageName != "")
        {
            sourceText.text = "";
            AttachImage(sourceImage, Global.sourceImageName);
        }
        else
        {
            sourceText.text = "Select Image";
        }
        if(Global.targetImageName != "")
        {
            targetText.text = "";
            AttachImage(targetImage, Global.targetImageName);
        }
        else
        {
            targetText.text = "Select Image";
        }
    }

    private void AttachImage(GameObject gameObject, string imageName)
    {
        byte[] bytes = File.ReadAllBytes(string.Format("{0}/{1}.png", Global.imagePath, imageName));
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);
        gameObject.GetComponent<RawImage>().texture = texture;
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
        if(Global.sourceImageName == "")
        {
            return;
        }
        else if (Global.targetImageName == "")
        {
            return;
        }
        else
        {
            GameObject.Find("Connector").GetComponent<ConnectController>().SaveSynthesizedImage();
        }
    }
}
