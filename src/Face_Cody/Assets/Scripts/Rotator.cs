using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 angle = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        angle.z -= Time.deltaTime * 240f;
        transform.rotation = Quaternion.Euler(angle);
    }
}
