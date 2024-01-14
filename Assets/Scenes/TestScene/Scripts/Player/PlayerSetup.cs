using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public PlayerController playerController;

    public GameObject cam;

    public void IsLocalPlayer(){
        playerController.enabled = true;
        cam.SetActive(true);
    }
}
