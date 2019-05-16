using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPoseController : MonoBehaviour
{
    public Texture[] direction = new Texture[4]; //up, down, left, right
    Color currColor;
    float opaque = 1.0f; //for rawimage color
    float transparent = 0.0f; //for rawimage color
    RawImage headPose;

    // Start is called before the first frame update
    void Start()
    {
        headPose = gameObject.GetComponent<RawImage>();

        currColor = headPose.color;
        currColor.a = transparent;
        headPose.color = currColor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if(pose.x < -5) //pitch
        {
            currColor.a = opaque;
            headPose.color = currColor;
            headPose.texture = direction[0]; //rotate down
        }
        else if (pose.x > 9) //pitch
        {
            currColor.a = opaque;
            headPose.color = currColor;
            headPose.texture = direction[1]; //rotate up
        }
        else if (pose.y > 10) //yaw
        {
            currColor.a = opaque;
            headPose.color = currColor;
            headPose.texture = direction[2]; //rotate left
        }
        else if (pose.y < -10) //yaw
        {
            currColor.a = opaque;
            headPose.color = currColor;
            headPose.texture = direction[3]; //rotate right
        }
        else
        {
            currColor.a = transparent;
            headPose.color = currColor;
            headPose.texture = null;
        }
    }
}
