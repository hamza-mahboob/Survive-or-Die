using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            //host is killer
            if (PhotonNetwork.IsMasterClient)
            {
                CreateKillerController();
                return;
            }
            CreateController();
        }
    }

    void CreateController()
    {
        Debug.Log("Instantiate player controller");
        //instantiate player controller
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }

    void CreateKillerController()
    {
        //create enemy at host
        Debug.Log("Instantiate killer controller");
        //instantiate killer controller
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "KillerController"), Vector3.zero, Quaternion.identity);
    }
}