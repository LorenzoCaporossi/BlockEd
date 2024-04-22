using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerHandler : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (stream.IsWriting)
            {
                //stream.SendNext(GamePlayHandler.Instance.playersDone[0]);
                //stream.SendNext(GamePlayHandler.Instance.playersDone[1]);
                //stream.SendNext(GamePlayHandler.Instance.playersDone[2]);
                //stream.SendNext(GamePlayHandler.Instance.playersDone[3]);
            }
            else if (stream.IsReading)
            {
                //GamePlayHandler.Instance.playersDone[0]=(int)stream.ReceiveNext();
                //GamePlayHandler.Instance.playersDone[1]=(int)stream.ReceiveNext();
                //GamePlayHandler.Instance.playersDone[2]=(int)stream.ReceiveNext();
                //GamePlayHandler.Instance.playersDone[3]=(int)stream.ReceiveNext();
            }
        }
        
    }
}
