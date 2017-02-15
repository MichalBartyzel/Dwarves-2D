using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*** script for collectible coins objects ***/

public class CoinScript : MonoBehaviour {

	public static int coins;
	Text coinText;	// UI coin counter to be updated

	void Awake(){
		coinText = GameObject.Find ("CoinsText").GetComponent<Text>();
	}

	// on collision with player update coin counter
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			Destroy (gameObject);
			coins++;
			coinText.text = "" + coins;
		}
	}
}
