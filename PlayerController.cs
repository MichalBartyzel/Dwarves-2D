using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*** script for controlling player character ***/

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb2d;
	SpriteRenderer sr;
	Collider2D[] colliders;
	Animator anim;
	Killable enemy;

	static int attackState = Animator.StringToHash("Player_attack");
	static int shootState = Animator.StringToHash("Player_shoot");

	public float health = 100f;
	public float maxSpeed = 5f;
	public float jumpForce = 700f;
	float flipPositionOffset = 0.75f; // offset for horizontal character flipping
	float shootForce = 400;
	float shootPower = 50;
	float attackPower = 25;
	public bool facingRight = true;
	bool grounded = false; // is character touching ground?
	bool[] skillsCD = new bool[]{false,false}; // cooldowns for basic attack and shooting

	public Image healthBar; // UI health bar

	// Prefab and Transform for shooting
	public Rigidbody2D bulletPrefab;
	public Transform barrelPosition;

	// Transforms needed to check ground collision
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask groundLayer;

	// Transforms for checking axe attack hit collision
	public Transform hitCheck;
	float hitRadius = 0.5f;
	public LayerMask hitLayer;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		colliders = GetComponents <Collider2D> ();
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate () {

		// check if character is grounded
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, groundLayer);

		anim.SetFloat ("vertical speed", Mathf.Abs(rb2d.velocity.y));
		AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo (0);

		// get input on Horizontal Axis
		float move = Input.GetAxis ("Horizontal");

		// if character is grounded and not attacking or shooting change it's X velocity
		if (!(grounded && currentState.shortNameHash == attackState) && currentState.shortNameHash != shootState) {
			anim.SetFloat ("speed", Mathf.Abs (move));
			rb2d.velocity = new Vector2 (move * maxSpeed, rb2d.velocity.y);
		}

		// flip character when changing move direction
		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip ();

	}

	// get input on jumping, attacking and shooting
	void Update(){
		if (Input.GetKeyDown (KeyCode.Space))
			Jump ();
		if (Input.GetKeyDown (KeyCode.Q))
			Attack ();
		if (Input.GetKeyDown (KeyCode.E))
			Shoot ();
	}

	// Flip character and all it's colliders
	void Flip(){
		sr.flipX = !sr.flipX;
		transform.position = new Vector2 (transform.position.x - flipPositionOffset, transform.position.y);
		foreach(Collider2D coll in colliders){
			coll.offset = new Vector2 (-coll.offset.x, coll.offset.y);
		}
		hitCheck.localPosition = new Vector2 (-hitCheck.localPosition.x, hitCheck.localPosition.y);
		groundCheck.localPosition = new Vector2 (-groundCheck.localPosition.x, groundCheck.localPosition.y);
		barrelPosition.localPosition = new Vector2 (-barrelPosition.localPosition.x, barrelPosition.localPosition.y);
		facingRight = !facingRight;
		flipPositionOffset = -flipPositionOffset;
		shootForce = -shootForce;
	}

	// jump by adding force
	void Jump(){
		if (grounded) {
			rb2d.AddForce (new Vector2 (0,jumpForce));
		}
	}

	// start attack animation and set cooldown
	void Attack(){
		if (skillsCD[0] == true)
			return;
		anim.SetTrigger ("attack");
		StartCoroutine(setCooldown(0,0.1f));
	}

	/*** attack and check if you hit something
	 *   deal damage to killable enemies
	 *   this method is triggered by attack animation ***/
	void attemptAttack(){
		Collider2D hit = Physics2D.OverlapCircle (hitCheck.position, hitRadius, hitLayer);
		if (hit) {
			enemy = hit.gameObject.GetComponent<Killable> ();
			enemy.takeDamage (attackPower);
		}
	}

	// start shooting animation and set shoot cooldown
	void Shoot(){
		if (skillsCD[1] == true)
			return;
		anim.SetTrigger ("shoot");
		StartCoroutine(setCooldown(1,5f));
	}

	/*** instantiate and fire bullet prefab
	 *   this method is triggered at the end 
	 *   of shooting animation	***/
	void Fire(){
		Rigidbody2D bullet = Instantiate (bulletPrefab, barrelPosition.position,Quaternion.identity) as Rigidbody2D;
		bullet.AddForce (new Vector2 (shootForce, 0));
		bullet.gameObject.GetComponent<ProjectileController> ().setDamage(shootPower);
	}

	// coroutine for setting attacks cooldowns
	IEnumerator setCooldown(int skillN, float delay){
		skillsCD[skillN] = true;
		yield return new WaitForSeconds (delay);
		skillsCD[skillN] = false;
	}

	// lower player health when he gets hit
	public void takeDamage(float dmg){
		health -= dmg;
		if (health < 0) {
			health = 0;
		}
		updateHealth ();
	}

	// restore player health 
	public void restoreHealth(float heal){
		health += heal;
		if (health > 100)
			health = 100;
		updateHealth ();
	}

	// update UI health bar
	void updateHealth(){
		healthBar.GetComponent<RectTransform>().localScale = new Vector3(health/100,1,1);
	}
		
}
