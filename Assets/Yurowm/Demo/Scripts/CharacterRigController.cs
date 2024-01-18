using UnityEngine;
using System.Collections;
using Photon.Pun;

[RequireComponent (typeof (Animator))]
public class CharacterRigController : MonoBehaviourPun {

	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;

	private Animator animator;

	private PhotonView photonView;

	void Awake() {
		animator = GetComponent<Animator> ();

		photonView = GetComponent<PhotonView>();
		
		if (arsenal.Length > 0)
			SetArsenal (arsenal[0].name);
		}

	void Update(){

		/*
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
		*/
	}

	[PunRPC]
    private void ChangeAnimatorController(int controllerIndex)
    {
        // Check if the controllerIndex is within the bounds of your arsenal array
        if (controllerIndex >= 0 && controllerIndex < arsenal.Length)
        {
            animator.runtimeAnimatorController = arsenal[controllerIndex].controller;
        }
    }

    private void ChangeRemoteAnimatorController(int controllerIndex)
    {
        photonView.RPC("ChangeAnimatorController", RpcTarget.All, controllerIndex);
    }



	[PunRPC]
	public void SwitchWeapon(int selectedWeaponIndex)
	{
		if (selectedWeaponIndex >= 0 && selectedWeaponIndex < arsenal.Length)
		{
			// Assuming you have a method to instantiate weapons based on an index
			InstantiateWeapon(selectedWeaponIndex);
		}
	}

	private void InstantiateWeapon(int weaponIndex)
	{
		Arsenal hand = arsenal[weaponIndex];

		// Instantiate and set up right hand
		if (hand.rightGun != null)
		{
			GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
			newRightGun.transform.parent = rightGunBone;
			newRightGun.transform.localPosition = Vector3.zero;
			newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
		}

		// Instantiate and set up left hand
		if (hand.leftGun != null)
		{
			GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
			newLeftGun.transform.parent = leftGunBone;
			newLeftGun.transform.localPosition = Vector3.zero;
			newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
		}
	}


	public void SetArsenal(string name)
	{
		for (int i = 0; i < arsenal.Length; i++)
		{
			Arsenal hand = arsenal[i];
			if (hand.name == name)
			{
				// Destroy any weapons if any
				if (rightGunBone.childCount > 0)
				{
					Destroy(rightGunBone.GetChild(0).gameObject);
				}
				if (leftGunBone.childCount > 0)
				{
					Destroy(leftGunBone.GetChild(0).gameObject);
				}

				// Instantiate and set up weapons
				InstantiateWeapon(i);

				// Set animator controller
				animator.runtimeAnimatorController = hand.controller;

				// Change remote animator controller
				ChangeRemoteAnimatorController(i);

				// Switch weapon using RPC by sending the weapon index
				photonView.RPC("SwitchWeapon", RpcTarget.All, i);

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
