using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.fontSize = 36;
        text.color = Color.red;
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pose = GameObject.Find("FaceOccluder").GetComponent<GoogleARCore.Examples.AugmentedFaces.ARCoreAugmentedFaceMeshFilter>().GetHeadPose();
        text.text = string.Format("Pitch:\t{0}\nYaw :\t{1}\nRoll  :\t{2}", pose.x, pose.y, pose.z);
    }
}
