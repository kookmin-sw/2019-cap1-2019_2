using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPoseController1 : MonoBehaviour
{
    GameObject downArrow, leftArrow, rightArrow, upArrow;

    // Start is called before the first frame update
    void Start()
    {
        downArrow = GameObject.Find("DownArrow");
        leftArrow = GameObject.Find("LeftArrow");
        rightArrow = GameObject.Find("RightArrow");
        upArrow = GameObject.Find("UpArrow");
        downArrow.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        upArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if (pose.x < -5) //pitch - down
        {
            downArrow.SetActive(true);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            upArrow.SetActive(false);
        }
        else if (pose.x > 9) //pitch - up
        {
            downArrow.SetActive(false);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            upArrow.SetActive(true);
        }
        else if (pose.y > 10) //yaw
        {
            downArrow.SetActive(false);
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
            upArrow.SetActive(false);
        }
        else if (pose.y < -10) //yaw
        {
            downArrow.SetActive(false);
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
            upArrow.SetActive(false);
        }
        else
        {
            downArrow.SetActive(false);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            upArrow.SetActive(false);
        }
    }
}
