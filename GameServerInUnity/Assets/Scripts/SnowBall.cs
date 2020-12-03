using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    public int id;
    public float thrust = 2f;
    public Rigidbody rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * thrust);
    }

    public void Initialize(int _id)
    {
        id = _id;
    }



    public void FixedUpdate()
    {
        ServerSend.SnowBallPosition(this);
        //send position
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Server.snowBalls.Remove(id);
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(100f);
           // collision.gameObject.GetComponent<Player>().TakeDamage(10f); //for test

        }
        ServerSend.SnowBallDestroy(this);
    }
}
