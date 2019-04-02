using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxFade : MonoBehaviour {

    public SpriteRenderer[] sprites;

    public float fadeStart;//World coordinates
    public float fadeEnd;//World coordinates
    public bool fadeOut = false;
    private float distance;
	// Use this for initialization

	void Start () {
        float temp = fadeStart;
        fadeStart = Mathf.Min(fadeStart, fadeEnd);
        fadeEnd = Mathf.Max(temp, fadeEnd);
        //Fade Start will always have the smallest components
        //Fade end will always have the largest
        distance = fadeEnd - fadeStart;
        sprites = GetComponentsInChildren<SpriteRenderer>();

	}
    bool hasFaded = true;
	// Update is called once per frame
	void Update () {
        
		if (transform.position.x > fadeStart && transform.position.y < fadeEnd) {
            DoFade();
            hasFaded = true;
        } else if (hasFaded && transform.position.x > fadeEnd) {
            float t = (fadeOut)? 0 : 1;
            foreach (SpriteRenderer rend in sprites) {
                rend.color = new Color(1, 1, 1, t);
            }
            hasFaded = false;
        } else if (hasFaded && transform.position.x < fadeStart) {
            float t = ( fadeOut ) ? 1 : 0;
            foreach (SpriteRenderer rend in sprites) {
                rend.color = new Color(1, 1, 1, t);
            }
            hasFaded = false;
        }
	}

    public void DoFade() {
        float distThrough = ( fadeOut ) ? distance - ( transform.position.x - fadeStart ) : distance - ( fadeEnd - transform.position.x );
        //distThrough = distThrough / distance;
        float t = distThrough / distance;
        //Fade based on this information
        foreach (SpriteRenderer rend in sprites) {
            rend.color = new Color(1, 1, 1, t);
        }
    }

    private void OnDrawGizmosSelected () {
        Gizmos.color = new Color(255, 0, 255);
        float size = ( fadeEnd - fadeStart ) / 2;
        Gizmos.DrawWireCube(new Vector3(fadeStart + size, 0, 0), new Vector3(size*2, 1, 1));
        Gizmos.DrawSphere(new Vector3(fadeStart, 0, 0), 0.2f);
        Gizmos.DrawSphere(new Vector3(fadeEnd, 0, 0), 0.2f);
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    private bool VectorGreaterThan(Vector3 a, Vector3 b) {
        return a.x > b.x && a.y > b.y && a.z > b.z;
    }

    private bool VectorLessThan ( Vector3 a, Vector3 b ) {
        return a.x < b.x && a.y < b.y && a.z < b.z;
    }
}
