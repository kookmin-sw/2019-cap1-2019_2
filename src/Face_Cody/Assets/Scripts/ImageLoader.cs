using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
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
        if(Global.selectMode == 1) // select target image
        {
            if(Global.targetImageName != "")
            {
                GameObject oldTargetImage = GameObject.FindWithTag("TargetImage");
                oldTargetImage.GetComponent<ImageLoader>().SetMark(false);
                oldTargetImage.tag = "Untagged";
            }            

            Global.targetImageName = imageName;
            gameObject.tag = "TargetImage";
            SetMark(true);
        }
        else if(Global.selectMode == 2)// select source image
        {
            if(Global.sourceImageName != "")
            {
                GameObject oldSourceImage = GameObject.FindWithTag("SourceImage");
                oldSourceImage.GetComponent<ImageLoader>().SetMark(false);
                oldSourceImage.tag = "Untagged";
            }

            Global.sourceImageName = imageName;
            gameObject.tag = "SourceImage";
            SetMark(true);
        }
    }

    public void SetImageName(string name)
    {
        imageName = name;
    }

    public void SetMark(bool isMark)
    {
        if(isMark == true)
        {
            img.GetComponent<RawImage>().color = Color.gray;
        }
        else
        {
            img.GetComponent<RawImage>().color = Color.white;
        }
    }
}
