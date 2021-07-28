using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        if(pv.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Pool_Player"), Vector3.zero, Quaternion.identity);

    }
}
