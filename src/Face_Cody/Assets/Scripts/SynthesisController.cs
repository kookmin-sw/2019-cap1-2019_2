using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SynthesisController : MonoBehaviour
{
    private GameObject targetImage;
    private GameObject sourceImage;
    private Text targetText;
    private Text sourceText;

    private GameObject savePanel;
    private GameObject synthesisPanel;
    private GameObject loadPanel;

    float angle = 0.0f;

    void Start()
    {
        targetImage = GameObject.Find("TargetImage");
        sourceImage = GameObject.Find("SourceImage");
        targetText = GameObject.Find("TargetText").GetComponent<Text>();
        sourceText = GameObject.Find("SourceText").GetComponent<Text>();
        savePanel = GameObject.Find("SavePanel");
        synthesisPanel = GameObject.Find("SynthesisPanel");
        loadPanel = GameObject.Find("LoadingSign");

        //synthesisPanel.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        LoadImages();
    }
    void Update()
    {
        angle += Time.deltaTime * -30.0f;
        loadPanel.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
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
            synthesisPanel.SetActive(false);
            loadPanel.SetActive(true);
        }
    }

    public void SaveImages(byte[] bytes)
    {
        loadPanel.SetActive(false);
        savePanel.SetActive(true);
        // change raw image 
        // save to gallery
    }
    private void SaveToGallery(byte[] bytes)
    {
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.LoadImage(bytes);
        texture.Apply();

        Debug.Log("" + NativeGallery.SaveImageToGallery(texture, "FaceCody", "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));
    }
}
