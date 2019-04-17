using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyDraggable : MonoBehaviour, IPausable {
    [Tooltip("The script automatically looks for an object tagged as main camera. Only set this if you need to use a custom camera.")]
    public Camera mainCamera;
    [Tooltip("True when the object is being dragged.")]
    public bool dragging = false;
    [Tooltip("How much force to apply to the object when being dragged.")]
    public float multiplier = 0.25f;
    [Tooltip("Sets how the object will behave as it reaches its maximum altitude.")]
    private Rigidbody2D rb;

    public GameObject highlightSprite;

    public SmoothTransition highlight;
    public float highlightTime = 1;

    public bool mouseOver = false;

    public float highlightRadius = 10f;
    public float highlightRadiusSquared;
    public LayerMask playerMask;

    public bool lastInside = false;
    public bool DrawRadiusGizmo = false;

    public float energyRequirement = 0.1f;

    public GameObject player;

    public MouseSound tinkleSound;

    public bool paused;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null) {
            mainCamera = LevelControl.Instance.MainCamera.GetComponent<Camera>();
        }

        if (highlightSprite == null) {
            highlightSprite = transform.GetChild(0).gameObject;
        }

        highlight = new SmoothTransition(0, 1, null, highlightTime);

        if (player == null) {
            player = LevelControl.Instance.Keisel;
        }

        tinkleSound = GetComponent<MouseSound>();
        highlightRadiusSquared = highlightRadius * highlightRadius;
    }
	
	
	void Update () {
        if (!paused) {
            if (mouseOver) {
                if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Interact")) {
                    dragging = true;
                    GetComponent<ParticleSystem>().Play();
                }
            }
            if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Interact")) {
                dragging = false;
                GetComponent<ParticleSystem>().Stop();
            }

            if (dragging) {
                //Get the position of the mouse in world space
                Vector2 mouseWorldPos = new Vector2(LevelControl.Instance.Fireflies.transform.position.x, LevelControl.Instance.Fireflies.transform.position.y);//mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                //The distance between cursor and object
                Vector2 directionVector = (mouseWorldPos - (Vector2)transform.position);
                rb.velocity = multiplier * new Vector3(directionVector.x, directionVector.y, 0);
                

                Fireflies.Instance.tiredness -= energyRequirement * Time.deltaTime;

                if (Fireflies.Instance.tiredness <= 0) {
                    dragging = false;
                    GetComponent<ParticleSystem>().Stop();
                    tinkleSound.ForceStop();
                }
            }

            if (highlight.running) {
                highlightSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, highlight.DriveForward());
            }

            Vector2 dst = (transform.position - LevelControl.Instance.Fireflies.transform.position);
            bool currentInside = (dst).sqrMagnitude < highlightRadiusSquared;

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

    public void OnDrawGizmos() {
        if (DrawRadiusGizmo) {
            Gizmos.color = new Color(0, 255, 0);
            Gizmos.DrawWireSphere(transform.position, highlightRadius);
        }
    }

    public void OnTriggerEnter2D ( Collider2D collision ) {
        
        if (collision.gameObject.tag == "Fireflies") {
            mouseOver = true;
        }
    }

    public void OnTriggerExit2D ( Collider2D collision ) {
        if (collision.gameObject.tag == "Fireflies") {
            mouseOver = false;
        }
    }

    Vector3 oldVel = new Vector3();

    public void OnPause() {
        //throw new System.NotImplementedException();
        paused = true;
        oldVel = rb.velocity;
        rb.isKinematic = true;
    }

    public void OnUnPause() {
        //throw new System.NotImplementedException();
        paused = false;
        rb.isKinematic = false;
        rb.velocity = oldVel;
    }
}
