using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class upload_button : MonoBehaviour
{
    public void onClick()
    {
        uploadFiles();
        //SceneManager.LoadScene("upload");
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

        string path = Application.persistentDataPath + "/My_ScreenShot/";
        string name = path + img_data.filename + "_down.png";

        File.WriteAllBytes(name, w.bytes);
    }
}
