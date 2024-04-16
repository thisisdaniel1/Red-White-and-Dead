using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardManager : MonoBehaviour
{
    public static BillboardManager Instance;

    public List<Billboard> billboards = new List<Billboard>();
    private Transform localPlayerTransform;

    void Awake(){
        Instance = this;
    }

    public void AddBillboard(Billboard billboard){
        if (!billboards.Contains(billboard)){
            billboards.Add(billboard);

            if (localPlayerTransform != null){
                billboard.SetTarget(localPlayerTransform);
            }
        }
    }

    public void SetLocalPlayer(Transform playerTransform){
        localPlayerTransform = playerTransform;
        foreach (var billboard in billboards){
            billboard.SetTarget(playerTransform);
        }
    }
}
