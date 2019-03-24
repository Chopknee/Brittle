using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSound: MonoBehaviour {

    AudioSource audioSource;
    public float fadeTime;
    [Range(0, 1)]
    public float targetVolume;
    SmoothTransition transition;
    bool transitioning = false;
    public AnimationCurve fadeCurve;
    public bool canInteract = false;
    void Start () {
        audioSource = GetComponent<AudioSource>();
        //audioSource.Play();
        transition = new SmoothTransition();
        transition.OnFinish += OnTransitionEnd;
    }

    void Update () {

        if (canInteract) {
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Interact")) {
                audioSource.Play();
                transitioning = true;
                transition.Begin(0, targetVolume, fadeCurve, fadeTime);
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Interact")) {
            if (!transitioning && audioSource.volume != 0) {
                transitioning = true;
                transition.Begin(targetVolume, 0, fadeCurve, fadeTime);
            }
        }

        if (transitioning) {
            audioSource.volume = transition.DriveForward();
        }
    }

    void OnTransitionEnd () {

        transitioning = false;

        if (audioSource.volume == 0) {
            audioSource.Stop();
        }
    }

    public void ForceStop () {
        transitioning = true;
        transition.Begin(targetVolume, 0, fadeCurve, fadeTime);
    }

    public void OnTriggerEnter2D ( Collider2D collision ) {
        if (collision.gameObject.tag == "Fireflies") {
            canInteract = true;
        }
    }

    public void OnTriggerExit2D ( Collider2D collision ) {
        if (collision.gameObject.tag == "Fireflies") {
            canInteract = false;
        }
    }
}