using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Vector2 minPosition = new Vector2(-10, -10);
    public Vector2 maxPosition = new Vector2(10, 10);
    public GameObject followTarget;
    public bool drawBoundsGizmo = true;
    [Tooltip("Controls how the camera settles on the target object when it stops moving.")]
    public AnimationCurve followCurve;
    [Range(0.5f, 4), Tooltip("Controls how far from the center of the camera the target can get.")]
    public float multiplier = 1;
    // Use this for initialization
    void Start () {
        if (followTarget == null) {
            followTarget = GameObject.FindGameObjectWithTag("Player");
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (followTarget != null) {
            
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
            diff.x = Mathf.Max(Mathf.Min(diff.x, maxPosition.x), minPosition.x);
            diff.y = Mathf.Max(Mathf.Min(diff.y, maxPosition.y), minPosition.y);

            transform.position = diff;//Finally we can set the position of the camera!
        }
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
}
