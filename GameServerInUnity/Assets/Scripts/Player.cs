using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public Transform shootOrigin;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public float health;
    public float maxHealth = 100f;
    public Quaternion look = new Quaternion();

    private bool[] inputs;
    private float yVelocity = 0;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if(health <= 0)
        {
            return;
        }
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }

        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection)
    {

        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if(controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        //ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void SetLook(Quaternion _look)
    {
        look = _look;
    }

    public void Shoot(Vector3 _viewDirection)
    {
        /*
        if(Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f))
        {
            if(_hit.collider.CompareTag("Player"))
            {
                _hit.collider.GetComponent<Player>().TakeDamage(50f);
            }
        }*/

        //Check if ready
        SnowBall snowball;
        Quaternion _shootDirection = transform.rotation * look ;
        snowball = NetworkManager.instance.InstantiateSnowBall(shootOrigin.position, _shootDirection);
        for(int i = 0; i < 300; i++)
        {
            if(!Server.snowBalls.ContainsKey(i))
            {
                snowball.Initialize(i);
                Server.snowBalls.Add(i, snowball);
                ServerSend.SpawnSnowBall(snowball);
                return;
            }
        }
        Debug.Log("Max amount of snowBall");
        
    }

    public void TakeDamage(float _damage)
    {
        if(health <= 0f)
        {
            return;
        }

        health -= _damage;
        if(health <= 0f)
        {
            health = 0;
            controller.enabled = false;
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        transform.position = new Vector3(40f, 0.5f, 50f);
        ServerSend.PlayerPosition(this);

        health = maxHealth;

        controller.enabled = true;
        ServerSend.PlayerRespawned(this);
        
    }
}
