using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
using System.IO;

public class InputArgument : MonoBehaviour
{
    string[] args = Environment.GetCommandLineArgs();
    private string SaveImagePath = "C:\\Users\\mugcup\\Desktop\\Modeling\\Assets";

    private string SourceImageName = "";
    private string TargetImageName = "";
    private string MeshDataPath = "";
    public void Awake()
    {
        if (args.Length != 0)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-SavePath" || args[i] == "-SP")//Capture된 Image를 저장할 Path
                {
                    SaveImagePath = args[i + 1];
                }
                else if(args[i] == "-SourceImage" || args[i] == "-SI")//SourceImage Name을 받을 변수
                {
                    SourceImageName = args[i + 1];
                }
                else if(args[i] == "-TargetImage" || args[i] == "-TI")//TargetImage Name을 받을 변수
                {
                    TargetImageName = args[i + 1];
                }
            }
        }
        //SourceImageName = "190509_174439"; 유니티에서 바로 실행시 필요한 변수
        //TargetImageName = "190509_174524";
    }
    public string GetSaveImagePath()
    {
        return SaveImagePath;
    }
    public string GetSourceImageName()
    {
        return SourceImageName;
    }
    public string GetTargetImageName()
    {
        return TargetImageName;
    }
    void Start()
    {
    }
    public void Update()
    {
    }
}