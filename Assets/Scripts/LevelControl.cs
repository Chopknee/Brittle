using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LevelControl : MonoBehaviour {

    public static Color cameraColliderColor = new Color(0, 0, 1);
    public static bool showCameraColliders = true;

    public static Color cutsceneTriggerColor = new Color(1, 0, 1);
    public static bool showCutsceneTriggers = true;

    public static LevelControl Instance {
        get {
            return instance;
        }
    }

    private static LevelControl instance;

    public Vector3 keiselStartPosition;
    public bool overrideStartPosition;
    public bool drawStartPosition;
    //Accessible to all objects.

    public GameObject Keisel;
    public GameObject MainCamera;
    public GameObject Fireflies;
    public GameObject PauseMenu;
    private PauseMenu pauseMenuScript;

    public Color CameraBoxColliderColor = new Color(0, 0, 1);
    public bool CameraCollidersVisible = true;

    public Color CutsceneTriggersColor = new Color(0, 0, 1);
    public bool CutsceneTriggersVisible = true;

    //public bool JoystickIsUsed = false;

    public Vector2 MainCameraOrthoSize;
    public float aspectRatio;

    public List<IPausable> pausables;
    public bool disableCutscenes = false;
    public bool disableDialogs = false;
    public bool constantlyDisableCutscenes = false;

    public bool FreezeKeiselOnLoad = false;

    private void Awake () {
        if (Application.isPlaying) {
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

            if (PauseMenu != null) {
                pauseMenuScript = PauseMenu.GetComponent<PauseMenu>();
                pauseMenuScript.OnPaused += OnPaused;
            }

            aspectRatio = (float)Screen.width / (float)Screen.height;

            MainCameraOrthoSize = new Vector2(MainCamera.GetComponent<Camera>().orthographicSize * aspectRatio, MainCamera.GetComponent<Camera>().orthographicSize);

            pausables = new List<IPausable>();
            if (FreezeKeiselOnLoad) {
                Keisel.GetComponent<KieselControl>().OnPause();
            }
            var ps = FindObjectsOfType<MonoBehaviour>().OfType<IPausable>();
            foreach (IPausable p in ps) {
                pausables.Add(p);
            }
        }

    }

    // Use this for initialization
    void Start () {

        if (overrideStartPosition) {
            MainCamera.transform.position = new Vector3(Keisel.transform.position.x, Keisel.transform.position.y, MainCamera.transform.position.z);
        } else {
            MainCamera.transform.position = new Vector3(keiselStartPosition.x, keiselStartPosition.y, MainCamera.transform.position.z);
            Keisel.transform.position = keiselStartPosition;
            
        }

        //JoystickIsUsed = Globals.Instance.joystickUsed;
	}

    float dt = 0;

    public void Update () {
        if (!Application.isPlaying) {
            if (constantlyDisableCutscenes) {
                var cs = FindObjectsOfType<MonoBehaviour>().OfType<CutsceneTrigger>();
                foreach (CutsceneTrigger cc in cs) {
                    cc.enabled = !disableCutscenes;
                }

                var ds = FindObjectsOfType<MonoBehaviour>().OfType<DialogTrigger>();
                foreach (DialogTrigger cc in ds) {
                    cc.enabled = !disableDialogs;
                }
            }
            cameraColliderColor = CameraBoxColliderColor;
            showCameraColliders = CameraCollidersVisible;
            showCutsceneTriggers = CutsceneTriggersVisible;
            cutsceneTriggerColor = CutsceneTriggersColor;
        } else {
            MainCameraOrthoSize = new Vector2(MainCamera.GetComponent<Camera>().orthographicSize * aspectRatio, MainCamera.GetComponent<Camera>().orthographicSize);
        }
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
