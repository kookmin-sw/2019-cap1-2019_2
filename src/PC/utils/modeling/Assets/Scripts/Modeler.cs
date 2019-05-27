using UnityEngine;
using System.IO;

public class Modeler : MonoBehaviour
{
    Mesh mesh;
    Camera camera;
    Light light;
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        light = GameObject.Find("PointLight").GetComponent<Light>();
    }

    void LoadLightData(string fileName)
    {
        float lightFactor = 10.0f;

        string[] lines = File.ReadAllLines(string.Format("{0}/{1}_light.txt", Global.meshPath, fileName));
        float intensity = float.Parse(lines[0]);
        light.range = intensity * lightFactor;
    }
    void LoadMeshData(string fileName)
    {
        string[] lines = File.ReadAllLines(string.Format("{0}/{1}_vertices.txt", Global.meshPath, fileName));
        Vector3[] vertices = new Vector3[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(' ');
            vertices[i] = new Vector3(float.Parse(line[0]), float.Parse(line[1]), float.Parse(line[2]));
        }

        lines = File.ReadAllLines(string.Format("{0}/uvs.txt", Global.meshPath));
        Vector2[] uvs = new Vector2[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(' ');
            uvs[i] = new Vector2(float.Parse(line[0]), float.Parse(line[1]));
        }

        lines = File.ReadAllLines(string.Format("{0}/triangles.txt", Global.meshPath));
        int[] triangles = new int[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            triangles[i] = int.Parse(lines[i]);
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void AttachTexture(string fileName)
    {
        byte[] bytes = File.ReadAllBytes(string.Format("{0}/{1}_unwrapped.png", Global.meshPath, fileName));
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);

        Renderer renderer = GetComponent<MeshRenderer>();
        renderer.materials[0].mainTexture = texture;
    }

    Vector3 LoadHeadPose(string fileName)
    {
        string[] lines = File.ReadAllLines(string.Format("{0}/{1}_headPose.txt", Global.meshPath, fileName));
        string[] line = lines[0].Split(' ');
        transform.position = new Vector3(float.Parse(line[0]), float.Parse(line[1]), float.Parse(line[2]));

        line = lines[1].Split(' ');
        return new Vector3(-float.Parse(line[0]), -float.Parse(line[1]), -float.Parse(line[2]));
    }

    void RotationMesh(Vector3 angles)
    {
        angles -= transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(angles);
    }

    Vector2 LocalToPixelPoint(Vector3 coord)
    {
        // local space to world space
        coord = transform.TransformPoint(coord);
        // world space to screen space
        coord = camera.WorldToScreenPoint(coord);
        // screen space to pixel space
        return new Vector2(coord.x, Screen.height - coord.y);
    }

    void SaveUpdatedHeadPose(string fileName)
    {
        Vector2 center = LocalToPixelPoint(transform.position);
        FileStream fs = new FileStream(string.Format("{0}/{1}_headPose.txt", Global.texturePath, fileName), FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);

        sw.WriteLine(string.Format("{0} {1}", center.x, center.y));

        sw.Close();
        fs.Close();
    }

    void SaveRotatedVertices(string fileName)
    {
        //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        FileStream fs = new FileStream(string.Format("{0}/{1}_vertices.txt", Global.texturePath, fileName), FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);

        foreach (Vector3 vertex in mesh.vertices)
        {
            Vector2 coord = LocalToPixelPoint(vertex);
            sw.WriteLine("{0} {1}", coord.x, coord.y);
        }

        sw.Close();
        fs.Close();
    }

    public void DrawMesh()
    {
        LoadLightData(Global.targetName);
        LoadMeshData(Global.sourceName);
        AttachTexture(Global.sourceName);
        RotationMesh(LoadHeadPose(Global.targetName) - LoadHeadPose(Global.sourceName));
        SaveUpdatedHeadPose(Global.sourceName);
        SaveRotatedVertices(Global.sourceName);
        GameObject.Find("ScreenCapturer").GetComponent<ScreenCaptureController>().ScreenCapture(Global.sourceName);
    }
}
