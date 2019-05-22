using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    private bool isClicked = false;
    private string imageName = "";
    public RawImage img;

    public void AttachImage()
    {
        string path = string.Format("{0}/{1}.png", Global.imagePath, imageName);
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(byteTexture);
        img.GetComponent<RawImage>().texture = texture;
    }
    public void OnClick()
    {
        if(Global.selectMode == false)
        {
            if (isClicked == false)
            {
                //if (Global.targetImageName != imageName && Global.targetImageName != "")
                //{
                //    alertMessage = true;
                //}
                if (Global.targetImageName == imageName || Global.targetImageName == "")
                {
                    isClicked = true;
                    img.GetComponent<RawImage>().color = Color.gray;
                    Global.targetImageName = imageName;
                }
            }
            else
            {
                isClicked = false;
                img.GetComponent<RawImage>().color = Color.white;
                Global.targetImageName = "";
            }
        }
        else
        {
            if (isClicked == false)
            {
                isClicked = true;
                img.GetComponent<RawImage>().color = Color.gray;
                Global.sourceImageName = imageName;
            }
            else
            {
                isClicked = false;
                img.GetComponent<RawImage>().color = Color.white;
                Global.sourceImageName = "";
            }
        }

    }

    public void SetImageName(string name)
    {
        imageName = name;
    }
    
    public void SetWhite()
    {
        img.GetComponent<RawImage>().color = Color.white;
    }
}
