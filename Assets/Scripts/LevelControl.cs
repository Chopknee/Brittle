using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour {

    public Vector3 keiselStartPosition;
    public bool overrideStartPosition;
    public bool drawStartPosition;
    public CameraModifyTrigger initialCameraBounds;
    //Accessible to all objects.
    public static GameObject Keisel;
    public static GameObject MainCamera;

    private void Awake () {
        if (Keisel == null) {
            Keisel = GameObject.FindGameObjectWithTag("Player");
        }

        if (MainCamera == null) {
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    // Use this for initialization
    void Start () {

        if (overrideStartPosition) {
            
            if (initialCameraBounds != null) {
                MainCamera.GetComponent<CameraFollow>().SetBounds(initialCameraBounds.minPosition, initialCameraBounds.maxPosition, initialCameraBounds.boundsTransitionTime);
                MainCamera.GetComponent<CameraFollow>().SetZoom(initialCameraBounds.cameraSize, initialCameraBounds.sizeTransitionCurve, initialCameraBounds.zoomTransitionTime);
            }
            //MainCamera.transform.position = new Vector3(Keisel.transform.position.x, Keisel.transform.position.y, MainCamera.transform.position.z);
        } else {
            MainCamera.transform.position = new Vector3(keiselStartPosition.x, keiselStartPosition.y, MainCamera.transform.position.z);
            Keisel.transform.position = keiselStartPosition;
            
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
}
