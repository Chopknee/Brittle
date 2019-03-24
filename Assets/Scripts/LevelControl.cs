using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelControl : MonoBehaviour {

    public static LevelControl Instance {
        get {
            return instance;
        }
    }

    private static LevelControl instance;

    public Vector3 keiselStartPosition;
    public bool overrideStartPosition;
    public bool drawStartPosition;
    public CameraModifyTrigger initialCameraBounds;
    //Accessible to all objects.

    public GameObject Keisel;
    public GameObject MainCamera;
    public GameObject Fireflies;
    public GameObject PauseMenu;
    private PauseMenu pauseMenuScript;

    public bool JoystickIsUsed = false;

    public Vector2 MainCameraOrthoSize;
    public float aspectRatio;

    public List<IPausable> pausables;

    private void Awake () {

        if (instance == null || instance != this) {
            instance = this;
        }

        if (Keisel == null) {
            Keisel = GameObject.FindGameObjectWithTag("Player");
        }

        if (MainCamera == null) {
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        if (Fireflies == null) {
            Fireflies = GameObject.FindGameObjectWithTag("Fireflies");
        }

        if (PauseMenu == null) {
            PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        }
        pauseMenuScript = PauseMenu.GetComponent<PauseMenu>();
        pauseMenuScript.OnPaused += OnPaused;
        
        aspectRatio = (float) Screen.width / (float)Screen.height;

        Debug.Log("Aspect ratio: " + aspectRatio);
        MainCameraOrthoSize = new Vector2(MainCamera.GetComponent<Camera>().orthographicSize * aspectRatio, MainCamera.GetComponent<Camera>().orthographicSize);
        Debug.Log(MainCameraOrthoSize);

        var ps = FindObjectsOfType<MonoBehaviour>().OfType<IPausable>();
        foreach (IPausable p in ps) {
            pausables.Add(p);
        }
    }

    // Use this for initialization
    void Start () {

        if (overrideStartPosition) {
            
            if (initialCameraBounds != null) {
                MainCamera.GetComponent<CameraFollow>().SetBounds(initialCameraBounds.minPosition, initialCameraBounds.maxPosition, initialCameraBounds.boundsTransitionTime);
                MainCamera.GetComponent<CameraFollow>().SetZoom(initialCameraBounds.cameraSize, initialCameraBounds.sizeTransitionCurve, initialCameraBounds.zoomTransitionTime);
            }
            //MainCamera.transform.position = new Vector3(Keisel.transform.position.x, Keisel.transform.position.y, MainCamera.transform.position.z);
        } else {
            MainCamera.transform.position = new Vector3(keiselStartPosition.x, keiselStartPosition.y, MainCamera.transform.position.z);
            Keisel.transform.position = keiselStartPosition;
            
        }
	}

    public void Update () {
        MainCameraOrthoSize = new Vector2(MainCamera.GetComponent<Camera>().orthographicSize * aspectRatio, MainCamera.GetComponent<Camera>().orthographicSize);
    }

    public void OnDrawGizmos() {
        
        if (drawStartPosition) {
            if (overrideStartPosition) {
                Gizmos.color = new Color(1, 0.25f, 0);
            } else {
                Gizmos.color = new Color(0.25f, 1, 0);
            }
            Gizmos.DrawSphere(keiselStartPosition, 1f);
        }
    }

    public void OnPaused(bool paused) {
        //Whenever the pause menu is activated, here is where the active objects will be paused
        if (paused) {
            foreach (IPausable p in pausables) {
                p.OnPause();
            }
        } else {
            foreach (IPausable p in pausables) {
                p.OnUnPause();
            }
        }
    }
}
