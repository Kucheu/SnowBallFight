using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    [SerializeField]
    GameObject scoreTemplate,title, blueSide, redSite, scoreboard;

    PlayerManager[] Players;



    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            ShowScoreboard();
        }
        else
        {
            scoreboard.SetActive(false);
        }
    }

    void ShowScoreboard()
    {
        //Clean currentScoreboard
        foreach (Transform child in blueSide.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in redSite.transform)
        {
            Destroy(child.gameObject);
        }



        //Add title
        GameObject _title;
        _title = Instantiate(title, transform);
        _title.transform.SetParent(redSite.transform);
        _title = Instantiate(title, transform);
        _title.transform.SetParent(blueSide.transform);



        //Show new Score
        Players = FindObjectsOfType<PlayerManager>();

        foreach (PlayerManager player in Players)
        {
            GameObject _player = Instantiate(scoreTemplate);
            _player.transform.GetChild(0).GetComponent<TMP_Text>().text = player.userName;
            _player.transform.GetChild(1).GetComponent<TMP_Text>().text = player.kills.ToString();
            _player.transform.GetChild(2).GetComponent<TMP_Text>().text = player.deaths.ToString();

            if (player.teamType == TeamType.BlueTeam)
            {
                _player.transform.SetParent(blueSide.transform);
            }
            else
            {
                _player.transform.SetParent(redSite.transform);
            }

            _player.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
