using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
     public TMP_Text text;
     private RoomInfo info;
     
     public void SetUp(RoomInfo _info)
     {
          info = _info;
          text.text = _info.Name;
     }

     public void OnClick()
     {
          Launcher.instance.findRoomMenu.SetActive(false);
          Launcher.instance.JoinRoom(info);
     }
}
