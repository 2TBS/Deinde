﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float maxSpeed = 7.0f;
	[Range(1, 10)] public float speed = 5.0f;
	[Range(1, 10)] public float jumpForce;
	private Rigidbody2D rb;
	Vector2 playerSize;
	Vector2 boxSize;
	private float groundCheckMargin = 0.05f;
	public LayerMask groundLayer;
	private bool grounded;


	[SerializeField] private Transform projectile;
	[Range(0, 15)] public float firePower = 15.0f;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		playerSize = GetComponent<BoxCollider2D>().size;
		boxSize = new Vector2(playerSize.x, groundCheckMargin);
	}

	void FixedUpdate() {
		grounded = false;
		Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
		grounded =  Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer) != null;
	}

    public void Move(float move, bool jump) {
		if(!WillHitWall(move)) {
			float t = rb.velocity.x / maxSpeed;
			
			float lerp = Mathf.Lerp(maxSpeed, 0f, Mathf.Abs(t));

			Vector2 movement = new Vector2(lerp * speed, 0f);
			if(move < 0)
				movement.x *= -1;
			else if(move == 0)
				movement.x = 0;
			
			if(grounded)
				rb.AddForce(movement * rb.mass, ForceMode2D.Force);
			else
				rb.AddForce(1.75f * movement * rb.mass, ForceMode2D.Force);
		}

		if(jump && grounded) {
			Jump();
		}

	}
	
    public void Shoot() {
        Transform clone = Instantiate(projectile, new Vector2(transform.position.x + (playerSize.x * 2f/3f), transform.position.y), 
			projectile.rotation);
		Debug.Log(firePower);
		clone.GetComponent<Rigidbody2D>().velocity = Vector2.right * firePower;
		Destroy(clone.gameObject, 10.0f);
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

}
