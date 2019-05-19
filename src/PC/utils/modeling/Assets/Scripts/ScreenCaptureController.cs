using UnityEngine;
using System.IO;

public class ScreenCaptureController : MonoBehaviour
{
    Camera camera;

    void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void ScreenCapture(string fileName)
    {
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture.active = rt;

        camera.targetTexture = rt;
        camera.Render();
        camera.targetTexture = null;

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        RenderTexture.active = null;

        byte[] imageBytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(string.Format("{0}/{1}.png", Global.texturePath, fileName), imageBytes);
    }
}
