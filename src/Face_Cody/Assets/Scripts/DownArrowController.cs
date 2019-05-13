using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownArrowController : MonoBehaviour
{
    RawImage downArrow;
    // Start is called before the first frame update
    void Start()
    {
        downArrow = gameObject.GetComponent<RawImage>();

        downArrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if (pose.x < -5) //pitch
        {
            downArrow.enabled = true;
        }
        else
        {
            downArrow.enabled = false;
        }
    }
}
