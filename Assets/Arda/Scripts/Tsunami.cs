using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Tsunami : NetworkBehaviour
{
    Rigidbody rigid;
    public float speed;
    public int attackDamage;

    public override void OnStartServer()
    {
        ProgressManager.instance.tsunamis.Add(gameObject);
    }

    public override void OnStartClient()
    {
        ProgressManager.instance.tsunamis.Add(gameObject);
    }

    private void Start()
    {     
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(Destroy());
    }

    void FixedUpdate()
    {
        rigid.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    [ServerCallback]
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Seamine"))
        {
            RpcWave(col.gameObject);
           
        }

        if (col.gameObject.CompareTag("Barrel"))
        {
            RpcWave(col.gameObject);
        }

        if (col.gameObject.CompareTag("Lilypad"))
        {
            RpcWave(col.gameObject);
        }

        if (col.gameObject.layer == 11)
        {
            RpcWave(col.gameObject);
        }

        if (col.gameObject.layer == 16)
        {
            NetworkServer.Destroy(col.gameObject);
        }

        if (col.gameObject.layer == 21)
        {
            NetworkServer.Destroy(col.gameObject);
        }
    }

    [ClientRpc]
    void RpcWave(GameObject col)
    {
        col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        NetworkServer.Destroy(gameObject);
    }
}
