using UnityEngine;
using System.Collections;
using Photon.Pun;

[RequireComponent (typeof (Animator))]
public class Actions : MonoBehaviourPun {

	private Animator animator;

	public Camera cam;

	private CharacterRigController characterRigController;

	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;

	void Awake () {
		animator = GetComponent<Animator> ();
		characterRigController = GetComponent<CharacterRigController>();
	}

	void Update(){
			
		if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")){
			
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
		if (Input.GetKeyDown("6")){
			characterRigController.SetArsenal("PPK");
		}
		if (Input.GetKeyDown("7")){
			characterRigController.SetClothing("Shirt");
		}
		if (Input.GetKeyDown("8")){
			characterRigController.SetClothing("Pants");
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

	public void Attack (int damage, float range) {
		Aiming ();
		animator.SetTrigger ("Attack");

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);

		RaycastHit hit;

		if (Physics.Raycast(ray.origin, ray.direction, out hit, range)){

			Debug.Log("hitting target");

			// if thing that is hit has a health bar/component
			if (hit.transform.gameObject.GetComponentInParent<PlayerHealth>()){
				Debug.Log("Target has health and is hitting");
				 hit.transform.gameObject.GetComponentInParent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
				//PhotonView temp = hit.transform.gameObject.GetComponentInParent<PhotonView>(); //.RPC("Take Damage", RpcTarget.All, 25);
				//temp.gameObject.GetComponent<PlayerHealth>().TakeDamage(25);
			}
		}
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
