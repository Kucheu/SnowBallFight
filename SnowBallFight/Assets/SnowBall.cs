using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SnowBall : MonoBehaviour
{
    TeamType teamType;


    PhotonView PV;
    Rigidbody rb;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(rb);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            PlayerController _player = collision.gameObject.GetComponent<PlayerController>();
            _player.getHit();
            if (_player.teamType != teamType)
            {
                
            }
            
        }

        if(PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        

    }
}
