using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageLoader : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D texture = null;
    private string imageName = "";
    public RawImage img;

    void Start()
    {
    
    }

    public void AttachImage()
    {
        string path = Global.imgPath + imageName +".png";
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        texture = new Texture2D(0, 0);
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
