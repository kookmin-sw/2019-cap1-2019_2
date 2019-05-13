using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightArrowController : MonoBehaviour
{
    RawImage rightArrow;

    // Start is called before the first frame update
    void Start()
    {
        rightArrow = gameObject.GetComponent<RawImage>();

        rightArrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if (pose.y < -10) //pitch
        {
            rightArrow.enabled = true;
        }
        else
        {
            rightArrow.enabled = false;
        }
    }
}
