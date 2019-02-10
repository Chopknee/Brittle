using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour {
    public Camera mainCamera;
    public bool convertToWorldSpace = false;
	// Use this for initialization
	void Start () {
        if (mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }		
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
	}
}
