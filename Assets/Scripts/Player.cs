﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private float maxSpeed = 15.0f;
	[Range(1, 10)] public float moveSpeed = 5.0f;
	[Range(1, 10)] public float jumpForce;
	private Rigidbody2D rb;
	private Vector2 playerSize;
	private Vector2 boxSize;
	private float groundCheckMargin = 0.05f;
	public LayerMask groundLayer;
	public bool speedPowerUpOn;
	private bool grounded;

	[SerializeField] private Transform projectile;
	[Range(0, 15)] public float firePower = 15.0f;
	private float time;
	public bool destructionPowerUpOn;

	//Player health
	private const int MAX_HEALTH = 3;
	public int health = MAX_HEALTH;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		playerSize = GetComponent<BoxCollider2D>().size;
		// Small rectangle that is very thin that spans the underneath of the player body
		boxSize = new Vector2(playerSize.x, groundCheckMargin);
		destructionPowerUpOn = false;
		time = 0;
	}

	void FixedUpdate() {
		grounded = false; // If player is touching the ground 
		// Overlap with the ground to check for a ground collision
		Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
		grounded =  Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer) != null;

		if(!CheckInView()) {
			ResetPosition();
		}
		//Add timer for powerup
	}

	//Checks if the player is in view of the camera
	bool CheckInView() {
		//If you see an error in your editor for the next line, ign it. It's a unity thing
		if (!GetComponent<Renderer>().IsVisibleFrom(Camera.main)) {
	   		Debug.Log("Not Visible");
		}
		return GetComponent<Renderer>().IsVisibleFrom(Camera.main);
	}

	void ResetPosition() {
		Vector3 centerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
        this.transform.position = centerPos;
		rb.velocity = new Vector2(0.0f, 0.0f);
		Debug.Log("Position Reset, deducted health");
		Debug.Log("New health: " + health);
		health--;
	}

	/*void Update() {
		if(destructionPowerUpOn) {
			time += Time.deltaTime;
			if(time > seconds) {
				time = 0;
				//Set Destruction back to OG Destruction
			}
		}
		else {time = 0;}
	}*/

    public void Move(float move, bool jump) { // Do not edit this method -Adarsh
		// Scaled movement that is proportional to current velocity
		if(!WillHitWall(move)) {
			float t = rb.velocity.x / maxSpeed;
			
			float lerp = Mathf.Lerp(maxSpeed, 0f, Mathf.Abs(t));

			Vector2 movement = new Vector2(move * lerp, 0f);

			// rb.AddForce(movement * rb.mass, ForceMode2D.Impulse);
			rb.velocity = new Vector2(movement.x, rb.velocity.y);

			Debug.Log(rb.velocity.x + ", " + lerp);

		}

		if(jump && grounded) {
			Jump();
		}

	}

	private void Jump() {
		GetComponent<Rigidbody2D>().gravityScale = 1;
		GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		grounded = false;
	}

	private bool WillHitWall(float move) {
		RaycastHit2D hit;
		bool hitWall = false;
		if (move > 0f) {
			if(hit = Physics2D.CircleCast(transform.position, .5f, transform.right, .6f)) {
				if(hit.collider.tag == "Environment") {
					hitWall = true;
				}
			}
		}
		else if (move < 0f) {
			if(hit = Physics2D.CircleCast(transform.position, .5f, -transform.right, .6f)) {
				if(hit.collider.tag == "Environment") {
					hitWall = true;
				}
			}
		}

		return hitWall;
	}

	public void Shoot() {
        Transform clone = Instantiate(projectile, new Vector2(transform.position.x + (playerSize.x * 2f/3f), transform.position.y), 
			projectile.rotation);
		clone.GetComponent<Rigidbody2D>().velocity = Vector2.right * firePower;
		Destroy(clone.gameObject, 10.0f);
    }


	public void PowerUpSpeed(float speedIncrease, float duration) {
		moveSpeed += speedIncrease;
	}

	public void PowerUpDestruction(float destructionIncrease, float duration) {
		//destruction += destructionIncrease;
	}

}
