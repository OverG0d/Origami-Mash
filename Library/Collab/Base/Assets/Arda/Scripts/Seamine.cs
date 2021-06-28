
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using IEntity;

public class Seamine : SpawnableObject
{
    public GameObject barrel;
    public GameObject id;

    public bool spawn;
    public int speed;
    public int attackDamage;
    public GameObject explosion;

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
            rotation = transform.rotation;
            spawned = true;
        }
    }

    public override void OnStartClient()
    {
        if(!isServer)
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

    void FixedUpdate()
    {
        if(spawn)
        rigid.AddForce(transform.forward * speed);
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Lilypad"))
        {
            GameObject obj = Instantiate(explosion, gameObject.transform.position + new Vector3(0, 20, 0), gameObject.transform.rotation);
            NetworkServer.Spawn(obj);
            RpcExplosion();
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }

        if (col.gameObject.layer == 11)
        {
            GameObject obj = Instantiate(explosion, gameObject.transform.position + new Vector3(0, 20, 0), gameObject.transform.rotation);
            NetworkServer.Spawn(obj);        
            RpcExplosion();
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }

        if (col.gameObject.layer == 12)
        {
            GameObject obj = Instantiate(explosion, gameObject.transform.position + new Vector3(0, 20, 0), gameObject.transform.rotation);
            NetworkServer.Spawn(obj);        
            RpcExplosion();
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }

        if (col.gameObject.layer == 18)
        {
            GameObject obj = Instantiate(explosion, gameObject.transform.position + new Vector3(0, 20, 0), gameObject.transform.rotation);
            NetworkServer.Spawn(obj);
            RpcExplosion();
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }
    }

    [ClientRpc]
    void RpcExplosion()
    {
        ExplosionDamage(transform.position, 15);
    }

    public void RotateOutwards(Vector3 playerPos)
    {
        Quaternion targetRotation;
        Vector3 targetPoint;
        targetPoint = playerPos - transform.position;
        targetRotation = Quaternion.LookRotation(-targetPoint);
        transform.rotation = targetRotation;
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Rigidbody>() != null)
            {
                if(hitCollider.GetComponent<Player>() != null && hitCollider.gameObject != gameObject)
                {
                    hitCollider.GetComponent<Player>().playerHealth -= 10;
                }
                hitCollider.GetComponent<Rigidbody>().AddForce(-gameObject.transform.forward * 20, ForceMode.Impulse);
            }
        }
    }
}
