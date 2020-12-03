using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallManager : MonoBehaviour
{
    public int id;
    public Rigidbody rb;
    public float thrust = 1000f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * thrust);
    }

    public void Initialize(int _id)
    {
        id = _id;
    }

    private void FixedUpdate()
    {
        if(transform.position.y < -500f)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
        GameManager.snowBalls.Remove(id);
    }
    
}
