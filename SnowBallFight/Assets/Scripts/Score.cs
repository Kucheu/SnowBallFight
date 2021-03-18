using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Score : MonoBehaviourPunCallbacks
{

    public int blueScore { get; private set; } = 0;

    public int redScore { get; private set; } = 0;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        PV.RPC("GetPointFromMaster", RpcTarget.MasterClient);
    }

    public void ResetScore()
    {
        blueScore = 0;
        redScore = 0;
    }

    public void AddPoint(TeamType teamType)
    {
        PV.RPC("AddPointPunRPC", RpcTarget.MasterClient, teamType);
    }

    [PunRPC]
    public void GetPointFromMaster()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PV.RPC("SetPoints", RpcTarget.All, blueScore, redScore);
    }

    [PunRPC]
    public void AddPointPunRPC(TeamType teamType)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        switch (teamType)
        {
            case TeamType.BlueTeam:
                blueScore++;
                break;

            case TeamType.RedTeam:
                redScore++;
                break;
        }

        PV.RPC("SetPoints", RpcTarget.All, blueScore, redScore);
        
        
    }

    [PunRPC]
    public void SetPoints(int _blueScore, int _redScore)
    {
        if (PhotonNetwork.IsMasterClient) return;


        blueScore = _blueScore;
        redScore = _redScore;
    }
   

    
}
