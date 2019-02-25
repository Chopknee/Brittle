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

    public Camera cam;

    public void Start() {
        if (cf == null) {
            cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        }

        if (cam == null) {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!triggered) {
            if (disabledOnTrigger) {
                triggered = true;
            }
            cf.minPosition = minPosition+(Vector2)transform.position;
            cf.maxPosition = maxPosition+(Vector2)transform.position;
        }
    }

    private void OnDrawGizmos() {
        //Visualizes the boundaries of where the camera is able to travel.
        if (drawBoundsGizmo) {
            if (cam == null) {
                cam = GetComponent<Camera>();
            }
            float asp = (float)Screen.width / Screen.height;
            float orthoWidth = cam.orthographicSize * asp;
            float orthoHeight = cam.orthographicSize;

            Gizmos.color = new Color(0, 0, 255);
            //Upper line
            Vector3 cornerA = new Vector3(minPosition.x + transform.position.x, maxPosition.y + transform.position.y, transform.position.z);
            Vector3 cornerB = new Vector3(minPosition.x + transform.position.x, minPosition.y + transform.position.y, transform.position.z);
            Vector3 cornerC = new Vector3(maxPosition.x + transform.position.x, minPosition.y + transform.position.y, transform.position.z);
            Vector3 cornerD = new Vector3(maxPosition.x + transform.position.x, maxPosition.y + transform.position.y, transform.position.z);
            DrawGizmoBox(cornerA, cornerB, cornerC, cornerD);
            Gizmos.color = new Color(255, 0, 0);
            cornerA = new Vector3(minPosition.x - orthoWidth + transform.position.x, maxPosition.y + orthoHeight + transform.position.y, transform.position.z);
            cornerB = new Vector3(minPosition.x - orthoWidth + transform.position.x, minPosition.y - orthoHeight + transform.position.y, transform.position.z);
            cornerC = new Vector3(maxPosition.x + orthoWidth + transform.position.x, minPosition.y - orthoHeight + transform.position.y, transform.position.z);
            cornerD = new Vector3(maxPosition.x + orthoWidth + transform.position.x, maxPosition.y + orthoHeight + transform.position.y, transform.position.z);
            DrawGizmoBox(cornerA, cornerB, cornerC, cornerD);
        }
    }

    public void DrawGizmoBox(Vector3 cornerA, Vector3 cornerB, Vector3 cornerC, Vector3 cornerD) {
        Gizmos.DrawLine(cornerA, cornerB);
        Gizmos.DrawLine(cornerB, cornerC);
        Gizmos.DrawLine(cornerC, cornerD);
        Gizmos.DrawLine(cornerD, cornerA);
    }
}
