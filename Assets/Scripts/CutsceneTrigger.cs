using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour {

    public PlayableDirector director;
    public bool triggered = false;

    public bool animateKeisel = false;
    public bool cameraTakeover = false;
    // Use this for initialization

    public Vector2 KeiselMoveTo;

    public GameObject[] cutsceneObjects;

	void Start () {
		if (director == null) {
            director = GetComponent<PlayableDirector>();
        }
	}

    public void OnTriggerEnter2D ( Collider2D collision ) {
        if (!triggered) {
            if (collision.gameObject.tag == "Player") {
                director.stopped += FinishedPlaying;
                triggered = true;
                if (animateKeisel) {
                    //Make sure keisel is set to the correct animation state.
                    LevelControl.Keisel.SetActive(false);

                }
                if (cameraTakeover) {
                    //Make sure the camera can be animated.
                    LevelControl.MainCamera.GetComponent<CameraFollow>().enabled = false;
                }
                director.Play();
            }
        }
    }

    public void FinishedPlaying(PlayableDirector dir) {
        if (animateKeisel) {
            LevelControl.Keisel.transform.position = new Vector3(KeiselMoveTo.x+transform.position.x, KeiselMoveTo.y+transform.position.y, LevelControl.Keisel.transform.position.z);
            LevelControl.Keisel.SetActive(true);
        }

        if (cameraTakeover) {
            LevelControl.MainCamera.GetComponent<CameraFollow>().enabled = true;
        }

        foreach (GameObject g in cutsceneObjects) {
            g.SetActive(false);
        }
        

        director.stopped -= FinishedPlaying;
    }

    public void OnDrawGizmos () {
        if (KeiselMoveTo != null) {
            Gizmos.color = new Color(122, 122, 0);
            Gizmos.DrawSphere(KeiselMoveTo+(Vector2)transform.position, 0.4f);
        }
    }

}