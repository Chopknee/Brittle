using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHighlight : MonoBehaviour {

    public GameObject highlightSprite;

    public SmoothTransition highlight;
    public float highlightTime = 1;

    public bool mouseOver = false;

    // Use this for initialization
    void Start () {

        if (highlightSprite == null) {
            highlightSprite = transform.GetChild(0).gameObject;
        }

        highlight = new SmoothTransition(0, 1, null, highlightTime);

    }
	
	// Update is called once per frame
	void Update () {

        if (highlight.running) {
            highlightSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, highlight.DriveForward());
        }

    }

    private void OnMouseOver () {
        highlight.start = 0;
        highlight.end = 1;
        highlight.Begin();
    }

    private void OnMouseExit () {
        highlight.start = 1;
        highlight.end = 0;
        highlight.Begin();
    }
}
