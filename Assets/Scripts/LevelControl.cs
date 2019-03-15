using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour {

    public Vector3 keiselStartPosition;
    public bool overrideStartPosition;
    public bool drawStartPosition;
    public GameObject player;
    public GameObject mainCamera;
    public CameraModifyTrigger initialCameraBounds;

	// Use this for initialization
	void Start () {
		
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        if (overrideStartPosition) {
            mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);
            if (initialCameraBounds != null) {
                mainCamera.GetComponent<CameraFollow>().SetBounds(initialCameraBounds.minPosition, initialCameraBounds.maxPosition, initialCameraBounds.boundsTransitionTime);
            }
        } else {
            player.transform.position = keiselStartPosition;
            mainCamera.transform.position = new Vector3(keiselStartPosition.x, keiselStartPosition.y, mainCamera.transform.position.z);
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
