using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*** class for enemies that can be destroyed ***/

public class Killable : MonoBehaviour {

	public float health; // creature HP
	protected Animator anim;
	protected PlayerController player;

	protected void Start () {
		anim = GetComponent<Animator> ();
		anim.SetFloat ("health",health);
	}

	// lower health when hit, dies when health reaches 0 or lower
	public void takeDamage(float dmg){
		health -= dmg;
		anim.SetFloat ("health",health);
	}

	// triggered by death animation, destroys this creature
	protected void Die(){
		Destroy (gameObject);
	}
}
