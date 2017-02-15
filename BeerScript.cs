using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***	Restore player health when collected	***/

public class BeerScript : MonoBehaviour {

	public float healAmount;

	PlayerController player;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			Destroy (gameObject);
			player = other.gameObject.GetComponent<PlayerController> ();
			player.restoreHealth (healAmount);
		}
	}
}
