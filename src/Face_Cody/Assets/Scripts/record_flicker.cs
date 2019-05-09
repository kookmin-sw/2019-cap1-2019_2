using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class record_flicker : MonoBehaviour
{
    public GameObject record;
    public Button video_button;
    public static record_flicker instance;

    void Awake()
    {
        video_button = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        record.SetActive(false);
    }

    public void OnClick()
    {
        StartCoroutine(flick());
    }

    IEnumerator flick()
    {
        int count = 0;
        while(count < 6)
        {
            record.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            record.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
