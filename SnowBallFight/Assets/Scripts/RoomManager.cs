﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    
    
    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
             GameObject playerManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PlayerManager"),Vector3.zero, Quaternion.identity);
            playerManager.GetComponent<PlayerManager>().SetUserName(PhotonNetwork.NickName);

            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefab", "Score"),Vector3.zero,Quaternion.identity);
            }
        }
    }

    
}
