using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextureImage : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D texture = null;
    private int _id;
    private string imagename;
    public RawImage img;

    void Start()
    {
        get_image();
        attach_image();
    }
    void get_image()
    { 
        _id = GameObject.Find("Controller").GetComponent<LoadImage>().getId();
        GameObject.Find("Controller").GetComponent<LoadImage>().CountId();
        imagename = GameObject.Find("Controller").GetComponent<LoadImage>().ImageNames[_id];
    }
    void attach_image()
    {
        string path = Global.imgPath + imagename +".png";
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        texture = new Texture2D(0, 0);
        texture.LoadImage(byteTexture);
        img.GetComponent<RawImage>().texture = texture;
    }
    public void on_click()
    {
        if(Global.mode == 0)
        {
            Global.targetImageName = imagename;
            //GameObject.Find("Text1").GetComponent<Text>().text = controller.target_id.ToString() + " " + controller.source_id.ToString();
        }
        else
        {
            Global.sourceImageName = imagename;
            //GameObject.Find("Text1").GetComponent<Text>().text = controller.target_id.ToString() + " " + controller.source_id.ToString();
        }

    }

}
