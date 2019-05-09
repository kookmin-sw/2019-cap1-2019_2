using UnityEngine;

public class CameraButtonController : MonoBehaviour
{
    private GameObject faceOccluder;
    private GameObject screenCapturer;

    void Start()
    {
        faceOccluder = GameObject.Find("FaceOccluder");
        screenCapturer = GameObject.Find("ScreenCapturer");
    }

    public void onClick()
    {
        string name = System.DateTime.Now.ToString("yyMMdd_HHmmss");
        screenCapturer.GetComponent<ScreenCaptureController>().ScreenCapture(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveMeshInfo(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveTextureInfo(name);
    }
}
