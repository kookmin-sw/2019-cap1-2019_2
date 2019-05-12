using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GalleryController : MonoBehaviour
{
    private List<string> imageNames = new List<string>();
    public GameObject image;

    private void Start()
    {
        UpdateImageList();
        CreateImageObject();
    }

    private void UpdateImageList()
    {
        System.IO.DirectoryInfo dictInfo = new System.IO.DirectoryInfo(Global.imgPath);
        foreach (System.IO.FileInfo File in dictInfo.GetFiles())
        {
            if (File.Extension.ToLower() == ".png")
            {
                string filename = File.Name.Substring(0, File.Name.Length-4);
                imageNames.Add(filename);
            }
        }
    }

    private void CreateImageObject()
    {
        foreach (string imageName in imageNames)
        {
            GameObject imageObject = GameObject.Instantiate(image, transform.position, transform.rotation);
            imageObject.transform.SetParent(GameObject.Find("Grid").transform, false);
            imageObject.GetComponent<ImageLoader>().SetImageName(imageName);
            imageObject.GetComponent<ImageLoader>().AttachImage();
        }
    }
}

