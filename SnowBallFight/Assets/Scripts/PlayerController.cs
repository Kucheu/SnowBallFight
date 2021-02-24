using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cameraholder, snowBallholder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime, throwForce;
    
    [SerializeField] TMP_Text userName;

    float verticalLookRoattion;
    public bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    PlayerManager playerManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    public void Start()
    {
        if(!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
        else
        {
            //Destroy(userName);
        }
        userName.text = playerManager.userName;
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
            Move();
            Jump();
            Shoot();
        }
        
        

        if(transform.position.y < 10)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        }
        
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject _snowBall;
            if (playerManager.teamType == TeamType.BlueTeam)
            {
                _snowBall = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "BlueSnowball"), snowBallholder.transform.position, snowBallholder.transform.rotation);
                
            }
            else
            {
                _snowBall = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "RedSnowball"), snowBallholder.transform.position, snowBallholder.transform.rotation);
            }
            
            Rigidbody _snowBallRb = _snowBall.GetComponent<Rigidbody>();
            _snowBallRb.AddForce(cameraholder.transform.forward * throwForce);
        }
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRoattion += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRoattion = Mathf.Clamp(verticalLookRoattion, -90f, 90f);

        cameraholder.transform.localEulerAngles = Vector3.left * verticalLookRoattion; 
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    public void getHit()
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All);
    }

    public TeamType GetTeam()
    {
        return playerManager.teamType;
    }

    [PunRPC]
    void RPC_TakeDamage()
    {
        if (!PV.IsMine)
            return;

        Die();
    }

    void Die()
    {
        playerManager.Die();
    }
}
