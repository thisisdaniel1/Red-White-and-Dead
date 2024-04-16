using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject cam;
    public GameObject weaponSprite;
    private PhotonView photonView;

    private void Awake() {
        photonView = GetComponent<PhotonView>();
    }

    public void InitializePlayer() {
        if (photonView.IsMine) {
            playerController.enabled = true;
            cam.SetActive(true);
            weaponSprite.SetActive(true);
            BillboardManager.Instance.SetLocalPlayer(transform);
            EnemyManager.Instance.SetLocalPlayer(transform);
        } else {
            playerController.enabled = false;
            cam.SetActive(false);
        }
    }
}