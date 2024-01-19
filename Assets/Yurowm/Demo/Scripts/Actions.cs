using UnityEngine;
using System.Collections;
using Photon.Pun;

[RequireComponent (typeof (Animator))]
public class Actions : MonoBehaviourPun {

	private Animator animator;

	private CharacterRigController characterRigController;

	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;

	void Awake () {
		animator = GetComponent<Animator> ();
		characterRigController = GetComponent<CharacterRigController>();
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

		if (Input.GetKeyDown("1")){
			characterRigController.SetArsenal("Empty");
		}
		if (Input.GetKeyDown("2")){
			characterRigController.SetArsenal("One Pistol");
		}
		if (Input.GetKeyDown("3")){
			characterRigController.SetArsenal("Two Pistols");
		}
		if (Input.GetKeyDown("4")){
			characterRigController.SetArsenal("Sniper Rifle");
		}
		if (Input.GetKeyDown("5")){
			characterRigController.SetArsenal("Brown Bess");
		}
	}

	public void Stay () {
		SetRemoteAnimationParameters(false, 0f);
	}

	public void Walk () {
		SetRemoteAnimationParameters(false, 0.5f);
	}

	public void Run () {
		SetRemoteAnimationParameters(false, 1f);
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

	[PunRPC]
	void SetAnimationParameters(bool aiming, float speed){
		animator.SetBool("Aiming", aiming);
		animator.SetFloat("Speed", speed);
	}

	void SetRemoteAnimationParameters(bool aiming, float speed){
		photonView.RPC("SetAnimationParameters", RpcTarget.All, aiming, speed);
	}
}
