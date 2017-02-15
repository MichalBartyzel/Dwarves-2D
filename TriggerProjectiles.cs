using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***	Projectiles fired by enemies are
 *  	destroyed only when collide with player
 *		or environment (not other enemies)	***/

public class TriggerProjectiles : MonoBehaviour {

	PlayerController player;
	float damage;
	public string enemyTag;
	public string friendlyTag;

	void Start () {
		/*** if projectile did not collide with anything
		 *	 within first 5 seconds of it's life, destroy it ***/	
		Destroy (gameObject, 5); 
	}

	void OnTriggerEnter2D(Collider2D other){
		// if triggered by player, he takes damage
		if (other.gameObject.tag == enemyTag) {
			player = other.gameObject.GetComponent<PlayerController> ();
			player.takeDamage (damage);
		}
		// if projectile hits player of environment, destroy it
		if(other.gameObject.tag != friendlyTag && other.gameObject.tag != "PlayerGizmo" && other.gameObject.tag != "Collectible")
			Destroy (gameObject);
	}

	public void setDamage(float dmg){
		damage = dmg;
	}
}
