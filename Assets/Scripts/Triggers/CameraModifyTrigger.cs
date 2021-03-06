﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModifyTrigger : MonoBehaviour {
    public GameObject mainCamera;


    public bool changeZoom = false;
    public float cameraSize = 0;
    public AnimationCurve sizeTransitionCurve;
    public float zoomTransitionTime = 0;
    

    public bool changeColliderSize = false;
    public float colliderSize = 0;
    public AnimationCurve collSizeCurve;
    public float colliderTransitionTime = 0;

    public bool disableAfterTrigger = true;

    private CameraFollow cf;
    private Camera cam;
    private bool triggered = false;
    public void Start() {
        if (mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        cf = mainCamera.GetComponent<CameraFollow>();
        cam = mainCamera.GetComponent<Camera>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (!triggered) {
                if (disableAfterTrigger) {
                    triggered = true;
                }
                if (changeColliderSize) {
                    cf.SetColliderSize(colliderSize, collSizeCurve, colliderTransitionTime);
                }
                if (changeZoom) {
                    cf.SetZoom(cameraSize, sizeTransitionCurve, zoomTransitionTime);
                }
            }
        }
    }

    //private void OnDrawGizmos() {
    //    //Visualizes the boundaries of where the camera is able to travel.
    //    //if (drawBoundsGizmo) {
    //    //    if (cam == null) {
    //    //        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    //    //    }
    //    //    float asp = (float)Screen.width / Screen.height;
    //    //    float orthoWidth = cam.orthographicSize * asp;
    //    //    float orthoHeight = cam.orthographicSize;
    //    //    if (changeZoom) {
    //    //        orthoWidth = cameraSize * asp;
    //    //        orthoHeight = cameraSize;
    //    //    }

    //    //    Gizmos.color = new Color(0, 0, 255);
    //    //    //Upper line
    //    //    Vector3 cornerA = new Vector3(minPosition.x, maxPosition.y, transform.position.z);
    //    //    Vector3 cornerB = new Vector3(minPosition.x, minPosition.y, transform.position.z);
    //    //    Vector3 cornerC = new Vector3(maxPosition.x, minPosition.y, transform.position.z);
    //    //    Vector3 cornerD = new Vector3(maxPosition.x, maxPosition.y, transform.position.z);
    //    //    DrawGizmoBox(cornerA, cornerB, cornerC, cornerD);
    //    //    Gizmos.color = new Color(255, 0, 0);
    //    //    cornerA = new Vector3(minPosition.x - orthoWidth, maxPosition.y + orthoHeight, transform.position.z);
    //    //    cornerB = new Vector3(minPosition.x - orthoWidth, minPosition.y - orthoHeight, transform.position.z);
    //    //    cornerC = new Vector3(maxPosition.x + orthoWidth, minPosition.y - orthoHeight, transform.position.z);
    //    //    cornerD = new Vector3(maxPosition.x + orthoWidth, maxPosition.y + orthoHeight, transform.position.z);
    //    //    DrawGizmoBox(cornerA, cornerB, cornerC, cornerD);
    //    }
    //}

    //public void DrawGizmoBox(Vector3 cornerA, Vector3 cornerB, Vector3 cornerC, Vector3 cornerD) {
    //    //Gizmos.DrawLine(cornerA, cornerB);
    //    //Gizmos.DrawLine(cornerB, cornerC);
    //    //Gizmos.DrawLine(cornerC, cornerD);
    //    //Gizmos.DrawLine(cornerD, cornerA);
    //}
}
