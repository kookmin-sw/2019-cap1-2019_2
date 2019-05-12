using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            MoveToCamera();
        }
    }
    public void MoveToGallery()
    {
        SceneManager.LoadScene("Gallery");
    }
    public void MoveToSynthesis()
    {
        SceneManager.LoadScene("Synthesis");
    }
    public void MoveToCamera()
    {
        SceneManager.LoadScene("camera");
    }
}
