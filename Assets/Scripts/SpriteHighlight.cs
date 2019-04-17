using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHighlight : MonoBehaviour {

    public GameObject highlightSprite;

    public SmoothTransition highlight;
    public float highlightTime = 1;

    public bool mouseOver = false;

    public float highlightRadius = 10f;
    private float highlightRadiusSquared;

    private GameObject player;

    private bool lastInside = false;

    // Use this for initialization
    void Start () {

        if (highlightSprite == null) {
            highlightSprite = transform.GetChild(0).gameObject;
        }

        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        highlight = new SmoothTransition(0, 1, null, highlightTime);
        highlightRadiusSquared = highlightRadius * highlightRadius;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (highlight.running) {
            highlightSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, highlight.DriveForward());
        }

        Vector2 dst = ( transform.position - LevelControl.Instance.Fireflies.transform.position );
        bool currentInside = ( dst ).sqrMagnitude < highlightRadiusSquared;
        if (currentInside != lastInside) {
            lastInside = currentInside;
            if (currentInside) {
                if (highlightSprite != null) {
                    highlight.start = highlight.outNumber;
                    highlight.end = 1;
                    highlight.Begin();
                }
            } else {
                if (highlightSprite != null) {
                    highlight.start = highlight.outNumber;
                    highlight.end = 0;
                    highlight.Begin();
                }
            }

        }

    }
}
