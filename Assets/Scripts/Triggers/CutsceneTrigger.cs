using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour {

    public PlayableDirector director;
    //private CharacterSpeechBox dialogBox;


    public bool triggerOnActivation = false;
    public bool hideFireflies = false;
    public bool freezeKeisel = false;
    public bool hideKeisel = false;
    public bool stopCameraFollow = false;
    public bool moveKeiselAfterCutscene = true;

    public float triggerDelay = 0;
    // Use this for initialization

    public Vector2 keiselEndPosition;

    public bool activated = false;
    public bool triggered = false;

    void Start () {
        if (director == null) {
            director = GetComponent<PlayableDirector>();
        }

        if (director == null) {
            Debug.Log("No director component is assigned to a cutscene trigger. GameObject name is " + gameObject.name + ". Trigger will be disabled.");
            triggered = true;
            activated = true;
        }

        if (triggerOnActivation && !activated) {
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

        director.stopped += FinishedPlaying;
        director.Play();

        if (freezeKeisel) {
            LevelControl.Instance.Keisel.GetComponent<IPausable>().OnPause();
        }

        if (hideKeisel) {
            LevelControl.Instance.Keisel.SetActive(false);
        }

        if (stopCameraFollow) {
            //Make sure the camera can be animated.
            LevelControl.Instance.MainCamera.GetComponent<CameraFollow>().enabled = false;
            LevelControl.Instance.MainCamera.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
            if (moveKeiselAfterCutscene) {
                LevelControl.Instance.Keisel.transform.position = new Vector3(keiselEndPosition.x + transform.position.x, keiselEndPosition.y + transform.position.y, LevelControl.Instance.Keisel.transform.position.z);
            }
            LevelControl.Instance.Keisel.GetComponent<IPausable>().OnUnPause();
        }

        if (hideKeisel) {
            LevelControl.Instance.Keisel.SetActive(true);
        }

        if (stopCameraFollow) {
            LevelControl.Instance.MainCamera.GetComponent<CameraFollow>().enabled = true;
        }

        if (hideFireflies) {
            LevelControl.Instance.Fireflies.SetActive(true);
        }

        
    }

    public void FinishedPlaying(PlayableDirector dir) {
        Finished();
    }

    private void OnDrawGizmos () {
        if (moveKeiselAfterCutscene) {
            Gizmos.color = new Color(122, 122, 0);
            Gizmos.DrawSphere(keiselEndPosition + (Vector2) transform.position, 0.4f);
        }
        if (LevelControl.showCutsceneTriggers) {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = LevelControl.cutsceneTriggerColor;//col;
            BoxCollider2D bc = GetComponent<BoxCollider2D>();
            Gizmos.DrawWireCube((Vector3) bc.offset, bc.size);
        }
    }

}