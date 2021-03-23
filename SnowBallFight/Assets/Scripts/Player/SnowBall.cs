using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SnowBall : MonoBehaviour
{
    public TeamType teamType;
    public PlayerManager snowballOwner;
    public AudioClip audioDestroySnowball;


    PhotonView PV;
    Rigidbody rb;
    Score score;
    AudioSource audioSource;
    MeshRenderer meshRenderer;
    Collider SnowballCollider;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        score = FindObjectOfType<Score>();
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
        SnowballCollider = GetComponent<Collider>();
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
            StartCoroutine(DestroySnowball());
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
                PV.RPC("KillInfo", RpcTarget.All, snowballOwner.userName, _player.playerManager.userName);
            }
            
        }

        StartCoroutine(DestroySnowball());



    }

    private IEnumerator DestroySnowball()
    {
        meshRenderer.enabled = false;
        SnowballCollider.enabled = false;
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioDestroySnowball);
        }
        yield return new WaitForSeconds(audioDestroySnowball.length);

        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void KillInfo(string whoKill, string whoDead)
    {
        FindObjectOfType<KillFeed>().SendKillInfo(whoDead, "snowball", whoKill);
    }
}
