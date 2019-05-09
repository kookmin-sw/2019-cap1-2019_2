using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPoseControl : MonoBehaviour
{
    public Texture[] direction = new Texture[4]; //up, down, left, right
    Color currColor;
    float opaque = 1.0f; //for rawimage color
    float transparent = 0.0f; //for rawimage color
    RawImage headpose;

    // Start is called before the first frame update
    void Start()
    {
        headpose = gameObject.GetComponent<RawImage>();

        currColor = headpose.color;
        currColor.a = transparent;
        headpose.color = currColor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();

        if(pose.x < -5) //pitch
        {
            currColor.a = opaque;
            headpose.color = currColor;
            headpose.texture = direction[0]; //rotate down
        }
        else if (pose.x > 9) //pitch
        {
            currColor.a = opaque;
            headpose.color = currColor;
            headpose.texture = direction[1]; //rotate up
        }
        else if (pose.y > 10) //yaw
        {
            currColor.a = opaque;
            headpose.color = currColor;
            headpose.texture = direction[2]; //rotate left
        }
        else if (pose.y < -10) //yaw
        {
            currColor.a = opaque;
            headpose.color = currColor;
            headpose.texture = direction[3]; //rotate right
        }
        else
        {
            currColor.a = transparent;
            headpose.color = currColor;
            headpose.texture = null;
        }
    }
}
