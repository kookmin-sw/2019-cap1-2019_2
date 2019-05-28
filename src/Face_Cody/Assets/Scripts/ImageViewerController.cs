using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewerController : MonoBehaviour
{
    private GameObject sourceImageViewer;
    private GameObject targetImageViewer;
    private GameObject synthesizedImageViewer;
    private GameObject recommendController;
    // Start is called before the first frame update
    void Start()
    {
        sourceImageViewer = GameObject.Find("SourceImageViewer");
        targetImageViewer = GameObject.Find("TargetImageViewer");
        synthesizedImageViewer = GameObject.Find("SynthesizedImageViewer");
        recommendController = GameObject.Find("RecommendController");
    }

    public void UpdateImageViewer(int panel)
    {
        switch (panel)
        {
            case 0:
                if (Global.sourceImageName != "")
                {
                    AttachImage(sourceImageViewer, Global.sourceImageName);
                    sourceImageViewer.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
                }
                else
                {
                    sourceImageViewer.transform.Find("Text").gameObject.GetComponent<Text>().text = "이미지 불러오기";
                }

                if (Global.targetImageName != "")
                {
                    AttachImage(targetImageViewer, Global.targetImageName);
                    targetImageViewer.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
                    recommendController.GetComponent<RecommendController>().SetSourceImage();
                    AttachImage(sourceImageViewer, Global.sourceImageName);
                    sourceImageViewer.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
                }
                else
                {
                    targetImageViewer.transform.Find("Text").gameObject.GetComponent<Text>().text = "이미지 불러오기";
                }
                break;

            case 2:
                byte[] imageBytes = GameObject.Find("SynthesisController").GetComponent<SynthesisController>().GetSyntehsizedImage();
                AttachImage(synthesizedImageViewer, imageBytes);
                break;

            default:
                break;
        }
    }

    void AttachImage(GameObject gameObject, string imageName)
    {
        byte[] bytes = File.ReadAllBytes(string.Format("{0}/{1}.png", Global.imagePath, imageName));
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);
        gameObject.GetComponent<RawImage>().texture = texture;
    }

    void AttachImage(GameObject gameObject, byte[] bytes)
    {
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);
        gameObject.GetComponent<RawImage>().texture = texture;
    }

    public void GoToSelectTargetImage()
    {
        Global.selectMode = 1;
        GameObject.Find("SceneLoadController").GetComponent<SceneLoadController>().MoveToGallery();
    }

    public void GoToSelectSourceImage()
    {
        Global.selectMode = 2;
        GameObject.Find("SceneLoadController").GetComponent<SceneLoadController>().MoveToGallery();
    }
}
