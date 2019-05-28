using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RecommendController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> Names = new List<string>();
    private string emotionPath;
    string meshPath;
    float maxEmotion = 0.5f;
    private GameObject sourceImageViewer;

    private void Start()
    {
        emotionPath = Global.logPath + "/emotion/";
        meshPath = Global.logPath + "/mesh/";
        sourceImageViewer = GameObject.Find("SourceImageViewer");

        UpdateNameList();
    }
    public void SetSourceImage()
    {
        GetSourceImage();
    }
    private void UpdateNameList()
    {
        System.IO.DirectoryInfo dictInfo = new System.IO.DirectoryInfo(Global.imagePath);
        foreach (System.IO.FileInfo File in dictInfo.GetFiles())
        {
            if (File.Extension.ToLower() == ".png")
            {
                string filename = File.Name.Substring(0, File.Name.Length - 4);

                Names.Add(filename);
            }
        }
    }
    private float GetPoseSimilarity(Vector3 v)
    {
        float distance = 0.0f;

        distance += v.x * v.x;
        distance += v.y * v.y;
        //distance += v.z * v.z;

        return distance;

    }
    private void GetSourceImage()
    {
        foreach(string Name in Names)
        {
            if(Name == Global.targetImageName)
            {
                continue;
            }
            string[] lines = File.ReadAllLines(meshPath + Name + "_headPose.txt");
            string[] line = lines[1].Split(' ');

            Vector3 pose = new Vector3(Convert.ToSingle(line[0]), Convert.ToSingle(line[1]), Convert.ToSingle(line[2]));
            if( GetPoseSimilarity(pose) < 181.0f )
            {
                line = File.ReadAllLines(emotionPath + Name + "_happiness.txt");
                float emotion = Convert.ToSingle(line[0]);
                if(emotion > maxEmotion)
                {
                    maxEmotion = emotion;
                    Global.sourceImageName = Name;
                }
            }
        }
    }
}
