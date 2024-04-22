using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoomListContainer : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI playersInRoom;

    public void JoinThisRoom()
    {
       PhotonHandler.Instance.JoinRoom(roomName.text);
    }

}
