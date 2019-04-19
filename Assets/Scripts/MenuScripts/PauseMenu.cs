using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject BookPanel;
    private Animator bookPanelAnimator;

    public delegate void Pause ( bool paused );
    public Pause OnPaused;

    public bool Paused = false;

    private bool canwork = true;

    public Button resume;
    public Button quit;

	// Use this for initialization
	void Start () {
		if (BookPanel == null) {
            Debug.Log("Missing book panel object for pause menu!! No pause menu will be available.");
            return;
        }
        bookPanelAnimator = BookPanel.GetComponent<Animator>();
        canwork = true;

        resume.onClick.AddListener(Resume);
        quit.onClick.AddListener(Quit);
	}
	
	// Update is called once per frame
	void Update () {
        if (canwork) {
            if (Input.GetButtonDown("Cancel")) {
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

    public void Resume() {
        Debug.Log("Resume clicked!");
        if (canwork) {
            Paused = !Paused;
            bookPanelAnimator.SetTrigger("Close");
            if (OnPaused != null) {
                OnPaused(Paused);
            }
            canwork = false;
            Invoke("UnUnwork", 2);
        }
    }

    public void Quit() {
        SceneManager.LoadScene("MainMenu");
    }
}
