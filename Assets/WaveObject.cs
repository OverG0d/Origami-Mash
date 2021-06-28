using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WaveObject : NetworkBehaviour
{
    public Vector3 targetDest;
    public Vector3 dir;
    public bool toward;

    private void Start()
    {
        dir = (targetDest - transform.position).normalized;
        if (!toward)
        {
            dir *= -1;
        }
        transform.forward = dir;
    }

    private void FixedUpdate()
    {
        Debug.Log(toward);
        if (toward)
        {
            if (Mathf.Abs(Vector3.Distance(targetDest, transform.position)) > 5)
            {
                //Debug.Log(Mathf.Abs(Vector3.Distance(targetDest, transform.position)));
                transform.position += dir/5;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (Mathf.Abs(Vector3.Distance(targetDest, transform.position)) < 60)
            {
                //Debug.Log(Mathf.Abs(Vector3.Distance(targetDest, transform.position)));
                transform.position += dir/5;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<Rigidbody>().velocity += transform.forward * 2;
        }
    }
}
