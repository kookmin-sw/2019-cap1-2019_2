using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadImage : MonoBehaviour
{
    public List<string> ImageNames = new List<string>();
    public GameObject Image;
    private int id = 0;
    private void Start()
    {
        Load();
    }
    private void Load()
    {
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Global.imgPath);
        foreach (System.IO.FileInfo File in di.GetFiles())
        {
            if (File.Extension.ToLower() == ".png")
            {
                //string fullfilename = File.FullName;
                string filename = File.Name.Substring(0, File.Name.Length-4);
                ImageNames.Add(filename);
            }
        }
        for (int i = 0; i < ImageNames.Count; i++)
        {
            Instantiate(Image).transform.SetParent(GameObject.Find("Grid").transform, false);
        }
    }
    public void CountId()
    {
        id += 1;
    }
    public int getId()
    {
        return id;
    }
}
