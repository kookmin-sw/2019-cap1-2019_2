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
        Screen.SetResolution(int.Parse(string.Format("{0}", args[3])), int.Parse(string.Format("{0}", args[4])), false);

        GameObject.Find("Mesh").GetComponent<Modeler>().DrawMesh();
    }
}
