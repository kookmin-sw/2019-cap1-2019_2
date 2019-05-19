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
        if(Global.selectMode == false)
        {
            Global.targetImageName = imageName;
        }
        else
        {
            Global.sourceImageName = imageName;
        }

    }
    public void SetImageName(string name)
    {
        imageName = name;
    }
}
