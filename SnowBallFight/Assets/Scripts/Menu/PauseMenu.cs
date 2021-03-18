using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayCore.Menu;
using Photon.Pun;
public class PauseMenu : MonoBehaviourPunCallbacks
{

    public static PauseMenu Instance;
    PlayerManager playerManager;

    [SerializeField] GameObject firstCamera;

    bool firstChangeTeam = false;

    private bool menuActive = true;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(menuActive)
            {
                if(PageController.Instance.activePageType == PageType.ChangeTeamMenu)
                {
                    PageController.Instance.TurnPageOff(PageType.ChangeTeamMenu);
                    CloseMenu();
                }
            }
            else
            {
                PageController.Instance.TurnPageOn(PageType.ChangeTeamMenu);
                OpenMenu();
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PageController.Instance.activePageType == PageType.none)
            {
                PageController.Instance.TurnPageOn(PageType.PauseMenu);
                OpenMenu();
            }
            else
            {
                if(!PageController.Instance.StepBackPage())
                {
                    PageController.Instance.TurnPageOff(PageController.Instance.activePageType);
                    CloseMenu();
                }
            }
        }
    }


    void CloseMenu()
    {
        menuActive = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OpenMenu()
    {
        menuActive = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void FirstOpen(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
        firstCamera.SetActive(true);
        OpenMenu();
        PageController.Instance.TurnPageOn(PageType.ChangeTeamMenu);
    }

    public void changeTeam(string _teamType)
    {

        firstChangeTeam = true;
        switch (_teamType)
        {
            case "RedTeam":
                playerManager.ChangeTeam(TeamType.RedTeam);
                break;
            case "BlueTeam":
                playerManager.ChangeTeam(TeamType.BlueTeam);
                break;
            default:
                playerManager.ChangeTeam(TeamType.BlueTeam);
                break;
        }
        if(firstCamera.activeSelf)
        {
            firstCamera.SetActive(false);
        }
        CloseMenu();
        PageController.Instance.TurnPageOff(PageType.ChangeTeamMenu);
    }

    public void Resume()
    {
        PageController.Instance.TurnPageOff(PageController.Instance.activePageType);
        CloseMenu();
    }

    public void QuitToMenu()
    {
        PhotonNetwork.LeaveRoom();
        
    }

    public override void  OnLeftRoom()
    {
        Destroy(FindObjectOfType<RoomManager>().gameObject);
        PhotonNetwork.LoadLevel(0);
    }
    
}
