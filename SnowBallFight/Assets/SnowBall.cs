using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SnowBall : MonoBehaviour
{
    public TeamType teamType;


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

    private void Update()
    {
        if(transform.position.y < -10)
        {
            DestroySnowball();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            PlayerController _player = collision.gameObject.GetComponent<PlayerController>();
            
            if (_player.GetTeam() != teamType)
            {
                _player.getHit();
            }
            
        }

        DestroySnowball();



    }

    private void DestroySnowball()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
