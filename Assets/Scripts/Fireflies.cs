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

    public float recoveryAmount = 0.05f;

    private void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        mainCamera = LevelControl.MainCamera.GetComponent<Camera>();

        particles = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (convertToWorldSpace) {
            //Basically, converts the screen coordinates to world coordinates. Puts the cursor at that point.
            transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
        } else {
            //Uses screen space in stead. This is for a UI cursor rather than an in-game cursor.
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
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
