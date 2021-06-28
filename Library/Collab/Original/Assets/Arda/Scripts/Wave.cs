using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Wave : NetworkBehaviour
{
    Rigidbody rigid;
    public float speed;
    public GameObject id;
    public int attackDamage;
    public ParticleSystem[] particles;
    public GameObject waveModel;
    public GameObject explosion;
    public bool rigidDisabled;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (!rigidDisabled)
        {
            rigid.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }
        else
        {
            Debug.Log("FALSSSEEE");
            bool canDestroy = true;
            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.EmissionModule psEM = ps.emission;

                if (psEM.enabled || ps.particleCount > 0)
                {
                    canDestroy = false;
                    break;
                }
            }
            if (canDestroy)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    public void RotateTowards(Vector3 playerPos)
    {
        Quaternion targetRotation;
        Vector3 targetPoint;
        targetPoint = playerPos - transform.position;
        targetRotation = Quaternion.LookRotation(targetPoint);
        transform.rotation = targetRotation;
    }

    public void RotateOutwards(Vector3 playerPos)
    {
        Quaternion targetRotation;
        Vector3 targetPoint;
        targetPoint = playerPos - transform.position;
        targetRotation = Quaternion.LookRotation(-targetPoint);
        transform.rotation = targetRotation;
    }

    [ServerCallback]
    public void OnCollisionEnter(Collision col)
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * 10);

        if (col.gameObject.CompareTag("Seamine"))
        {
            RpcId(col.gameObject);
            RpcWave(col.gameObject);

            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.EmissionModule psEM = ps.emission;
                psEM.enabled = false;
                waveModel.SetActive(false);
            }
            GetComponent<Collider>().enabled = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            rigidDisabled = true;
            explosion.SetActive(true);
        }

        if (col.gameObject.CompareTag("Barrel"))
        {
            RpcWave(col.gameObject);

            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.EmissionModule psEM = ps.emission;
                psEM.enabled = false;
                waveModel.SetActive(false);
            }
            GetComponent<Collider>().enabled = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            rigidDisabled = true;
        }

        if (col.gameObject.CompareTag("Lilypad"))
        {
            RpcWave(col.gameObject);

            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.EmissionModule psEM = ps.emission;
                psEM.enabled = false;
                waveModel.SetActive(false);
            }
            GetComponent<Collider>().enabled = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            rigidDisabled = true;
            explosion.SetActive(true);
        }

        if (col.gameObject.layer == 11)
        {
            RpcWavePlayer(col.gameObject);

            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.EmissionModule psEM = ps.emission;
                psEM.enabled = false;
                waveModel.SetActive(false);
            }
            GetComponent<Collider>().enabled = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            rigidDisabled = true;
            explosion.SetActive(true);
        }

        if (col.gameObject.layer == 12 || col.gameObject.layer == 18)
        {
            foreach (Collider hitcol in hitColliders)
            {
                if (hitcol.gameObject.layer == 11) 
                {
                    Vector3 impactPoint = new Vector3(transform.position.x, hitcol.transform.position.y, transform.position.z);
                    hitcol.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1200.0f, impactPoint, 60, 0);
                    RpcWavePlayer(hitcol.gameObject);
                }
            }

            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.EmissionModule psEM = ps.emission;
                psEM.enabled = false;
                waveModel.SetActive(false);
            }
            GetComponent<Collider>().enabled = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            rigidDisabled = true;
            explosion.SetActive(true);
        }

        if (col.gameObject.layer == 16)
        {

            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.EmissionModule psEM = ps.emission;
                psEM.enabled = false;
                waveModel.SetActive(false);
            }
            GetComponent<Collider>().enabled = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            rigidDisabled = true;
            explosion.SetActive(true);
        }
    }


    [ServerCallback]
    public void OnTriggerEnter(Collider col)
    {
        //if (col.gameObject.layer == 18)
        //{
        //    Destroy(gameObject);
        //}
        
    }

    [ClientRpc]
    void RpcWave(GameObject col)
    {
        col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
    }

    [ClientRpc]
    void RpcWavePlayer(GameObject col)
    {
        col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
    }

    [ClientRpc]
    void RpcId(GameObject col)
    {
        col.gameObject.GetComponent<Seamine>().id = id;
    }
}
