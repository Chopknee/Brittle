using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSound: MonoBehaviour {

    AudioSource audioSource;
    public float fadeTime;
    [Range(0, 1)]
    public float targetVolume;
    SmoothTransition transition;
    public AnimationCurve fadeCurve;
    public bool canInteract = false;
    public float vol = 0;


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
                transition.Begin(0, targetVolume, fadeCurve, fadeTime);
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Interact")) {
            if (audioSource.isPlaying && audioSource.volume != 0) {
                transition.Begin(audioSource.volume, 0, fadeCurve, fadeTime);
            }
        }

        if (transition.running) {
            audioSource.volume = transition.DriveForward();
        }
        vol = transition.outNumber;
    }

    void OnTransitionEnd () {
        if (transition.end == 0) {
            audioSource.Stop();
        }
    }

    public void ForceStop () {
        transition.Begin(audioSource.volume, 0, fadeCurve, fadeTime);
    }

    //Checks if the mouse/fireflies are inside.
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