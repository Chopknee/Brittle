using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Vector2 minPosition = new Vector2(-10, -10);
    public Vector2 maxPosition = new Vector2(10, 10);
    public GameObject followTarget;
    [Range(0.01f, 1)]
    public float followSpeed = 0.95f;

	// Use this for initialization
	void Start () {
        if (followTarget == null) {
            followTarget = GameObject.FindGameObjectWithTag("Player");
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (followTarget != null) {
            //Follow the target, keeping the camera within the defined bounds

            //get distance between camera and target
            Vector3 diff = followTarget.transform.position - transform.position;

            //set the diff y to 0 because we don't want to change the current height
            diff.z = 0;
            //tween that difference
            diff = diff * followSpeed;//Move 10 % of the distance toward the target
            diff = diff * Time.deltaTime;//Make it time dependent
            diff = transform.position + diff;//Predict the new postion
            diff.x = Mathf.Max(Mathf.Min(diff.x, maxPosition.x), minPosition.x);
            diff.y = Mathf.Max(Mathf.Min(diff.y, maxPosition.y), minPosition.y);
            transform.position = diff;//Finally set the position of the camera!
        }
	}
}
