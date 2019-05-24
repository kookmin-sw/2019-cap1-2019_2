using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    float angle = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        angle += Time.deltaTime * -30.0f;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
    }
}
