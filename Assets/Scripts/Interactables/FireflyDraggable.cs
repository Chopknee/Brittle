using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyDraggable : MonoBehaviour, IPausable, IInteractable {
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
    public LayerMask playerMask;
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
    }
	
	
	void Update () {
        if (!paused) {
            if (highlight.running) {
                highlightSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, highlight.DriveForward());
            }
        }
	}

    Vector3 oldVel = new Vector3();

    public void OnPause() {
        paused = true;
        oldVel = rb.velocity;
        rb.isKinematic = true;
    }

    public void OnUnPause() {
        paused = false;
        rb.isKinematic = false;
        rb.velocity = oldVel;
    }

    public void Interact () {
        //isInteracted = true;
    }

    public GameObject GetGameObject () { 
        return gameObject;
    }

    public void Highlight () {
        highlight.start = highlight.outNumber;
        highlight.end = 1;
        highlight.Begin();
    }

    public void Dim () {
        highlight.start = highlight.outNumber;
        highlight.end = 0;
        highlight.Begin();
    }

    public void Select () {
        //This will be the selected show
        GetComponent<ParticleSystem>().Play();
    }

    public void DeSelect () {
        //throw new System.NotImplementedException();
        GetComponent<ParticleSystem>().Stop();
    }
}
