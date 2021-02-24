using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public TeamType teamType; 
    PhotonView PV;
    GameObject controller;

    GameObject[] redTeamSpawn;
    GameObject[] blueTeamSpawn;

    public string userName;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(PV.IsMine)
        {
            redTeamSpawn = GameObject.FindGameObjectsWithTag("redTeamSpawn");
            blueTeamSpawn = GameObject.FindGameObjectsWithTag("blueTeamSpawn");

            ChangeTeamMenu.Instance.FirstOpen(this);
        }
    }

    public void CreateController(TeamType _teamtype = TeamType.BlueTeam)
    {
        
        teamType = _teamtype;
        if(PV.IsMine)
        {
            if(controller != null)
            {
                PhotonNetwork.Destroy(controller);
            }

            Transform _spawn;
            switch (_teamtype)
            {
                case TeamType.BlueTeam:
                    _spawn = blueTeamSpawn[(int)Random.Range(0, redTeamSpawn.Length)].transform;
                    controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "BluePlayerController"), _spawn.position, _spawn.rotation, 0, new object[] { PV.ViewID });
                    break;

                case TeamType.RedTeam:
                    _spawn = redTeamSpawn[(int)Random.Range(0, redTeamSpawn.Length)].transform;
                    controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "RedPlayerController"), _spawn.position, _spawn.rotation, 0, new object[] { PV.ViewID });
                    break;
            }
        }
        


    }

    public void Die()
    {
        //PhotonNetwork.Destroy(controller);

        //Trigger death animation
        CreateController(teamType);
    }

    public void ChangeTeam(TeamType _teamType)
    {
        
        CreateController(_teamType);
        if (PV.IsMine)
        {
            ExitGames.Client.Photon.Hashtable hasz = new ExitGames.Client.Photon.Hashtable();
            hasz.Add("teamType", _teamType);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hasz);
        }
        
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner && changedProps.ContainsKey("teamType"))
        {
            CreateController((TeamType)changedProps["teamType"]);
        }

        if(!PV.IsMine && targetPlayer == PV.Owner && changedProps.ContainsKey("userName"))
        {
            SetUserName((string)changedProps["userName"]);
        }
    }

    public void SetUserName(string _userName)
    {

        if(PV.IsMine)
        {
            ExitGames.Client.Photon.Hashtable hasz = new ExitGames.Client.Photon.Hashtable();
            hasz.Add("userName", _userName);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hasz);
        }
        userName = _userName;
    }
}
