using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player; // main character that camera is following
	PlayerController playerScript;
	Camera cam;
	float xOffset; // horizontal offset needed for precise character flipping

	void Start () {
		cam = gameObject.GetComponent<Camera> ();
		playerScript = player.GetComponent<PlayerController> ();
	}

	void FixedUpdate () {
		if (playerScript.facingRight)
			xOffset = 0;
		else
			xOffset = 0.75f;

		/***	updating camera position to follow main character	***/

		cam.transform.position = new Vector3 (player.transform.position.x + xOffset , 
								cam.transform.position.y, cam.transform.position.z);
	}
}
