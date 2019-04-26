using UnityEngine;

public class Fireflies: MonoBehaviour {
    public bool convertToWorldSpace = false;
    public static Fireflies Instance {
        get {
            return instance;
        }
    }
    private static Fireflies instance;

    private Camera mainCamera;
    private ParticleSystem particles;
    [Range(0f, 1f)]
    public float tiredness = 0;

    public Color energeticColor;
    public Color tiredColor;

    private Vector2 local = new Vector2();

    public float recoveryAmount = 0.05f;

    private void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        mainCamera = LevelControl.Instance.MainCamera.GetComponent<Camera>();

        particles = GetComponent<ParticleSystem>();

        if (Globals.Instance.joystickUsed) {
            //Recenter the fireflies to the camera
            transform.position = new Vector3(LevelControl.Instance.MainCamera.transform.position.x, LevelControl.Instance.MainCamera.transform.position.y, transform.position.z);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!Globals.Instance.joystickUsed) {
            //Move based on mouse
            if (convertToWorldSpace) {
                //Basically, converts the screen coordinates to world coordinates. Puts the cursor at that point.
                transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            } else {
                //Uses screen space in stead. This is for a UI cursor rather than an in-game cursor.
                transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        } else {
            //Move based on joystick axes
            if (convertToWorldSpace) {
                //
                float horiz = Input.GetAxis("RHorizontal") / 10;
                float vertic = Input.GetAxis("RVertical") / 10;
                local = local + new Vector2(horiz, vertic);// + (Vector2)LevelControl.Instance.MainCamera.transform.position;
                transform.position = new Vector3(local.x, local.y, transform.position.z) + LevelControl.Instance.MainCamera.transform.position;
                //Keeping the camera within the bounds of the visible area
                transform.position = Vector2.Min(Vector2.Max(transform.position, (Vector2) LevelControl.Instance.MainCamera.transform.position - LevelControl.Instance.MainCameraOrthoSize),
                    (Vector2) LevelControl.Instance.MainCamera.transform.position + LevelControl.Instance.MainCameraOrthoSize);

                
                //Then we need to keep them within the camera bounds
            } else {
                //Move the fireflies faster in screen space coords
            }

        }
        ParticleSystem.EmissionModule em = particles.emission;
        em.rateOverTime = Mathf.Lerp(4, 90, tiredness);
        ParticleSystem.MainModule mn = particles.main;
        mn.startColor = Color.Lerp(tiredColor, energeticColor, tiredness);

        tiredness += recoveryAmount * Time.deltaTime;
        tiredness = Mathf.Min(tiredness, 1);
        tiredness = Mathf.Max(0, tiredness);

    }


}
