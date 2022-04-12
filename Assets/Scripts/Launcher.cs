using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    public GameObject mainMenu, loadingMenu, createRoomMenu, roomMenu, errorMenu, findRoomMenu;
    public TMP_InputField roomNameInput;
    public TMP_Text errorText, roomNameText;
    public Transform roomListContent;
    public GameObject roomListItemPrefab;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        loadingMenu.SetActive(true);

        //connect using the settings in photon networking file
        PhotonNetwork.ConnectUsingSettings();
    }

    //overriden photon methods
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        //join lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //open main menu
        mainMenu.SetActive(true);
        loadingMenu.SetActive(false);
        Debug.Log("Joined Lobby");
    }


    public override void OnJoinedRoom()
    {
        loadingMenu.SetActive(false);
        roomMenu.SetActive(true);
        roomNameText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        loadingMenu.SetActive(false);
        errorMenu.SetActive(true);

        errorText.text = "Room creation failed:" + message;
    }

    public override void OnLeftRoom()
    {
        loadingMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //clear the list
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        //initiate rooms
        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp((roomList[i]));
        }
    }


    //functions for buttons
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
            return;

        PhotonNetwork.CreateRoom(roomNameInput.text);
        createRoomMenu.SetActive(false);
        loadingMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        errorMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenCreateMenu()
    {
        mainMenu.SetActive(false);
        createRoomMenu.SetActive(true);
    }

    public void OpenFindMenu()
    {
        mainMenu.SetActive(false);
        findRoomMenu.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomMenu.SetActive(false);
        loadingMenu.SetActive(true);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom((info.Name));
        loadingMenu.SetActive(true);
    }

    public void CloseJoinRoom()
    {
        findRoomMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}