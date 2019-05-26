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
        SceneManager.LoadScene("gallery");
    }
    public void MoveToSynthesis()
    {
        Global.selectMode = 0;
        SceneManager.LoadScene("synthesis");
    }
    public void MoveToCamera()
    {
        Global.selectMode = 0;
        SceneManager.LoadScene("camera");
    }
}
