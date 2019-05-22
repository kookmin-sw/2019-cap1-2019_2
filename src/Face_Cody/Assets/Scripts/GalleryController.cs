using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GalleryController : MonoBehaviour
{
    private List<string> imageNames;
    public GameObject image;
    private GameObject imageObject;

    private void Start()
    {
        UpdateImageList();
        CreateImageObject();
    }

    private void UpdateImageList()
    {
        imageNames = new List<string>();

        DirectoryInfo dictInfo = new DirectoryInfo(Global.imagePath);
        foreach (FileInfo File in dictInfo.GetFiles())
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
            imageObject = Instantiate(image, transform.position, transform.rotation);
            imageObject.transform.SetParent(GameObject.Find("Grid").transform, false);
            Vector3 newScale = imageObject.transform.localScale;
            newScale *= 1.5f;
            imageObject.transform.localScale = newScale;
            imageObject.GetComponent<ImageLoader>().SetImageName(imageName);
            imageObject.GetComponent<ImageLoader>().AttachImage();
            imageObject.GetComponent<ImageLoader>().SetWhite();
        }
    }
}
