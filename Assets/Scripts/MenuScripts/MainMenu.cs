using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject BackgroundObjectsFade;
    //private 
    public Button btnStart;
    public Button btnOptions;
    public Button btnExit;
    public Button btnCloseOptions;

    private Animator CanvasAnimator;
    private bool locked = false;
    // Use this for initialization
    void Start () {

        if (btnStart == null || btnOptions == null || btnExit == null || BackgroundObjectsFade == null) {
            Debug.Log("Main menu can't work because some of the components have not been assigned!");
            gameObject.SetActive(false);
            return;
        }

        btnStart.onClick.AddListener(StartLoading);
        
        btnExit.onClick.AddListener(Exit);
        btnOptions.onClick.AddListener(OpenOptions);
        btnCloseOptions.onClick.AddListener(CloseOptions);
        CanvasAnimator = GetComponent<Animator>();

        btnStart.Select();
    }

    private void Exit () {
        Application.Quit();
    }

    private void StartLoading () {
        if (!locked) {
            CanvasAnimator.SetTrigger("Load");
            StartCoroutine(LoadLevelOne());
            locked = true;
        }
    }

    private void OpenOptions () {
        if (!locked) {
            CanvasAnimator.SetTrigger("OpenOptions");
            locked = true;
            Invoke("Unlock", 2);
            btnCloseOptions.Select();
        }
    }

    private void CloseOptions() {
        if (!locked) {
            CanvasAnimator.SetTrigger("CloseOptions");
            locked = true;
            Invoke("Unlock", 2);
            btnOptions.Select();
        }
    }

    private void Unlock() {
        locked = false;
    }

    IEnumerator LoadLevelOne() {
        yield return new WaitForSeconds(3f);

        AsyncOperation async = SceneManager.LoadSceneAsync("levelOne");
        async.allowSceneActivation = false;
        while (!async.isDone) {
            CanvasAnimator.SetTrigger("Finished");
            yield return new WaitForSeconds(2f);
            async.allowSceneActivation = true;
            yield return null;
        }

    }
}
