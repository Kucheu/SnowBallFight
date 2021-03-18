using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SnowBall : MonoBehaviour
{
    public TeamType teamType;
    public PlayerManager snowballOwner;

    PhotonView PV;
    Rigidbody rb;
    Score score;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        score = FindObjectOfType<Score>();
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(rb);
        }

        object[] data = PV.InstantiationData;
        int snowballOwnerID = (int)data[0];
        snowballOwner = PhotonView.Find(snowballOwnerID).gameObject.GetComponent<PlayerManager>();
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
                score.AddPoint(teamType);
                snowballOwner.GetKill();
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
