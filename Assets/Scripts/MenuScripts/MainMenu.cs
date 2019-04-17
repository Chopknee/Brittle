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

    private Animator backgroundObjectsAnimator;
    private Animator CanvasAnimator;

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
        backgroundObjectsAnimator = BackgroundObjectsFade.GetComponent<Animator>();
        CanvasAnimator = GetComponent<Animator>();
    }

    private void Exit () {
        Application.Quit();
    }

    private void StartLoading () {
        backgroundObjectsAnimator.SetTrigger("StartLoading");
        CanvasAnimator.SetTrigger("StartLoading");
        StartCoroutine(LoadLevelOne());
    }

    private void OpenOptions () {

    }

    IEnumerator LoadLevelOne() {
        yield return new WaitForSeconds(3f);

        AsyncOperation async = SceneManager.LoadSceneAsync("levelOne");
        async.allowSceneActivation = false;
        while (!async.isDone) {
            CanvasAnimator.SetTrigger("FadeToBlack");
            yield return new WaitForSeconds(2f);
            async.allowSceneActivation = true;
            yield return null;
        }

    }
}
