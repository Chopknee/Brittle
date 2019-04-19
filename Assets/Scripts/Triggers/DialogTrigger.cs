using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogTrigger : MonoBehaviour {

    public GameObject dialogCanvas;
    //private CharacterSpeechBox dialogBox;


    public bool beginOnActivate = false;
    public bool hideFireflies = false;
    public bool freezeKeisel = false;
    public bool hideKeisel = false;

    public float triggerDelay = 0;
    // Use this for initialization

    public bool activated = false;
    public bool triggered = false;

    void Start () {

        if (dialogCanvas == null || dialogCanvas.GetComponentInChildren<CharacterSpeechBox>() == null) {
            Debug.Log("No character speech box script is assigned to a dialog trigger. GameObject name is " + gameObject.name + ". Trigger will be disabled.");
            return;
        }

        dialogCanvas.GetComponentInChildren<CharacterSpeechBox>().OnFinished += Finished;

        if (beginOnActivate && !activated) {
            activated = true;
        }

	}

    public void OnTriggerEnter2D ( Collider2D collision ) {
        if (!activated) {
            if (collision.gameObject.tag == "Player") {
                activated = true;
            }
            
        }
    }

    public void Trigger () {
        
        if (freezeKeisel) {
            LevelControl.Instance.Keisel.GetComponent<IPausable>().OnPause();
        }

        if (hideKeisel) {
            LevelControl.Instance.Keisel.SetActive(false);
        }

        if (dialogCanvas != null) {
            dialogCanvas.SetActive(true);
        }

        if (hideFireflies) {
            LevelControl.Instance.Fireflies.SetActive(false);
        }

        activated = true;
        LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(false);
    }

    private float t = 0;

    public void Update () {
        if (activated && !triggered) {
            t += Time.deltaTime;
            if (t > triggerDelay) {
                Trigger();
                triggered = true;
            }
        }
    }

    public void Finished() {

        LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(true);

        if (freezeKeisel) {
            LevelControl.Instance.Keisel.GetComponent<IPausable>().OnUnPause();
        }

        if (hideKeisel) {
            LevelControl.Instance.Keisel.SetActive(true);
        }

        if (dialogCanvas != null) {
            dialogCanvas.SetActive(false);
        }

        if (hideFireflies) {
            LevelControl.Instance.Fireflies.SetActive(true);
        }
    }

    public void FinishedPlaying(PlayableDirector dir) {
        Finished();
    }
}