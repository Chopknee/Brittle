using UnityEngine;

public class CameraFollow : MonoBehaviour, IPausable {

    //public Vector2 minPosition = new Vector2(-10, -10);
    //public Vector2 maxPosition = new Vector2(10, 10);
    public GameObject followTarget;
    public bool drawBoundsGizmo = true;
    [Tooltip("Controls how the camera settles on the target object when it stops moving.")]
    public AnimationCurve followCurve;
    [Range(1, 700), Tooltip("Controls how far from the center of the camera the target can get.")]
    public float multiplier = 1;

    private float boundsTransitionTimeCount = 0;

    
    private Camera cam;
    private Vector3[] oldBgScales;
    SmoothTransition zoomTransition;

    SmoothTransition collisionTransition;

    Rigidbody2D rb;
    CapsuleCollider2D cc;

    private bool paused = false;
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        if (followTarget == null) {
            followTarget = LevelControl.Instance.Keisel;
        }

        cam = GetComponent<Camera>();

        cc.size = LevelControl.Instance.MainCameraOrthoSize;

        oldBgScales = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            oldBgScales[i] = transform.GetChild(i).localScale;
        }

        zoomTransition = new SmoothTransition(0, 0, null, 0);
        zoomTransition.OnFinish += OnZoomFinish;
        collisionTransition = new SmoothTransition(0, 0, null, 0);
        collisionTransition.OnFinish += OnCollisionSizeChangeFinish;
    }

    void FixedUpdate() {
        if (followTarget != null && !paused) {

            //get the difference between the camera and player
            Vector3 diff = followTarget.transform.position - transform.position;

            //The z should be forced to 0 because we are on a 2d plane
            diff.z = 0;

            //Take the magnitude (distance from target) and scale it down. We only want the camera to move a portion of the distance from the target.
            //This number grows larger as the distance gets bigger, until the mul is equal to the distance moved by the target since the last frame.
            //Time.deltaTime ensures that this is the same on any framerate
            float mul = followCurve.Evaluate(diff.magnitude) * multiplier * Time.deltaTime;

            //This is now applied to the position
            diff = diff * mul;
            rb.velocity = diff;
            //transform.position = new Vector3(diff.x, diff.y, transform.position.z);//Finally we can set the position of the camera!
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (followTarget != null && !paused) {
            if (zoomTransition.running) {
                float cz = zoomTransition.DriveForward();
                cam.orthographicSize = cz;
                for (int i = 0; i < transform.childCount; i++) {
                    GameObject background = transform.GetChild(i).gameObject;
                    background.transform.localScale = new Vector3(
                        ( oldBgScales[i].x * cz ) / zoomTransition.start,
                        ( oldBgScales[i].y * cz ) / zoomTransition.start,
                        ( oldBgScales[i].z * cz ) / zoomTransition.start
                        );
                }
            }

            if (collisionTransition.running) {
                float size = collisionTransition.DriveForward();
                //Figure out what size to set the two values to
                cc.size = new Vector2(size * LevelControl.Instance.aspectRatio, size);

            }
        }
	}

    public void SetZoom(float newSize, AnimationCurve transitionCurve, float transitionTime) {
        zoomTransition.Begin(cam.orthographicSize, newSize, transitionCurve, transitionTime);
        for (int i = 0; i < transform.childCount; i++) {
            oldBgScales[i] = transform.GetChild(i).localScale;
        }
    }

    public void SetColliderSize(float newSize, AnimationCurve transitionCurve, float transitionTime) {
        collisionTransition.Begin(cc.size.y, newSize, transitionCurve, transitionTime);
    }

    public void OnZoomFinish() {
        //zooming = false;
    }

    public void OnCollisionSizeChangeFinish() {
        //
    }

    //private void OnDrawGizmos() {
    //    //Visualizes the boundaries of where the camera is able to travel.
    //    if (drawBoundsGizmo) {
    //        Gizmos.color = new Color(0, 0, 255);
    //    }
    //}

    public void OnPause() {
        paused = true;
    }

    public void OnUnPause() {
        paused = false;
    }
}
