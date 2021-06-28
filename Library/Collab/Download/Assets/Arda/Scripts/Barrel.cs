using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Barrel : SpawnableObject
{
    public GameObject powerUp;
    public GameObject barrel;

    [SyncVar]
    public int health;

    public override void OnStartServer()
    {
        ProgressManager.instance.levelObjects.Add(gameObject);
        spawned = false;
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero; 
        if (!spawned)
        {
            startPosition = transform.position;
            spawned = true;
        }
    }

    public override void OnStartClient()
    {
        if (!isServer)
            ProgressManager.instance.levelObjects.Add(gameObject);
        spawned = false;
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        if (!spawned)
        {
            startPosition = transform.position;
            rotation = transform.rotation;
            spawned = true;
        }
    }

    void Start()
    {
        if (isServer)   // isLocalPlayer if you're doing this on players
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 16)
        {
            health--;
            if (health <= 0)
            { 
                RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
                NetworkServer.Spawn(Instantiate(powerUp, transform.position, transform.rotation));
            }
            rigid.AddForce(col.gameObject.transform.forward * 20f, ForceMode.Impulse);
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Seamine"))
        {
            //StartCoroutine(Spawn());
            NetworkServer.Spawn(Instantiate(powerUp, transform.position, transform.rotation));
        }

    }
}
