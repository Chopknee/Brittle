using UnityEngine;

public class CameraFollow : MonoBehaviour, IPausable {

    public Vector2 minPosition = new Vector2(-10, -10);
    public Vector2 maxPosition = new Vector2(10, 10);
    public GameObject followTarget;
    public bool drawBoundsGizmo = true;
    [Tooltip("Controls how the camera settles on the target object when it stops moving.")]
    public AnimationCurve followCurve;
    [Range(0.5f, 4), Tooltip("Controls how far from the center of the camera the target can get.")]
    public float multiplier = 1;

    private bool boundsTransitioning = false;
    private float boundsTransitionTime = 0;
    private Vector2 transitionMinPosition = new Vector2();
    private Vector2 transitionMaxPosition = new Vector2();
    private float boundsTransitionTimeCount = 0;

    
    private Camera cam;
    //private GameObject[] background;
    //private Vector3 oldBgScale;
    private Vector3[] oldBgScales;
    private bool zooming = false;
    SmoothTransition zoomTransition;

    private bool paused = false;
    
    // Use this for initialization
    void Start () {
        if (followTarget == null) {
            followTarget = GameObject.FindGameObjectWithTag("Player");
        }
        cam = GetComponent<Camera>();
        //background = transform.child.gameObject;//Always the first child?
        oldBgScales = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            oldBgScales[i] = transform.GetChild(i).localScale;
        }
        zoomTransition = new SmoothTransition(0, 0, null, 0);
        zoomTransition.OnFinish += OnZoomFinish;
    }
	
	// Update is called once per frame
	void Update () {
		if (followTarget != null || !paused) {
            
            //get the difference between the camera and player
            Vector3 diff = followTarget.transform.position - transform.position;

            //The z should be forced to 0 because we are on a 2d plane
            diff.z = 0;


            //Take the magnitude (distance from target) and scale it down. We only want the camera to move a portion of the distance from the target.
            //This number grows larger as the distance gets bigger, until the mul is equal to the distance moved by the target since the last frame.
            //Time.deltaTime ensures that this is the same on any framerate
            float mul = followCurve.Evaluate(diff.magnitude) * multiplier * Time.deltaTime;

            //This is now applied to the position
            diff = transform.position + (diff * mul);

            //Make sure it is within the current bounds.
            if (!boundsTransitioning) {
                diff = Vector2.Max(minPosition, Vector2.Min(maxPosition, diff));
            } else {
                Vector2 tmpMin = Vector2.Lerp(minPosition, transitionMinPosition, boundsTransitionTimeCount/boundsTransitionTime);
                Vector2 tmpMax = Vector2.Lerp(maxPosition, transitionMaxPosition, boundsTransitionTimeCount/boundsTransitionTime);
                diff = Vector2.Max(tmpMin, Vector2.Min(tmpMax, diff));
                //Bring the two bounds closer
                boundsTransitionTimeCount += Time.deltaTime;
                if (boundsTransitionTimeCount >= boundsTransitionTime) {
                    boundsTransitioning = false;
                    minPosition = transitionMinPosition;
                    maxPosition = transitionMaxPosition;
                    boundsTransitionTimeCount = 0;
                }
            }
            transform.position = new Vector3(diff.x, diff.y, transform.position.z);//Finally we can set the position of the camera!

            if (zooming) {
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
        }
	}

    public void SetBounds(Vector2 min, Vector2 max, float boundsTransitionTime) {
        transitionMinPosition = min;
        transitionMaxPosition = max;
        boundsTransitioning = true;
        this.boundsTransitionTime = boundsTransitionTime;
    }

    public void SetZoom(float newSize, AnimationCurve transitionCurve, float transitionTime) {
        zooming = true;
        zoomTransition.Begin(cam.orthographicSize, newSize, transitionCurve, transitionTime);
        for (int i = 0; i < transform.childCount; i++) {
            oldBgScales[i] = transform.GetChild(i).localScale;
        }
    }

    public void OnZoomFinish() {
        zooming = false;
    }

    private void OnDrawGizmos() {
        //Visualizes the boundaries of where the camera is able to travel.
        if (drawBoundsGizmo) {
            Gizmos.color = new Color(0, 0, 255);
            //Upper line
            Gizmos.DrawLine(new Vector2(minPosition.x, minPosition.y), new Vector2(maxPosition.x, minPosition.y));
            Gizmos.DrawLine(new Vector2(minPosition.x, maxPosition.y), new Vector2(maxPosition.x, maxPosition.y));

            Gizmos.DrawLine(new Vector2(minPosition.x, minPosition.y), new Vector2(minPosition.x, maxPosition.y));
            Gizmos.DrawLine(new Vector2(maxPosition.x, minPosition.y), new Vector2(maxPosition.x, maxPosition.y));
        }
    }

    public void OnPause() {
        //throw new System.NotImplementedException();
        paused = true;
    }

    public void OnUnPause() {
        //throw new System.NotImplementedException();
        paused = false;
    }
}
