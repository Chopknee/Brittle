using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundsChangeTrigger : MonoBehaviour {

    public Vector2 minPosition = new Vector2(-10, -10);
    public Vector2 maxPosition = new Vector2(10, 10);

    public bool drawBoundsGizmo = true;


    public CameraFollow cf;
    public bool triggered = false;

    public bool disabledOnTrigger = true;

    public void Start() {
        if (cf == null) {
            cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!triggered) {
            if (disabledOnTrigger) {
                triggered = true;
            }
            cf.minPosition = minPosition;
            cf.maxPosition = maxPosition;
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
