using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class CharacterRigController : MonoBehaviour {

	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;

	private Animator animator;

	void Awake() {
		animator = GetComponent<Animator> ();
		if (arsenal.Length > 0)
			SetArsenal (arsenal[0].name);
		}

	void Update(){
		if (Input.GetKeyDown("1")){
			SetArsenal("Empty");
		}
		if (Input.GetKeyDown("2")){
			SetArsenal("One Pistol");
		}
		if (Input.GetKeyDown("3")){
			SetArsenal("Two Pistols");
		}
		if (Input.GetKeyDown("4")){
			SetArsenal("Sniper Rifle");
		}
		if (Input.GetKeyDown("5")){
			SetArsenal("Musket");
		}
	}

	public void SetArsenal(string name) {
		foreach (Arsenal hand in arsenal) {
			if (hand.name == name) {
				if (rightGunBone.childCount > 0)
					Destroy(rightGunBone.GetChild(0).gameObject);
				if (leftGunBone.childCount > 0)
					Destroy(leftGunBone.GetChild(0).gameObject);
				if (hand.rightGun != null) {
					GameObject newRightGun = (GameObject) Instantiate(hand.rightGun);
					newRightGun.transform.parent = rightGunBone;
					newRightGun.transform.localPosition = Vector3.zero;
					newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
					}
				if (hand.leftGun != null) {
					GameObject newLeftGun = (GameObject) Instantiate(hand.leftGun);
					newLeftGun.transform.parent = leftGunBone;
					newLeftGun.transform.localPosition = Vector3.zero;
					newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
				}
				animator.runtimeAnimatorController = hand.controller;
				return;
			}
		}
	}

	[System.Serializable]
	public struct Arsenal {
		public string name;
		public GameObject rightGun;
		public GameObject leftGun;
		public RuntimeAnimatorController controller;
	}
}
