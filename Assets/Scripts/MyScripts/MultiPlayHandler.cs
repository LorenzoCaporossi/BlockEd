using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayHandler : MonoBehaviourPunCallbacks
{
    public static MultiPlayHandler Instance;
    public int ID;
    [SerializeField] PhotonView pv;
    [SerializeField]GamePlayHandler gamePlayHandler;
    [SerializeField] string playerObjName;

    [SerializeField] GameObject connectionCanvas;
    [SerializeField] POWChallenge pow;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
       
    }
    public override void OnEnable()
    {
        pow.CubeDone = false;
        pow.OtherPlayerTxt.gameObject.SetActive(false);
        ID = PhotonNetwork.LocalPlayer.ActorNumber;
        if (PhotonNetwork.IsMasterClient)
        {
           // PhotonNetwork.Instantiate(playerObjName, Vector3.zero, Quaternion.identity);
        }
    }
    public void SendPlayerDone()
    {
        pv.RPC("PlayerDone", RpcTarget.Others);
    }

    
    public void StartSetConnectionCanvasOff()
    {
        pv.RPC("SetConnectionCanvasOff", RpcTarget.All);
    }

    [PunRPC]
    public void SetConnectionCanvasOff()
    {
     connectionCanvas.SetActive(false);
    }
    [PunRPC]
    public void PlayerDone()
    {
        pow.CubeDone = true;
        pow.OtherPlayerTxt.gameObject.SetActive(true);
        pow.ShowSolution();
        pow.failPanel.gameObject.SetActive(true);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
       if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerObjName, Vector3.zero, Quaternion.identity);
        }
    }
}
