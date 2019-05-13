using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpArrowController : MonoBehaviour
{
    RawImage upArrow; 

    // Start is called before the first frame update
    void Start()
    {
        upArrow = gameObject.GetComponent<RawImage>();

        upArrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if (pose.x > 9) //pitch
        {
            upArrow.enabled = true;
        }
        else
        {
            upArrow.enabled = false;
        }
    }
}
