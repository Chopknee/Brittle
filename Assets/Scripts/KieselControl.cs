using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class KieselControl : MonoBehaviour {
    public string HorizontalAxis = "Horizontal";
    public string JumpButton = "Jump";
    public float horizontalForce = 10;
    public float verticalForce = 100;
    public Vector2 maxVelocity = new Vector2(5, 10);
    public bool facingRight = true;
    public float moveX;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask[] whatIsGround;
    public bool grounded;
    private Rigidbody2D rb;
    private Collider2D coll;
    public bool drawGroundCheckGizmo;
    Keisel_AnimationController kam;
    public string currentGroundType;

    private bool jumped = false;

    public PhysicsMaterial2D ground_physicsMaterial;
    public PhysicsMaterial2D airborne_physicsMaterial;

    public void Start() {
        
        rb = GetComponent<Rigidbody2D>();
        kam = GetComponent<Keisel_AnimationController>();
        coll = GetComponent<Collider2D>();
    }

    
    public void Update() {
        //Controling the direction the sprite faces
        moveX = Input.GetAxis(HorizontalAxis);
        if (moveX < 0.0 && facingRight == true) {
            facingRight = false;
            kam.FlipDirection((int)(moveX / Mathf.Abs(moveX)));
        } else if (moveX > 0.0f && facingRight == false) {
            facingRight = true;
            kam.FlipDirection((int)(moveX / Mathf.Abs(moveX)));
        }


        //Setting the velocity of the character
        if (grounded) {
            if (moveX != 0) {
                rb.AddForce(new Vector2(moveX / Mathf.Abs(moveX) * horizontalForce, 0));
            }
            if (Input.GetButtonDown(JumpButton) && !jumped) {
                jumped = true;
                kam.Jump();
                //Apply the jump velocity.
                rb.AddForce(new Vector2(moveX, 2) * verticalForce);
            }
        }

        rb.velocity = Vector2.Min(Vector2.Max(rb.velocity, -maxVelocity), maxVelocity);

        kam.horizontalSpeed = Mathf.Abs(rb.velocity.x);
        kam.verticalSpeed = rb.velocity.y;
    }

    private bool lastJumped = false;

    public void FixedUpdate() {
        //Checking if the character is on the ground
        foreach (LayerMask groundMask in whatIsGround) {
            Collider2D result;
            result = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
            if (result != null) {
                currentGroundType = result.gameObject.tag;
                grounded = true;
                coll.sharedMaterial = ground_physicsMaterial;
            } else {
                currentGroundType = "";
                grounded = false;
                coll.sharedMaterial = airborne_physicsMaterial;
            }
            if (grounded) { break; }
        }
        kam.grounded = grounded;
        if (!grounded && jumped && lastJumped != jumped) {
            jumped = false;
        }
    }

    public void OnDrawGizmos() {
        if (drawGroundCheckGizmo) {
            Gizmos.color = new Color(0, 0, 255);
            Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        }
    }
}
