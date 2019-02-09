using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newcontroller : MonoBehaviour {

	public float playerSpeed = 10;
	public bool facingRight = true;
	public float playerJH = 1250;
	public float moveX;
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;


	// Update is called once per frame
	void Update () {

		PlayerMove ();
	}

	void FixedUpdate(){

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}

	void PlayerMove() {
		moveX = Input.GetAxis ("Horizontal");
		if (Input.GetButtonDown ("Jump") && grounded){
			Jump();
		}
		//Animations
		if (moveX < 0.0 && facingRight == true) {
			FlipPlayer ();
		} else if (moveX > 0.0f && facingRight == false) {
			FlipPlayer ();
		}

		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D> ().velocity.y);
	}
		void Jump(){
		GetComponent<Rigidbody2D> ().AddForce (Vector2.up * playerJH);
	}

	void FlipPlayer() {
		facingRight = !facingRight;
		Vector2 localScale = gameObject.transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
	}


}
