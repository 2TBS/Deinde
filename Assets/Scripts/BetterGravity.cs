﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterGravity : MonoBehaviour {

	public float fallMult = 3f;
	public float lowJumpMult = 2f;
	
	Rigidbody2D rb;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		if(rb.velocity.y < 0) {
			rb.gravityScale = fallMult;
		} else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
				rb.gravityScale = lowJumpMult;
		} else {
			rb.gravityScale = 1;
		}
	}

}