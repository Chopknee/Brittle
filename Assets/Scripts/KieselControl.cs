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
    public float moveX;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool grounded;
    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;
    public bool drawGroundCheckGizmo;

    public Vector2 lastVelocity;
    private bool setAnimationState = false;
    public bool jump = false;
    public void Start() {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
        lastVelocity = rb.velocity;
    }

    
    public void Update() {
        PlayerMove();
    }

    public void FixedUpdate() {
        //Checking if the character is on the ground
        if (!jump) {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
    }

    private void PlayerMove() {
        moveX = Input.GetAxis(HorizontalAxis);
        if (Input.GetButtonDown(JumpButton) && grounded) {
            Jump();
        }
        //Controling the direction which the sprite faces
        if (moveX < 0.0 && facingRight == true) {
            FlipSprite();
        } else if (moveX > 0.0f && facingRight == false) {
            FlipSprite();
        }
        //Setting the velocity of the character
        rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);

        if (rb.velocity != lastVelocity && jump == false) {
            //The velocity has changed
            if (Mathf.Abs(rb.velocity.x) > 0 && grounded) {//Running animation
                skeletonAnimation.AnimationState.SetAnimation(0, "run", true);
            }

            if (Mathf.Abs(rb.velocity.x) < 0.01 && grounded) {//Idle animation
                skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            }
        }

        lastVelocity = rb.velocity;
    }

    private void Jump() {
        jump = true;
        rb.AddForce(Vector2.up * playerJH);
        skeletonAnimation.AnimationState.SetAnimation(0, "jump", false);
        Invoke("JumpDel", 0.1f);
    }

    private void FlipSprite() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void OnDrawGizmos() {
        if (drawGroundCheckGizmo) {
            Gizmos.color = new Color(0, 0, 255);
            Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        }
    }

    public void JumpDel() {
        jump = false;
    }
}
