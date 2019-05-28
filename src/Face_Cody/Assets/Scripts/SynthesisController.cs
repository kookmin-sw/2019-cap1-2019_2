using UnityEngine;
using UnityEngine.UI;

public class SynthesisController : MonoBehaviour
{
    private GameObject panelsController;
    private GameObject nextButton;
    private byte[] synthesizedImage;

    // Start is called before the first frame update
    void Start()
    {
        panelsController = GameObject.Find("PanelsController");
        nextButton = GameObject.Find("NextButton");
        synthesizedImage = null;
    }

    public void OnClick()
    {
        int panel = panelsController.GetComponent<PanelsController>().GetActivePanel();

        switch (panel)
        {
            case 0:
                if (synthesizedImage != null)
                {
                    System.Array.Clear(synthesizedImage, 0, synthesizedImage.Length);
                    synthesizedImage = null;
                }

                GameObject.Find("Connector").GetComponent<ConnectController>().LoadSynthesizedImage();
                panelsController.GetComponent<PanelsController>().ChangeActivePanel(1);
                break;

            case 2:
                SaveToAndroidGallery();
                break;

            default:
                break;
        }
    }

    public void SetSyntheSizedImage(byte[] bytes)
    {
        synthesizedImage = bytes.Clone() as byte[];
    }

    public byte[] GetSyntehsizedImage()
    {
        return synthesizedImage;
    }

    public void UpdateNextButton(int panel)
    {
        switch (panel)
        {
            case 0:
                nextButton.transform.Find("Text").GetComponent<Text>().text = "합성";
                break;

            case 1:
                nextButton.SetActive(false);
                break;

            case 2:
                nextButton.SetActive(true);
                nextButton.transform.Find("Text").GetComponent<Text>().text = "저장";
                break;

            default:
                break;
        }
    }

    public void SaveToAndroidGallery()
    {
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.LoadImage(synthesizedImage);
        NativeGallery.SaveImageToGallery(texture, "FaceCody", string.Format("{0}+{1}.png", Global.targetImageName, Global.sourceImageName));
    }
}
