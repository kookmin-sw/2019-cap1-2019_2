using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    private string[] args;

    // Start is called before the first frame update
    void Start()
    {
        args = Environment.GetCommandLineArgs();
        Global.targetName = args[1];
        Global.sourceName = args[2];
        Screen.SetResolution(Global.imageWidth, Global.imageHeight, false);

        GameObject.Find("Mesh").GetComponent<Modeler>().DrawMesh();
    }
}
