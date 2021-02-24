using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTeamMenu : MonoBehaviour
{
    public static ChangeTeamMenu Instance;
    PlayerManager playerManager;

    [SerializeField] GameObject changeTeamMenu, firstCamera;

    bool firstChangeTeam = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(changeTeamMenu.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void FirstOpen(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
        firstCamera.SetActive(true);
        OpenMenu();
    }

   public void OpenMenu()
   {
        changeTeamMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void changeTeam(string _teamType)
    {
        
        firstChangeTeam = true;
        switch(_teamType)
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
        CloseMenu();
    }
    public void CloseMenu()
    {
        if(!firstChangeTeam)
        {
            playerManager.ChangeTeam(TeamType.BlueTeam);
        }
        changeTeamMenu.SetActive(false);
        firstCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
}
