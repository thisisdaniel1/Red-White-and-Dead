﻿using UnityEngine;
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

		// Destroy any weapons if any
		if (rightGunBone.childCount > 0)
		{
			Destroy(rightGunBone.GetChild(0).gameObject);
		}
		if (leftGunBone.childCount > 0)
		{
			Destroy(leftGunBone.GetChild(0).gameObject);
		}


		// Instantiate and set up right hand
		if (hand.rightGun != null)
		{
			// Debug.Log("swapping right");
			GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
			newRightGun.transform.parent = rightGunBone;
			newRightGun.transform.localPosition = hand.RGLocalPosition;
			newRightGun.transform.localRotation = Quaternion.Euler(hand.RGLocalRotation);
			/*
			newRightGun.transform.localPosition = Vector3.zero;
			newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
			*/
		}

		// Instantiate and set up left hand
		if (hand.leftGun != null)
		{
			// Debug.Log("swapping left");
			GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
			newLeftGun.transform.parent = leftGunBone;
			newLeftGun.transform.localPosition = hand.LGLocalPosition;
			newLeftGun.transform.localRotation = Quaternion.Euler(hand.LGLocalRotation);
			/*
			newLeftGun.transform.localPosition = Vector3.zero;
			newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
			*/
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
				// InstantiateWeapon(i);

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

		public Vector3 RGLocalPosition;
		public Vector3 RGLocalRotation;
		//public Vector3 rightGunLocalScale;

		public Vector3 LGLocalPosition;
		public Vector3 LGLocalRotation;
		//public Vector3 leftGunLocalScale;
	}
}
