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
    public bool jumping = false;
    public bool falling = false;
    public bool running = false;
    public float moveX;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask[] whatIsGround;
    public bool grounded;
    private Rigidbody2D rb;
    private Collider2D coll;
    public bool drawGroundCheckGizmo;
    Animator kam;
    public string currentGroundType;

    private bool jumped = false;

    public PhysicsMaterial2D ground_physicsMaterial;
    public PhysicsMaterial2D airborne_physicsMaterial;

    public bool controlsFrozen = false;
    public bool freezeMovement = false;

    public void Start() {
        
        rb = GetComponent<Rigidbody2D>();
        kam = GetComponentInChildren<Animator>();
        coll = GetComponent<Collider2D>();
    }

    
    public void Update() {
        if (!freezeMovement) {
            //Controling the direction the sprite faces
            running = false;
            if (!controlsFrozen) {
                moveX = Input.GetAxis(HorizontalAxis);
                if (moveX < 0.0 && facingRight == true) {
                    facingRight = false;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                } else if (moveX > 0.0f && facingRight == false) {
                    facingRight = true;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }
            }


            //Setting the velocity of the character
            if (grounded) {
                if (moveX != 0) {
                    rb.AddForce(new Vector2(moveX / Mathf.Abs(moveX) * horizontalForce, 0));
                    running = true;
                }
                if (Input.GetButtonDown(JumpButton) && !jumped) {
                    jumped = true;
                    if (kam.gameObject.activeSelf) {
                        kam.SetTrigger("Jump");
                    }
                    //Apply the jump velocity.
                    rb.AddForce(new Vector2(moveX, 2) * verticalForce);
                }
            }

            rb.velocity = Vector2.Min(Vector2.Max(rb.velocity, -maxVelocity), maxVelocity);

            falling = ( rb.velocity.y < 0 && !grounded ) ? true : false;
            jumping = ( rb.velocity.y > 0 && !grounded ) ? true : false;
            if (kam.gameObject.activeSelf) {
                kam.SetFloat("HorizontalSpeed", Mathf.Abs(rb.velocity.x));
                kam.SetFloat("VerticalSpeed", rb.velocity.y);
                kam.SetBool("Jumping", jumping);
                kam.SetBool("Falling", falling);
                kam.SetBool("IsRunning", running);
            }
        }
    }

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

        if (kam.gameObject.activeSelf) {
            kam.SetBool("Grounded", grounded);
        }

        if (!grounded && jumped) {
            jumped = false;
        }
    }

    public void OnDrawGizmos() {
        if (drawGroundCheckGizmo) {
            Gizmos.color = new Color(0, 0, 255);
            Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        }
    }

    public void SetAnimationMode() {

    }

    public void EndAnimationMode() {

    }
}
