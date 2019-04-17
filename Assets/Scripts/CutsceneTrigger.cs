using System;
using System.Collections;
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
    public bool hideFireflies = false;
    public bool animateKeisel = false;
    public bool hideKeisel = false;
    public bool cameraTakeover = false;
    public bool moveKeisel = true;
    
    // Use this for initialization

    public Vector2 KeiselMoveTo;

    public GameObject[] cutsceneObjects;

    //[SerializeField]
    //public ITriggerableBehavior[] myTriggerScripts;

	void Start () {
        if (director == null) {
            director = GetComponent<PlayableDirector>();
            directorDependent = director != null;
        }

        if (dialogCanvas != null && directorDependent == false) {
            dialogCanvas.GetComponentInChildren<CharacterSpeechBox>().OnFinished += Finished;
        }

        if (director == null && dialogCanvas == null) {
            Debug.Log("Trigger is incapable of activating without either a dialog canvas, or director component.");
        }


        if (beginOnActivate && !triggered) {
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
            LevelControl.Instance.Keisel.GetComponent<IPausable>().OnPause();
        }

        if (hideKeisel) {
            LevelControl.Instance.Keisel.SetActive(false);
        }

        if (cameraTakeover) {
            //Make sure the camera can be animated.
            LevelControl.Instance.MainCamera.GetComponent<CameraFollow>().enabled = false;
            LevelControl.Instance.MainCamera.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (dialogCanvas != null) {
            dialogCanvas.SetActive(true);
        }

        if (hideFireflies) {
            LevelControl.Instance.Fireflies.SetActive(false);
        }

        //foreach (ITriggerableBehavior ts in myTriggerScripts) {
        //    ts.Trigger(this);
        //}

        triggered = true;
        LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(false);
    }

    public void Finished() {

        LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(true);

        if (animateKeisel) {
            if (moveKeisel) {
                LevelControl.Instance.Keisel.transform.position = new Vector3(KeiselMoveTo.x + transform.position.x, KeiselMoveTo.y + transform.position.y, LevelControl.Instance.Keisel.transform.position.z);
            }
            LevelControl.Instance.Keisel.GetComponent<IPausable>().OnUnPause();
        }

        if (hideKeisel) {
            LevelControl.Instance.Keisel.SetActive(true);
        }

        if (cameraTakeover) {
            LevelControl.Instance.MainCamera.GetComponent<CameraFollow>().enabled = true;
        }

        if (cutsceneObjects != null) {
            foreach (GameObject g in cutsceneObjects) {
                g.SetActive(false);
            }
        }

        if (directorDependent) {
            director.stopped -= FinishedPlaying;
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

    public void OnDrawGizmos () {
        if (moveKeisel) {
            Gizmos.color = new Color(122, 122, 0);
            Gizmos.DrawSphere(KeiselMoveTo+(Vector2)transform.position, 0.4f);
        }
    }

}