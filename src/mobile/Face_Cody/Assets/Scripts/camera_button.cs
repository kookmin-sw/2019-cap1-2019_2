using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class camera_button : MonoBehaviour
{
    private Camera camera;
    private GameObject faceOccluder;
    private string path;
    private string path2;
    private string path3;
    

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        camera = GameObject.Find("First Person Camera").GetComponent<Camera>();
        faceOccluder = GameObject.Find("FaceOccluder");
        path = Application.persistentDataPath + "/My_ScreenShot/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

    }

    public void onClick()
    {
        faceOccluder.SendMessage("SaveMeshInfo", SendMessageOptions.DontRequireReceiver);
        faceOccluder.SendMessage("SaveTextureInfo", SendMessageOptions.DontRequireReceiver);
        ScreenCapture();
        uploadFiles();
        //SceneManager.LoadScene("upload");
    }

    public void ScreenCapture()
    {
        string name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        camera.targetTexture = rt;
        Texture2D screen_shot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //Rect rect = new Rect(0, 0, screen_shot.width, screen_shot.height);
        camera.Render();
        RenderTexture.active = rt;
        screen_shot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screen_shot.Apply();

        img_data.bytes = screen_shot.EncodeToPNG();
        File.WriteAllBytes(name, img_data.bytes);
        camera.targetTexture = null;
    }

    public void uploadFiles()
    {
        StartCoroutine("UploadFiles");
    }

    IEnumerator UploadFiles()
    {
        string path2 = Application.persistentDataPath + "/My_Log/texture/";
        string path3 = Application.persistentDataPath + "/My_Log/mesh/";
        string path_tt = path2 + img_data.filename + "_triangles.txt";
        string path_tu = path2 + img_data.filename + "_uvs.txt";
        string path_tv = path2 + img_data.filename + "_vertices.txt";
        string path_mt = path3 + img_data.filename + "_triangles.txt";
        string path_mu = path3 + img_data.filename + "_uvs.txt";
        string path_mv = path3 + img_data.filename + "_vertices.txt";

        byte[] data_tt = File.ReadAllBytes(path_tt);
        byte[] data_tu = File.ReadAllBytes(path_tu);
        byte[] data_tv = File.ReadAllBytes(path_tv);
        byte[] data_mt = File.ReadAllBytes(path_mt);
        byte[] data_mu = File.ReadAllBytes(path_mu);
        byte[] data_mv = File.ReadAllBytes(path_mv);

        WWWForm form = new WWWForm();

        Debug.Log("form created ");



        form.AddBinaryData("data_img", img_data.bytes, img_data.filename + ".png", "image/png");

        form.AddBinaryData("data_tt", data_tt, img_data.filename + "_texture_triangles.txt", "text/txt");

        form.AddBinaryData("data_tu", data_tu, img_data.filename + "_texture_uvs.txt", "text/txt");

        form.AddBinaryData("data_tv", data_tv, img_data.filename + "_texture_vertices.txt", "text/txt");

        form.AddBinaryData("data_mt", data_mt, img_data.filename + "_mesh_triangles.txt", "text/txt");

        form.AddBinaryData("data_mu", data_mu, img_data.filename + "_mesh_uvs.txt", "text/txt");

        form.AddBinaryData("data_mv", data_mv, img_data.filename + "_mesh_vertices.txt", "text/txt");



        WWW w = new WWW("http://192.168.35.152:8000/mediatest/upload", form);
        print("www created");

        yield return w;

        
        string downname = path + img_data.filename + "_down.png";

        File.WriteAllBytes(downname, w.bytes);
    }

}
