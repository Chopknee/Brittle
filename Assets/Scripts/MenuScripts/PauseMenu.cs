using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject BookPanel;
    private Animator bookPanelAnimator;

    public delegate void Pause ( bool paused );
    public Pause OnPaused;

    public bool Paused = false;

    private bool canwork = true;

	// Use this for initialization
	void Start () {
		if (BookPanel == null) {
            Debug.Log("Missing book panel object for pause menu!! No pause menu will be available.");
            return;
        }
        bookPanelAnimator = BookPanel.GetComponent<Animator>();
        canwork = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (canwork) {
            if (Input.GetButtonDown("Cancel")) {
                Debug.Log("Pause Presed!");
                Paused = !Paused;
                canwork = false;
                if (Paused) {
                    bookPanelAnimator.SetTrigger("Open");
                    if (OnPaused != null) {
                        OnPaused(Paused);
                    }
                } else {
                    bookPanelAnimator.SetTrigger("Close");
                }
                Invoke("UnUnwork", 2);
            }
        }
    }

    public void UnUnwork() {
        canwork = true;
        if (!Paused) {
            if (OnPaused != null) {
                OnPaused(Paused);
            }
        }
    }

    public void SetCanPause(bool canPause) {
        canwork = canPause;
    }
}
