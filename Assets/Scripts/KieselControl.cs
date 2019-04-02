using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class KieselControl : MonoBehaviour, IPausable {
    public string HorizontalAxis = "Horizontal";
    public string VerticalAxis = "Vertical";
    public string JumpButton = "Jump";
    public float horizontalForce = 10;
    public float airHorizontalForce = 5;
    public float verticalForce = 100;
    public Vector2 maxVelocity = new Vector2(5, 10);
    public bool facingRight = true;
    public bool jumping = false;
    public bool falling = false;
    public bool running = false;
    public bool climbing = false;
    public float moveX;
    public float moveY;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask[] whatIsGround;
    public bool grounded;
    private Rigidbody2D rb;
    private Collider2D coll;
    public bool drawGroundCheckGizmo;
    //Animator kam;
    Keisel_AnimationController kam;
    public string currentGroundType;

    private bool jumped = false;

    public PhysicsMaterial2D ground_physicsMaterial;
    public PhysicsMaterial2D airborne_physicsMaterial;

    public bool controlsFrozen = false;
    public bool freezeMovement = false;

    public void Start() {
        
        rb = GetComponent<Rigidbody2D>();
        //kam = GetComponentInChildren<Animator>();
        kam = GetComponent<Keisel_AnimationController>();
        coll = GetComponent<Collider2D>();
    }

    
    public void Update() {

        moveX = 0;
        moveY = 0;
        running = false;

        if (!freezeMovement) {

            if (!controlsFrozen) {

                //Horizontal or vertical movement
                moveX = Input.GetAxis(HorizontalAxis);
                moveY = Input.GetAxis(VerticalAxis);
                if (moveX < 0.0 && facingRight == true) {
                    facingRight = false;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                } else if (moveX > 0.0f && facingRight == false) {
                    facingRight = true;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }
            
            
                if (grounded) {//Jumping
                    if (Input.GetButtonDown(JumpButton) && !jumped) {
                        jumped = true;
                        if (kam.gameObject.activeSelf) {
                            //kam.SetTrigger("Jump");
                            kam.Jump();
                        }
                        if (climbing) {
                            StopClimbing();
                        }
                        //Apply the jump velocity.
                        rb.AddForce(new Vector2(moveX, 2) * verticalForce);
                    }
                }
            }

            //Horizontal jump launch
            if (moveX != 0 && !climbing) {
                float horizontalMultiplier = horizontalForce;
                if (jumped || !grounded) {
                    horizontalMultiplier = airHorizontalForce;
                }
                rb.AddForce(new Vector2(moveX / Mathf.Abs(moveX) * horizontalMultiplier, 0));
                running = true;
            }

            if (moveY != 0 && climbing) {

            }

            //Maximum speed control
            rb.velocity = Vector2.Min(Vector2.Max(rb.velocity, -maxVelocity), maxVelocity);

            //Animation controller
            falling = ( rb.velocity.y < 0 && !grounded ) ? true : false;
            jumping = ( rb.velocity.y > 0 && !grounded ) ? true : false;
            if (kam.gameObject.activeSelf) {
                kam.horizontalSpeed = Mathf.Abs(rb.velocity.x);
                kam.verticalSpeed = rb.velocity.y;
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
        if (climbing) {
            grounded = true;
        }

        if (kam.gameObject.activeSelf) {
            //kam.SetBool("Grounded", grounded);
            kam.grounded = grounded;
        }

        if (grounded && jumped) {
            jumped = false;
        }
    }

    public void OnDrawGizmos() {
        if (drawGroundCheckGizmo) {
            Gizmos.color = new Color(0, 0, 255);
            Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        }
    }

    Vector3 oldVel = new Vector3();

    public void OnPause() {
        //throw new System.NotImplementedException();
        controlsFrozen = true;
        freezeMovement = true;
        oldVel = rb.velocity;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    public void OnUnPause() {
        //throw new System.NotImplementedException();
        controlsFrozen = false;
        freezeMovement = false;
        rb.isKinematic = false;
        rb.velocity = oldVel;
    }

    public void OnTriggerEnter2D ( Collider2D collision ) {
        //
        if (collision.tag == "WallVines") {
            BeginClimbing();
        }
    }

    public void OnTriggerExit2D ( Collider2D collision ) {
        if (collision.tag == "WallVines") {
            //Eh?
        }
    }

    private float grav = 0;
    public void BeginClimbing() {
        climbing = true;
        grav = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
    }

    public void StopClimbing() {
        rb.gravityScale = grav;
        climbing = false;
    }
}
