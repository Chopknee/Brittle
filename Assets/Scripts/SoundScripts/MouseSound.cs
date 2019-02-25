using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSound : MonoBehaviour
{

    AudioSource audioSource;
    public float FadeTime;
    public float targetVolume;
    private float modify = 0;
    private float t = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    void Update()
    {
        //Fade out
        if (modify < 0)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(targetVolume, 0, t / FadeTime);
            if (t >= FadeTime)
            {
                modify = 0;
                t = 0;
            }
            //Fade in
        }
        else if (modify > 0)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, targetVolume, t / FadeTime);
            if (t >= FadeTime)
            {
                modify = 0;
                t = 0;
            }
        }
    }

    void OnMouseDown()
    {
        modify = 1;
    }

    void OnMouseUp()
    {
        modify = -1;
    }
}