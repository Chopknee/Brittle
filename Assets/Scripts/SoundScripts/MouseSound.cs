using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSound : MonoBehaviour
{

    AudioSource audioSource;
    public float fadeTime;
    [Range(0, 1)]
    public float targetVolume;
    SmoothTransition transition;
    bool transitioning = false;
    public AnimationCurve fadeCurve;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.Play();
        transition = new SmoothTransition();
        transition.OnFinish += OnTransitionEnd;
    }

    void Update()
    {
        if (transitioning) {
            audioSource.volume = transition.DriveForward();
        }
    }

    void OnMouseDown()
    {
        audioSource.Play();
        transitioning = true;
        transition.Begin(0, targetVolume, fadeCurve, fadeTime);
    }

    void OnMouseUp()
    {
        transitioning = true;
        transition.Begin(targetVolume, 0, fadeCurve, fadeTime);
    }

    void OnTransitionEnd() {

        transitioning = false;

        if (audioSource.volume == 0) {
            audioSource.Stop();
        }
    }
}