using UnityEngine;

public class CameraButtonController : MonoBehaviour
{
    private GameObject cameraAudio;
    private GameObject faceOccluder;
    private GameObject screenCapturer;
    private GameObject light;
    void Start()
    {
        cameraAudio = GameObject.Find("CameraAudio");
        faceOccluder = GameObject.Find("FaceOccluder");
        screenCapturer = GameObject.Find("ScreenCapturer");
        light = GameObject.Find("Environmental Light");
    }

    public void OnClick()
    {
        string name = System.DateTime.Now.ToString("yyMMdd_HHmmss");
        screenCapturer.GetComponent<ScreenCaptureController>().ScreenCapture(name);
        light.GetComponent<GoogleARCore.EnvironmentalLight>().SaveLightInfo(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveMeshInfo(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveTextureInfo(name);
        cameraAudio.GetComponent<CameraAudioController>().playSound();
    }
}
