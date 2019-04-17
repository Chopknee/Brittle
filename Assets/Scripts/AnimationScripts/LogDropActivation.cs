using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LogDropActivation : MonoBehaviour {

    public GameObject ActivateAfterFinished;

    public bool CanInteract = false;
    public bool triggered = false;

    public void Update () {
        if (!triggered) {
            if (CanInteract) {
                if (Input.GetButtonDown("Interact")) {
                    GetComponent<PlayableDirector>().stopped += OnFin;
                    GetComponent<PlayableDirector>().Play();
                    if (GetComponent<SpriteHighlight>() != null) {
                        GetComponent<SpriteHighlight>().enabled = false;
                        transform.GetChild(0).gameObject.SetActive(false);
                    }
                    triggered = true;
                    LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(false);
                }
            }
        }
    }

    public void OnFin( PlayableDirector dir ) {
        ActivateAfterFinished.SetActive(true);
        dir.stopped -= OnFin;
        Destroy(gameObject);
        LevelControl.Instance.PauseMenu.GetComponent<PauseMenu>().SetCanPause(true);
    }

    public void OnTriggerEnter2D ( Collider2D collision ) {
        if (collision.gameObject.tag == "Fireflies") {
            CanInteract = true;
        }
    }

    public void OnTriggerExit2D ( Collider2D collision ) {
        if (collision.gameObject.tag == "Fireflies") {
            CanInteract = false;
        }
    }

}
