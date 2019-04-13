﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour {

    public PlayableDirector director;
    private bool directorDependent = true;
    public GameObject dialogCanvas;
    //private CharacterSpeechBox dialogBox;

    public bool triggered = false;
    public bool beginOnActivate = false;

    public bool animateKeisel = false;
    public bool cameraTakeover = false;
    public bool moveKeisel = true;
    // Use this for initialization

    public Vector2 KeiselMoveTo;

    public GameObject[] cutsceneObjects;

	void Start () {
        if (director == null) {
            director = GetComponent<PlayableDirector>();
            directorDependent = false;
        }

        if (dialogCanvas != null && directorDependent == false) {
            dialogCanvas.GetComponentInChildren<CharacterSpeechBox>().OnFinished += Finished;
        }

        if (director == null && dialogCanvas == null) {
            Debug.Log("Trigger is incapable of activating without either a dialog canvas, or director component.");
        }


        if (beginOnActivate) {
            Trigger();
            Debug.Log("Triggering cutscene!");
        }

	}

    public void OnTriggerEnter2D ( Collider2D collision ) {
        if (!triggered) {
            if (collision.gameObject.tag == "Player") {
                Trigger();
            }
            
        }
    }

    public void Trigger () {
        if (directorDependent) {
            director.stopped += FinishedPlaying;
            director.Play();
        }
        if (animateKeisel) {
            //Make sure keisel is set to the correct animation state.
            LevelControl.Instance.Keisel.SetActive(false);
        }

        if (cameraTakeover) {
            //Make sure the camera can be animated.
            LevelControl.Instance.MainCamera.GetComponent<CameraFollow>().enabled = false;
        }

        if (dialogCanvas != null) {
            dialogCanvas.SetActive(true);
        }

        triggered = true;
        LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(false);
    }

    public void Finished() {

        if (animateKeisel) {
            if (moveKeisel) {
                LevelControl.Instance.Keisel.transform.position = new Vector3(KeiselMoveTo.x + transform.position.x, KeiselMoveTo.y + transform.position.y, LevelControl.Instance.Keisel.transform.position.z);
            }
            LevelControl.Instance.Keisel.SetActive(true);
        }

        if (cameraTakeover) {
            LevelControl.Instance.MainCamera.GetComponent<CameraFollow>().enabled = true;
        }

        foreach (GameObject g in cutsceneObjects) {
            g.SetActive(false);
        }
        LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(true);

        director.stopped -= FinishedPlaying;

        if (dialogCanvas != null) {
            dialogCanvas.SetActive(false);
        }
    }

    public void FinishedPlaying(PlayableDirector dir) {
        Finished();
    }

    public void OnDrawGizmos () {
        if (moveKeisel) {
            Gizmos.color = new Color(122, 122, 0);
            Gizmos.DrawSphere(KeiselMoveTo+(Vector2)transform.position, 0.4f);
        }
    }

}