using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GameplayCore.Menu;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text ErrorText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        Instance = this;
    }

    #region PublicFunctions

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master ");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        PageController.Instance.TurnPageOff(PageType.LoadingMenu, PageType.MainMenu);
        Debug.Log("joined Lobby");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) return;

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        PageController.Instance.TurnPageOff(PageType.CreateRoom, PageType.LoadingMenu);
    }

    public override void OnJoinedRoom()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PageController.Instance.TurnPageOff(PageType.LoadingMenu, PageType.Lobby);

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform _trans in playerListContent)
        {
            Destroy(_trans.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorText.text = "Room Creation Failed" + message;
        PageController.Instance.TurnPageOff(PageType.LoadingMenu, PageType.ErrorMenu);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        PageController.Instance.TurnPageOff(PageType.FindRoom, PageType.LoadingMenu);
    }

    public void LeftRoom()
    {
        PhotonNetwork.LeaveRoom();
        PageController.Instance.TurnPageOff(PageType.Lobby, PageType.LoadingMenu);
    }

    public override void OnLeftRoom()
    {
        PageController.Instance.TurnPageOff(PageType.LoadingMenu, PageType.MainMenu);
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform _trans in roomListContent)
        {
            Destroy(_trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) continue;

            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #endregion

    #region PrivateFuntions


    #endregion





}
