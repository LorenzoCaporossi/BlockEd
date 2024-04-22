using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonHandler : MonoBehaviourPunCallbacks
{
    public static PhotonHandler Instance;

    public delegate void PhotonHandlerDelegateString(bool status,string msg);
    public delegate void PhotonHandlerDelegateStringWithMasterStatus(bool isMaster,bool status,string msg);
    public delegate void PhotonHandlerDelegate(string msg);
    

    public static event PhotonHandlerDelegateString onConnection;
    public static event PhotonHandlerDelegateString onRoomJoin;
    public static event PhotonHandlerDelegateStringWithMasterStatus onRoomJoinWithMasterStatus;
    public static event PhotonHandlerDelegateString onRoomCreate;
    public static event PhotonHandlerDelegateString onLobby;
    public static event PhotonHandlerDelegate onOtherPlayerEnteredRoom;
    public static event PhotonHandlerDelegate onOtherPlayerLeftRoom;
    public static event PhotonHandlerDelegate onPlayerEnteredRoom;
    public static event PhotonHandlerDelegate onPlayerLeftRoom;
    public static event PhotonHandlerDelegate onDisconnection;


    [Tooltip("The maximum number of players per room")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;


    public GameObject roomListPrefab;
    public Transform roomListParent;

  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }
   
   

    private void Start()
    {
        // PlayerPrefs.SetInt("b", 0);
        // PlayerPrefs.Save();

        // loadingPanel.SetActive(true);
        //gamePanel.SetActive(false);
       Connect();
       // SetNickName(GameManager.Instance.playerName);
    }
    public void RefreshRooms()
    {
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "");
    }
    public void SetNickName(string pName)
    {
        PhotonNetwork.NickName = pName;
    }
    public void Connect()
    {
        if (CheckConnectionState())
        {
            return;
        }

        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            else
            {
               onLobby?.Invoke(true, "Lobby Joined");
            }
            onConnection?.Invoke(true, "Connected Photon");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();

        }
    }
    public override void OnConnectedToMaster()
    {
        onConnection?.Invoke(true, "");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
       

            onLobby?.Invoke(true, "Joined Lobby");

        }

    }
    public override void OnJoinedLobby()
    {



  onLobby?.Invoke(true, "");


    }

    public bool CheckConnectionState()
    {
        if (PhotonNetwork.InRoom || PhotonNetwork.NetworkClientState is ClientState.JoiningLobby or
           ClientState.Authenticating or ClientState.Joining or
           ClientState.ConnectingToMasterServer or ClientState.Leaving
           or ClientState.ConnectingToGameServer
           or ClientState.ConnectingToNameServer
           or ClientState.Disconnecting
           or ClientState.DisconnectingFromGameServer
           or ClientState.DisconnectingFromMasterServer
           or ClientState.DisconnectingFromNameServer)
        {

            return true;
        }
        return false;
    }
    public void CreateRoom(string roomName)
    {
        if (CheckConnectionState())
        {
            return;
        }
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = maxPlayersPerRoom,
            CleanupCacheOnLeave = false
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }


    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }


    public override void OnJoinedRoom()
    {
        onRoomJoin?.Invoke(true, "");
        onRoomJoinWithMasterStatus?.Invoke(PhotonNetwork.IsMasterClient,true,"Room Name: "+PhotonNetwork.CurrentRoom.Name+ "\nPlayers in Room  " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers);
        onPlayerEnteredRoom?.Invoke("");
      
    }
    public override void OnCreatedRoom()
    {
        onRoomCreate?.Invoke(true, "");
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomListParent != null)
        {
            for (int i = roomListParent.childCount - 1; i >= 0; i--)
            {
                Destroy(roomListParent.GetChild(i).gameObject);
            }

            for (int i = 0; i < roomList.Count; i++)
            {
                GameObject newRoom = Instantiate(roomListPrefab, roomListParent);
                newRoom.GetComponent<RoomListContainer>().roomName.text = roomList[i].Name;
                newRoom.GetComponent<RoomListContainer>().playersInRoom.text = roomList[i].PlayerCount + "/" + roomList[i].MaxPlayers;

            }
        }
       
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
       onOtherPlayerLeftRoom?.Invoke("");
        base.OnPlayerLeftRoom(otherPlayer);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
      onRoomJoin?.Invoke(false, "");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        onRoomCreate?.Invoke(false, "");
    }
    public void LeaveRoom()
    {
        if (CheckConnectionState())
        {
            return;
        }
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        onPlayerLeftRoom?.Invoke("");
        if(MultiPlayHandler.Instance!=null)
        {
            MultiPlayHandler.Instance.gameObject.SetActive(false);
        }
        Connect();

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

     
        onOtherPlayerEnteredRoom?.Invoke("Room Name: " + PhotonNetwork.CurrentRoom.Name + "\nPlayers in Room  " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers);


    }

    public void LoadGameScene(string sceneName)
    {
    
       
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(sceneName);
       
    }
    public bool CheckRoomFilledMaster()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount >= maxPlayersPerRoom && PhotonNetwork.IsMasterClient;
    }
    public bool CheckRoomFilledMaster(int players)
    {
        return PhotonNetwork.CurrentRoom.PlayerCount >= players;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Connect();
    }
    public override void OnLeftLobby()
    {
        Connect();
    }
}
