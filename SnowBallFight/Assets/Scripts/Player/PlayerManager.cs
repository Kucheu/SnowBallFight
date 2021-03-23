using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public TeamType teamType = TeamType.BlueTeam; 
    public PhotonView PV;
    GameObject controller;

    GameObject[] redTeamSpawn;
    GameObject[] blueTeamSpawn;

    public string userName;
    public int kills;
    public int deaths;

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

            PauseMenu.Instance.FirstOpen(this);
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

        if (!PV.IsMine) return;

        deaths++;
        ExitGames.Client.Photon.Hashtable hasz = new ExitGames.Client.Photon.Hashtable();
        hasz.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hasz);
    }

    public void ChangeTeam(TeamType _teamType)
    {
        
        
        if (PV.IsMine)
        {
            ExitGames.Client.Photon.Hashtable hasz = new ExitGames.Client.Photon.Hashtable();
            hasz.Add("teamType", _teamType);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hasz);
        }

        CreateController(_teamType);

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

        if(!PV.IsMine && targetPlayer == PV.Owner && changedProps.ContainsKey("kills"))
        {
            SetKills((int)changedProps["kills"]);
        }

        if (!PV.IsMine && targetPlayer == PV.Owner && changedProps.ContainsKey("deaths"))
        {
            SetDeaths((int)changedProps["deaths"]);
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

    public void GetKill()
    {
        if (!PV.IsMine) return;

        kills++;
        ExitGames.Client.Photon.Hashtable hasz = new ExitGames.Client.Photon.Hashtable();
        hasz.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hasz);
    }

    void SetKills(int _kills)
    {
        kills = _kills;
    }

    void  SetDeaths(int _deadhs)
    {
        deaths = _deadhs;
    }
}
