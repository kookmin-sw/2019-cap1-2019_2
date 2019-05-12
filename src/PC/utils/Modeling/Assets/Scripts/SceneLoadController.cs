using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        ApplicationLifeCycle();
    }
    public void ToGallery()
    {
        SceneManager.LoadScene("Gallery");
    }
    public void ToSynthesis()
    {
        SceneManager.LoadScene("Synthesis");
    }
    private void ApplicationLifeCycle()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("face_cody");
        }
    }
}
