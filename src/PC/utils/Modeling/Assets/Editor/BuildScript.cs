using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.ComponentModel;
using System.Diagnostics;

public static class BuildScript 
{
    static void BuildWindow()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows64);
        string binaryFilePath = UnityEngine.Application.dataPath + "/../" + "new/myGame.exe";

        List<string> enableScenePathList = new List<string>();
        enableScenePathList.Add(UnityEngine.Application.dataPath + "\\Scenes\\SampleScene.unity");

        if (false == File.Exists(binaryFilePath))
        {
            FileInfo fileInfo = new FileInfo(binaryFilePath);
            fileInfo.Directory.CreateSubdirectory(fileInfo.DirectoryName);
        }

        BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
        BuildOptions buildOption = BuildOptions.None;//AutoRunPlayer;
        
        BuildPipeline.BuildPlayer(enableScenePathList.ToArray(), binaryFilePath, buildTarget, buildOption);
    }

}
