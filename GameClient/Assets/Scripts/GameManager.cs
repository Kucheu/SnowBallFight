using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, SnowBallManager> snowBalls = new Dictionary<int, SnowBallManager>();

    public GameObject localPlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject snowBallPrefab;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Debug.Log("Instance already exist, destroing object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _postion, Quaternion _rotation)
    {
        GameObject _player;
        if(_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _postion, _rotation);
        }
        else
        {
            _player = Instantiate(PlayerPrefab, _postion, _rotation);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnSnowBall(int _id, Vector3 _postion, Quaternion _rotation)
    {
        GameObject _snowBall;
        _snowBall = Instantiate(snowBallPrefab, _postion, _rotation);

        _snowBall.GetComponent<SnowBallManager>().Initialize(_id);
        snowBalls.Add(_id, _snowBall.GetComponent<SnowBallManager>());
    }
}
