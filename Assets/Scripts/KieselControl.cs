using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class KieselControl : MonoBehaviour {
    public string HorizontalAxis = "Horizontal";
    public string JumpButton = "Jump";
    public float playerSpeed = 10;
    public bool facingRight = true;
    public float playerJH = 1250;
    public float jumpForceDelay = 0.2f;
    public float moveX;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask[] whatIsGround;
    public bool grounded;
    private Rigidbody2D rb;
    public bool drawGroundCheckGizmo;
    Keisel_AnimationController kam;
    public string currentGroundType;

    private bool jumped = false;

    public void Start() {
        
        rb = GetComponent<Rigidbody2D>();
        kam = GetComponent<Keisel_AnimationController>();

    }

    
    public void Update() {
        PlayerMove();
    }

    public void FixedUpdate() {
        //Checking if the character is on the ground
        foreach (LayerMask groundMask in whatIsGround) {
            Collider2D result;
            result = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
            if (result != null) {
                currentGroundType = result.gameObject.tag;
                grounded = true;
            } else {
                currentGroundType = "";
                grounded = false;
            }
            if (grounded) { break; }
        }
        kam.grounded = grounded;
    }

    private void PlayerMove() {
        
        if (Input.GetButtonDown(JumpButton) && grounded) {
            Jump();
        }
        //Controling the direction the sprite faces
        moveX = Input.GetAxis(HorizontalAxis);
        if (moveX < 0.0 && facingRight == true) {
            facingRight = false;
            kam.FlipDirection((int)(moveX/Mathf.Abs(moveX)));
        } else if (moveX > 0.0f && facingRight == false) {
            facingRight = true;
            kam.FlipDirection((int)(moveX / Mathf.Abs(moveX)));
        }
        //Setting the velocity of the character
        if (grounded) {
            rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);
        }

        kam.horizontalSpeed = Mathf.Abs(rb.velocity.x);
        kam.verticalSpeed = rb.velocity.y;
    }

    private void Jump() {
        if (!jumped && grounded) {
            kam.Jump();
            Invoke("JumpForce", jumpForceDelay);
            jumped = true;
        }
    }

    public void JumpForce() {
        jumped = false;
        rb.AddForce(Vector2.up * playerJH);
    }

    public void OnDrawGizmos() {
        if (drawGroundCheckGizmo) {
            Gizmos.color = new Color(0, 0, 255);
            Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        }
    }
}
