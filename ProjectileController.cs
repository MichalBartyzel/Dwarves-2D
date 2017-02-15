using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	PlayerController player;
	Killable enemy;
	float damage;
	public string enemyTag;
	public string friendlyTag;

	void Start () {
		Destroy (gameObject, 5); // lifetime of projectile
	}

	/*** destroy projectile on any collision,
	 * 	 deal damage to first enemy hit ***/
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == enemyTag) {
			enemy = other.gameObject.GetComponent<Killable> ();
			enemy.takeDamage (damage);
		}
		Destroy (gameObject);
	}

	public void setDamage(float dmg){
		damage = dmg;
	}
}