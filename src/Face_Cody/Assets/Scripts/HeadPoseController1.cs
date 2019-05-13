using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPoseController1 : MonoBehaviour
{
    Image downArrow, leftArrow, rightArrow, upArrow;

    // Start is called before the first frame update
    void Start()
    {
        downArrow = GameObject.Find("DownArrow").GetComponent<Image>();
        leftArrow = GameObject.Find("LeftArrow").GetComponent<Image>();
        rightArrow = GameObject.Find("RightArrow").GetComponent<Image>();
        upArrow = GameObject.Find("UpArrow").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if (pose.x < -5) //pitch - down
        {
            //downArrow.SetActive(true);
            //downArrow.sprite = direction[0];
            //leftArrow.sprite = null;
            //rightArrow.sprite = null;
            //upArrow.sprite = null;

            downArrow.enabled = true;
            leftArrow.enabled = false;
            rightArrow.enabled = false;
            upArrow.enabled = false;

        }
        else if (pose.x > 9) //pitch - up
        {
            //downArrow.SetActive(false);
            //downArrow.sprite = null;
            //leftArrow.sprite = null;
            //rightArrow.sprite = null;
            //upArrow.sprite = direction[3];

            downArrow.enabled = false;
            leftArrow.enabled = false;
            rightArrow.enabled = false;
            upArrow.enabled = true;
        }
        else if (pose.y > 10) //yaw
        {
            //downArrow.SetActive(false);
            //downArrow.sprite = null;
            //leftArrow.sprite = direction[1];
            //rightArrow.sprite = null;
            //upArrow.sprite = null;

            downArrow.enabled = false;
            leftArrow.enabled = true;
            rightArrow.enabled = false;
            upArrow.enabled = false;
        }
        else if (pose.y < -10) //yaw
        {
            //downArrow.SetActive(false);
            //downArrow.sprite = null;
            //leftArrow.sprite = null;
            //rightArrow.sprite = direction[2];
            //upArrow.sprite = null;
            downArrow.enabled = false;
            leftArrow.enabled = false;
            rightArrow.enabled = true;
            upArrow.enabled = false;
        }
        else
        {
            //downArrow.SetActive(false);
            //downArrow.sprite = null;
            //leftArrow.sprite = null;
            //rightArrow.sprite = null;
            //upArrow.sprite = null;
            downArrow.enabled = false;
            leftArrow.enabled = false;
            rightArrow.enabled = false;
            upArrow.enabled = false;
        }
    }
}
