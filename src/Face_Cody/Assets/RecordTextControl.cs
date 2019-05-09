using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTextControl : MonoBehaviour
{
    public GameObject recordText;
    public static RecordTextControl instance;


    void Awake()
    {
        if (RecordTextControl.instance == null)
            RecordTextControl.instance = this;
    }
    // Use this for initialization
    void Start()
    {
        recordText.SetActive(true);
        StartCoroutine(ShowReady());
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 3)
        {
            recordText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            recordText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

}
