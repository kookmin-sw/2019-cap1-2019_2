using UnityEngine;

public class CameraButtonController : MonoBehaviour
{
    private GameObject cameraAudio;
    private GameObject faceOccluder;
    private GameObject screenCapturer;

    void Start()
    {
        cameraAudio = GameObject.Find("CameraAudio");
        faceOccluder = GameObject.Find("FaceOccluder");
        screenCapturer = GameObject.Find("ScreenCapturer");
    }

    public void OnClick()
    {
        string name = System.DateTime.Now.ToString("yyMMdd_HHmmss");
        screenCapturer.GetComponent<ScreenCaptureController>().ScreenCapture(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveMeshInfo(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveTextureInfo(name);
        cameraAudio.GetComponent<CameraAudioController>().playSound();
    }
}
