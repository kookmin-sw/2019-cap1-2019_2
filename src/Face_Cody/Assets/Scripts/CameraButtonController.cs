using UnityEngine;
//using System.Collections;

public class CameraButtonController : MonoBehaviour
{
    //private AudioSource cameraAudio;
    private GameObject faceOccluder;
    private GameObject screenCapturer;

    void Start()
    {
        //cameraAudio = GetComponent<AudioSource>();
        faceOccluder = GameObject.Find("FaceOccluder");
        screenCapturer = GameObject.Find("ScreenCapturer");
    }

    public void OnClick()
    {
        //cameraAudio.Play();
        string name = System.DateTime.Now.ToString("yyMMdd_HHmmss");
        screenCapturer.GetComponent<ScreenCaptureController>().ScreenCapture(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveMeshInfo(name);
        faceOccluder.GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().SaveTextureInfo(name);
    }
}
