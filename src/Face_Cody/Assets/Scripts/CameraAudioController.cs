using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudioController : MonoBehaviour
{
    AudioSource cameraAudio;

    // Start is called before the first frame update
    void Start()
    {
        cameraAudio = GetComponent<AudioSource>();
        GetComponent<AudioSource>().Stop();
    }

    public void playSound()
    {
        
        cameraAudio.Play();
    }
}
