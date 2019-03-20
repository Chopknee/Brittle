using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LogDropActivation : MonoBehaviour {

    public GameObject ActivateAfterFinished;

    public void OnMouseDown () {
        GetComponent<PlayableDirector>().stopped += OnFin;
        GetComponent<PlayableDirector>().Play();
        if (GetComponent<SpriteHighlight>() != null) {
            GetComponent<SpriteHighlight>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Update () {
        
    }

    public void OnFin( PlayableDirector dir ) {
        ActivateAfterFinished.SetActive(true);
        dir.stopped -= OnFin;
        Destroy(gameObject);
    }
}
