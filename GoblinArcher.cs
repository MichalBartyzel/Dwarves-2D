using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***	stationary shooting enemy	***/

public class GoblinArcher : Killable {

	public float shootForce; // force applied to arrow (x-axis)
	public float shootDelay = 2f; // delay between attacks
	public Rigidbody2D bulletPrefab;
	public Transform bowPosition;
	float shootDamage=25f;
	bool shootCD = false;

	void Start () {
		base.Start ();
	}

	void FixedUpdate () {
		Shoot ();
	}

	//shoot arrow if attack is not on cooldown
	void Shoot(){
		if (!shootCD) {
			anim.SetTrigger ("shoot"); //starts shooting animation that triggers Fire() method
			StartCoroutine (setCooldown());
		}
	}

	//coroutine for setting cooldown between attacks
	IEnumerator setCooldown(){
		shootCD = true;
		yield return new WaitForSeconds (shootDelay);
		shootCD = false;
	}

	/*** triggered at the end of shooting animation
	 * 	 instantiate and fire arrow prefab		***/
	void Fire(){
		Rigidbody2D bullet = Instantiate (bulletPrefab, bowPosition.position,Quaternion.identity) as Rigidbody2D;
		bullet.AddForce (new Vector2 (shootForce, 0));
		bullet.gameObject.GetComponent<TriggerProjectiles> ().setDamage(shootDamage);
	}
}
