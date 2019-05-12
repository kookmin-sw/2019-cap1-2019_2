using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  
using System.IO;
using System;
using GoogleARCore.Examples.AugmentedFaces;
//using UnityEngine.SceneManagement;

//[RequireComponent(typeof(MeshFilter))]
public class Procedural_Mesh : MonoBehaviour
{
    /// <summary>;
    
    /// Path Value
    private string Path = "C:\\Users\\mugcup\\Desktop\\Modeling\\Assets\\Resources\\"; //Vertices, HeadPose가 저장 되어있는 경로
    private string SaveImagePath = ""; 
    private string SourceImageName = "";
    private string TargetImageName = "";

    InputArgument argument;

    /// 

    private Camera MainCamera;
    private bool ShootScreenShotState; //exe를 종료하기 위한 bool변수
    //Vector3[] TargetVertices;

    private float[] SourceFaceAngle;  
    private float[] TargetFaceAngle;

    /// </summary>
    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    void Awake()
    {
        argument = GameObject.Find("Main Camera").GetComponent<InputArgument>();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        SourceImageName = argument.GetSourceImageName();
        TargetImageName = argument.GetTargetImageName();

        SaveImagePath = argument.GetSaveImagePath();

        LoadMeshData();
        CreateMesh();

        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
        ShootScreenShotState = false;
        StartCoroutine("ReadyToShootScreenShot");//Rendering을 다 하고 나서 screenshot를 실행하기 위해 coroutine을 사용
        WriteTargetVertices(); // SourceHeadPose - TargetHeadPose의 각도 많큼 Source의 Vertices를 회전 이동 시킨후 저장하는 함수

    }


    // Update is called once per frame
    void Update()
    {
        if (ShootScreenShotState)
        {
            Application.Quit();
        }
    }

    void LoadMeshData()
    {
        // TextAsset data = Resources.Load("vertices", typeof(TextAsset)) as TextAsset;
        // StringReader sr = new StringReader(data.text);
        string[] lines = File.ReadAllLines(Path + SourceImageName + "_vertices.txt");
        vertices = new Vector3[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            vertices[i] = new Vector3(float.Parse(line[0]), float.Parse(line[1]), float.Parse(line[2]));
        }

        Debug.Log("DataLoad : " + vertices[0]);
        lines = File.ReadAllLines(Path + "UVs.txt");

        uv = new Vector2[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(',');
            uv[i] = new Vector2(float.Parse(line[0]), float.Parse(line[1]));
        }

        lines = File.ReadAllLines(Path + "\\triangles.txt");
        triangles = new int[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            triangles[i] = int.Parse(lines[i]);
        }
    
        lines = File.ReadAllLines(Path + SourceImageName + "_HeadPose.txt");
        lines = lines[0].Split(',');

        SourceFaceAngle = new float[3];
        for (int i = 0; i < lines.Length; i++)
        {
            SourceFaceAngle[i] = Convert.ToSingle(lines[i])*-1; //좌표계를 맞추기 위해 -1을 곱해서 저장
        }

        lines = File.ReadAllLines(Path + TargetImageName + "_HeadPose.txt");
        lines = lines[0].Split(',');

        TargetFaceAngle = new float[3];
        for (int i = 0; i < lines.Length; i++)
        {
            TargetFaceAngle[i] = Convert.ToSingle(lines[i]) * -1; //좌표계를 맞추기 위해 -1을 곱해서 저장
        }
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

    private void TakeShootScreenShot()
    {
        RenderTexture renderTexture = MainCamera.targetTexture; //camera가 찍을 화면을 Rendering하기 위한 변수
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

        RenderTexture.active = renderTexture;
        renderResult.ReadPixels(rect, 0, 0);
 
        byte[] byteArray = renderResult.EncodeToPNG();
        System.IO.File.WriteAllBytes(SaveImagePath + "//Source1.png", byteArray);

        RenderTexture.ReleaseTemporary(renderTexture);
        MainCamera.targetTexture = null;
        ShootScreenShotState = true;

    }

    IEnumerator ReadyToShootScreenShot()
    {
        MainCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 0.5f);

        MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.rotation = Quaternion.Euler(SourceFaceAngle[0],SourceFaceAngle[1],SourceFaceAngle[2]);
        
        MainCamera.clearFlags = CameraClearFlags.Nothing;

        MainCamera.targetTexture = RenderTexture.GetTemporary(1080, 2220, 16);
        yield return new WaitForEndOfFrame();
        TakeShootScreenShot();
    }

    void WriteTargetVertices()
    {
        Vector3[] TargetVertices = new Vector3[vertices.Length];
        Quaternion CacleHeadPose = Quaternion.Euler(SourceFaceAngle[0] - TargetFaceAngle[0], SourceFaceAngle[1] - TargetFaceAngle[1], SourceFaceAngle[2] - TargetFaceAngle[2]);

        FileStream fs = new FileStream(Path + TargetImageName + "_vertices1.txt", FileMode.Create, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);

        for (int i=0;i<vertices.Length; i++)
        {
            TargetVertices[i] = CacleHeadPose * vertices[i];
            sw.WriteLine(Convert.ToString(TargetVertices[i][0]) + ", " + Convert.ToString(TargetVertices[i][1]) + ", " + Convert.ToString(TargetVertices[i][2]));
        }
        sw.Flush();
        sw.Close();
        fs.Close();


        //오류나는 부분
        //Debug.Log(TargetVertices[0][0] + ", " + TargetVertices[0][1] + ", " + TargetVertices[0][2]);

        //ARCoreAugmentedFaceMeshFilter Local = new ARCoreAugmentedFaceMeshFilter();
        //ARCoreAugmentedFaceMeshFilter Local = new ARCoreAugmentedFaceMeshFilter();
        //Vector2[] CacleTargetVertices = new Vector2[vertices.Length];
        //FileStream fst = new FileStream(Path + "987654321" + "_vertices1.txt", FileMode.Create, FileAccess.ReadWrite);
        //StreamWriter swt = new StreamWriter(fst);

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    CacleTargetVertices[i] = Local.LocalToPixelPoint(TargetVertices[i]);
        //    swt.WriteLine(CacleTargetVertices[i][0] + ", " + CacleTargetVertices[i][1]);
        //}
        //swt.Flush();
        //swt.Close();
        //fst.Close();
        ///

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                           