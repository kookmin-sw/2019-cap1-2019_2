using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    private List<string> imageNames;
    public GameObject image;
    private GameObject imageObject;

    private void Start()
    {
        UpdateImageList();
        CreateImageObject();

        if (Global.selectMode == 0)
        {
            GameObject.Find("Text").GetComponent<Text>().text = "확인";
        }
        else
        {
            GameObject.Find("Text").GetComponent<Text>().text = "선택";
        }
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
            if(imageName == Global.targetImageName)
            {
                imageObject.gameObject.tag = "TargetImage";
            }
            else if(imageName == Global.sourceImageName)
            {
                imageObject.gameObject.tag = "SourceImage";
            }
            imageObject.GetComponent<ImageLoader>().SetImageName(imageName);
            imageObject.GetComponent<ImageLoader>().AttachImage();
        }

        switch (Global.selectMode)
        {
            case 1:
                GameObject.FindWithTag("TargetImage").GetComponent<ImageLoader>().SetMark(true);
                break;

            case 2:
                GameObject.FindWithTag("SourceImage").GetComponent<ImageLoader>().SetMark(true);
                break;

            default:
                break;
        }
    }

    public void DeleteAllImages()
    {
        DirectoryInfo dictInfo = new DirectoryInfo(Global.imagePath);
        foreach (FileInfo file in dictInfo.GetFiles())
        {
            File.Delete(file.FullName);
        }
    }
}
