﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyDraggable : MonoBehaviour {
    [Tooltip("The script automatically looks for an object tagged as main camera. Only set this if you need to use a custom camera.")]
    public Camera mainCamera;
    [Tooltip("True when the object is being dragged.")]
    public bool dragging = false;
    [Tooltip("How much force to apply to the object when being dragged.")]
    public float multiplier = 0.25f;
    public float maxAltitude = 10f;
    [Tooltip("Sets how the object will behave as it reaches its maximum altitude.")]
    public AnimationCurve altitudeApproachCurve;

    private Rigidbody2D rb;

    public GameObject highlightSprite;

    public SmoothTransition highlight;
    public float highlightTime = 1;

    public bool mouseOver = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        if (highlightSprite == null) {
            highlightSprite = transform.GetChild(0).gameObject;
        }

        highlight = new SmoothTransition(0, 1, null, highlightTime);
    }
	
	// Update is called once per frame
	void Update () {
		if (dragging) {
            //Get the position of the mouse in world space
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            //Create a normalized vector (we aren't worried about the distance from the mouse cursor, only the direction) pointing in the direction of the cursor
            Vector2 directionVector = (mouseWorldPos - (Vector2)transform.position).normalized;
            //Apply it as a force and multiply it to get a visible result
            rb.AddForce(directionVector * multiplier * altitudeApproachCurve.Evaluate(maxAltitude - transform.position.y) * Time.deltaTime);
        }

        if (highlight.running) {
            highlightSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, highlight.DriveForward());
        }
	}

    private void OnMouseOver() {
        mouseOver = true;
        //Create the halo object
        if (highlightSprite != null) {
            highlight.start = highlight.outNumber;
            highlight.end = 1;
            highlight.Begin();
        }
    }

    private void OnMouseExit() {
        mouseOver = false;

        if (highlightSprite != null && !dragging) {
            highlight.start = highlight.outNumber;
            highlight.end = 0;
            highlight.Begin();
        }
    }

    private void OnMouseDown() {
        dragging = true;

    }

    private void OnMouseUp() {
        dragging = false;
        if (!mouseOver) {
            if (highlightSprite != null) {
                highlight.start = highlight.outNumber;
                highlight.end = 0;
                highlight.Begin();
            }
        }
    }
}
