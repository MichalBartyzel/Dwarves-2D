using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*** class that allows enemy characters to move around and scout area ***/

public class ScoutingCharacter : Killable{

	public float facing = -1f; // 1 - facing right, -1 - facing left
	public float speed;
	public float flipOffset;
	public float scoutFrom;
	public float scoutTo;
	protected Rigidbody2D rb2d;
	protected Collider2D[] colliders;
	protected SpriteRenderer sr;
	protected AnimatorStateInfo currentState;

	protected void Start () {
		base.Start ();

		rb2d = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer>();
		colliders = GetComponents <Collider2D> ();
	}
		
	/*** character will scout ahead with it's speed
	 *   moving forward and changing direction upon
	 *   reaching one of the scouting end point ***/
	protected void FixedUpdate () {
		anim.SetFloat ("speed", Mathf.Abs (rb2d.velocity.x));
		currentState = anim.GetCurrentAnimatorStateInfo (0);

		if(!canMove())	
			return;
		if (transform.position.x <= scoutFrom || transform.position.x >= scoutTo) {
			flip ();
		}
		rb2d.velocity = new Vector2 (speed * facing, rb2d.velocity.y);
	}

	// flip character and it's colliders when changing movement direction
	protected void flip(){
		facing = -facing;
		sr.flipX = !sr.flipX;
		transform.position = new Vector2 (transform.position.x - flipOffset, transform.position.y);
		foreach(Collider2D coll in colliders){
			coll.offset = new Vector2 (-coll.offset.x, coll.offset.y);
		}
		flipOffset = -flipOffset;
	}

	/*** if both scoutFrom and scoutTo are equall 0
	 * 	 the character will remain stationary ***/
	virtual protected bool canMove(){
		if(scoutFrom == 0 && scoutTo == 0)
			return false;
		return true;
	}
}
