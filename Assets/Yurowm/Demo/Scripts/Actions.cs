using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class Actions : MonoBehaviour {

	private Animator animator;

	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;

	void Awake () {
		animator = GetComponent<Animator> ();
	}

	void Update(){
		if (Input.GetKeyDown("e")){
			Aiming();
		}
		else if (Input.GetKeyDown("r")){
			Attack();
		}
			
		else if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")){
			
			if (Input.GetKey("left shift")){
				Run();
			}
			else{
				Walk();
			}
			
		}

		if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("s") || Input.GetKeyUp("d")){
			Stay();
		}
	}

	public void Stay () {
		animator.SetBool("Aiming", false);
		animator.SetFloat ("Speed", 0f);
		}

	public void Walk () {
		animator.SetBool("Aiming", false);
		animator.SetFloat ("Speed", 0.5f);
	}

	public void Run () {
		animator.SetBool("Aiming", false);
		animator.SetFloat ("Speed", 1f);
	}

	public void Attack () {
		Aiming ();
		animator.SetTrigger ("Attack");
	}

	public void Death () {
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Death"))
			animator.Play("Idle", 0);
		else
			animator.SetTrigger ("Death");
	}

	public void Damage () {
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Death")) return;
		int id = Random.Range(0, countOfDamageAnimations);
		if (countOfDamageAnimations > 1)
			while (id == lastDamageAnimation)
				id = Random.Range(0, countOfDamageAnimations);
		lastDamageAnimation = id;
		animator.SetInteger ("DamageID", id);
		animator.SetTrigger ("Damage");
	}

	public void Jump () {
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0f);
		animator.SetBool("Aiming", false);
		animator.SetTrigger ("Jump");
	}

	public void Aiming () {
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0f);
		animator.SetBool("Aiming", true);
	}

	public void Sitting () {
		animator.SetBool ("Squat", !animator.GetBool("Squat"));
		animator.SetBool("Aiming", false);
	}
}
