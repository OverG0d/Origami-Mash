using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSimpleController : MonoBehaviour
{
    public Rigidbody rb;
    public ParticleSystem ps;

    private void Start()
    {
        ps.Play();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * 10);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-Vector3.right * 10);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-Vector3.forward * 10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * 10);
        }

        if (rb.velocity.magnitude > 4)
        {
            ParticleSystem.EmissionModule psEM = ps.emission;
            psEM.enabled = true;
            
        }
        else
        {
            ParticleSystem.EmissionModule psEM = ps.emission;
            psEM.enabled = false;
        }
    }
}
