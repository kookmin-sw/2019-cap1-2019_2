using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftArrowController : MonoBehaviour
{
    RawImage leftArrow;

    // Start is called before the first frame update
    void Start()
    {
        leftArrow = gameObject.GetComponent<RawImage>();

        leftArrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if (pose.y > 10) //pitch
        {
            leftArrow.enabled = true;
        }
        else
        {
            leftArrow.enabled = false;
        }
    }
}
