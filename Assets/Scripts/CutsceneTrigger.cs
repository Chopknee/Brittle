using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour {

    public PlayableDirector director;
    public bool triggered = false;
	// Use this for initialization
	void Start () {
		if (director == null) {
            director = GetComponent<PlayableDirector>();
        }
	}

    public void OnTriggerEnter2D ( Collider2D collision ) {
        if (!triggered) {
            if (collision.gameObject.tag == "Player") {
                triggered = true;
                director.Play();
            }
        }
    }
}