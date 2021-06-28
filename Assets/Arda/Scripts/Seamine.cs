
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
            GameObject explosionObj = Instantiate(explosion);
            explosionObj.transform.position = gameObject.transform.position + new Vector3(0, 20, 0);
            NetworkServer.Spawn(explosionObj);
            //RpcExplosion();
            ExplosionDamage(transform.position, 15);
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }

        if (col.gameObject.layer == 11)
        {
            GameObject explosionObj = Instantiate(explosion);
            explosionObj.transform.position = gameObject.transform.position + new Vector3(0, 20, 0);
            NetworkServer.Spawn(explosionObj);
            //RpcExplosion();
            ExplosionDamage(transform.position, 15);
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }

        if (col.gameObject.layer == 12)
        {
            GameObject explosionObj = Instantiate(explosion);
            explosionObj.transform.position = gameObject.transform.position + new Vector3(0, 20, 0);
            NetworkServer.Spawn(explosionObj);
            //RpcExplosion();
            ExplosionDamage(transform.position, 15);
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }

        if (col.gameObject.layer == 18)
        {
            GameObject explosionObj = Instantiate(explosion);
            explosionObj.transform.position = gameObject.transform.position + new Vector3(0, 20, 0);
            NetworkServer.Spawn(explosionObj);
            //RpcExplosion();
            ExplosionDamage(transform.position, 15);
            RespawnManager.instance.RpcDeSpawn(gameObject, startPosition);
        }
    }

    /*[ClientRpc]
    void RpcExplosion()
    {
        ExplosionDamage(transform.position, 15);
    }*/

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
            if (hitCollider.gameObject == gameObject) continue;

            Rigidbody hitRigidbody = hitCollider.GetComponent<Rigidbody>();
            if (hitRigidbody == null) continue;

            Player hitPlayer = hitCollider.GetComponent<Player>();

            if (hitPlayer != null)
            {
                hitPlayer.playerHealth -= 10;
                RpcDamage(hitCollider.gameObject);
            }
            hitRigidbody.AddForce(-transform.forward * 20, ForceMode.Impulse);
        }
    }

    [ClientRpc]
    void RpcDamage(GameObject obj)
    {
        if (id != null)
        {
            EventManager.instance.PlayerDeathNotif(obj, id);
            EventManager.instance.PlayerKillNotif(id, obj);
            EventManager.instance.RemovePlayerCheckWinner(obj);
            ProgressManager.instance.CheckPlayerAlive(obj);
        }
        else
        {
            EventManager.instance.RemovePlayerCheckWinner(obj);
            ProgressManager.instance.CheckPlayerAlive(obj);
        }
    }

}
