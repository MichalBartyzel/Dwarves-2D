using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*** scouting melee character ***/

public class OrcGrunt : ScoutingCharacter {

	static int attackState = Animator.StringToHash("Orc_attack");
	static int deathState = Animator.StringToHash("Orc_death");
	float hitDamage = 20f;

	public Transform hitCheck;
	public float hitRadius = 0.5f;
	public LayerMask hitLayer;

	float maxSpeed; // movement speed
	bool attackCD = false; // is attack on cooldown?

	void Start () {
		base.Start ();
		maxSpeed = speed;
	}

	void FixedUpdate () {
		base.FixedUpdate ();
	}

	// check if character is not performing other action and can move
	protected override bool canMove ()
	{
		if (currentState.shortNameHash == attackState || currentState.shortNameHash == deathState)
			return false;
		return base.canMove ();
	}

	/*** if player enters trigger collider
	 * 	 attached to the orc, stop
	 * 	 and check if you can attack ***/
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			speed = 0;
			if (!attackCD) {
				anim.SetTrigger ("attack");
				attackCD = true;
			}
		}
	}

	/*** try to attack and check if player is hit
	 *   this method is triggered near the end of
	 *   attack animation
	 *   deal damage to player if he is hit	***/

	IEnumerator attemptAttack(){
		Collider2D hit = Physics2D.OverlapCircle (hitCheck.position, hitRadius, hitLayer);
		if (hit) {
			player = hit.gameObject.GetComponent<PlayerController> ();
			player.takeDamage (hitDamage);
		}
		yield return new WaitForSeconds (2f); // wait in place after attack before going back to scouting
		speed = maxSpeed;	// resume scouting
		attackCD = false;
	}
		
}
