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
        Transform spawnPoint = SpawnManager.instance.GetRandomSpawnPoint();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position, spawnPoint.rotation);
    }

    void CreateKillerController()
    {
        //create enemy at host
        Debug.Log("Instantiate killer controller");
        //instantiate killer controller
        Transform spawnPoint = SpawnManager.instance.GetRandomSpawnPoint();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "KillerController"), spawnPoint.position, spawnPoint.rotation);
    }
}