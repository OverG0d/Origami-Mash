using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWave : MonoBehaviour
{
    Rigidbody rigid;
    public float speed;
    public TutorialShip source;
    public int attackDamage;
    public bool isOutward;
    public ParticleSystem[] particles;
    public GameObject waveModel;
    public GameObject explosionPrefab;
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
                Destroy(gameObject);
            }
        }
    }

    public void RotateTowards(Vector3 playerPos)
    {
        isOutward = false;
        Quaternion targetRotation;
        Vector3 targetPoint;
        targetPoint = playerPos - transform.position;
        targetRotation = Quaternion.LookRotation(targetPoint);
        transform.rotation = targetRotation;
    }

    public void RotateOutwards(Vector3 playerPos)
    {
        isOutward = true;
        Quaternion targetRotation;
        Vector3 targetPoint;
        targetPoint = playerPos - transform.position;
        targetRotation = Quaternion.LookRotation(-targetPoint);
        transform.rotation = targetRotation;
    }

    public void OnCollisionEnter(Collision col)
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale * 10);

        if (col.gameObject.CompareTag("Seamine"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);

            ParticleTransition();
        }

        if (col.gameObject.CompareTag("Barrel"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);

            ParticleTransition();
        }

        if (col.gameObject.CompareTag("Lilypad"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);

            ParticleTransition();
        }

        if (col.gameObject.layer == 11)
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);

            ParticleTransition();
        }

        if (col.gameObject.layer == 12)
        {
            foreach (Collider hitcol in hitColliders)
            {
                if (hitcol.gameObject.layer == 11)
                {
                    Vector3 impactPoint = new Vector3(transform.position.x, hitcol.transform.position.y, transform.position.z);
                    hitcol.gameObject.GetComponent<Rigidbody>().AddExplosionForce(20f, impactPoint, 30, 0);
                    col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                }
            }

            ParticleTransition();
        }

        if (col.gameObject.layer == 18)
        {
            foreach (Collider hitcol in hitColliders)
            {
                if (hitcol.gameObject.layer == 11)
                {
                    Vector3 impactPoint = new Vector3(transform.position.x, hitcol.transform.position.y, transform.position.z);
                    hitcol.gameObject.GetComponent<Rigidbody>().AddExplosionForce(20f, impactPoint, 30, 0);
                }
            }

            ParticleTransition();
        }

        if (col.gameObject.layer == 16)
        {
            ParticleTransition();
        }
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 18)
        {
            Destroy(gameObject);
        }
    }

    void ParticleTransition()
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
        GameObject explosionCopy = Instantiate(explosionPrefab, transform.position += new Vector3(0, 60 ,0), Quaternion.identity);
        explosionCopy.transform.SetParent(transform);
    }
}
