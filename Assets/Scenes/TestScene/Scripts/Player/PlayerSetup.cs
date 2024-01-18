using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public PlayerController playerController;

    public CharacterRigController characterRigController;

    public Actions actions;

    public GameObject cam;

    public void IsLocalPlayer(){
        playerController.enabled = true;
        //characterRigController.enabled = true;
        actions.enabled = true;
        cam.SetActive(true);
    }
}
