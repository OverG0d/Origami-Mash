using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSeamine : MonoBehaviour
{
    public GameObject barrel;
    public GameObject id;

    public bool spawn;
    public int speed;
    public int attackDamage;
    public GameObject explosion;

    public SpawnPoint spawnPoint;
    protected Vector3 startPosition;
    protected Rigidbody rigid;
    public bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if (!spawned)
        {
            startPosition = transform.position;
            spawned = true;
        }
    }

    void FixedUpdate()
    {
        if (spawn)
            rigid.AddForce(transform.forward * speed);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Seamine"))
        {
            Explode();
        }

        if (col.gameObject.layer == 11 || col.gameObject.layer == 12 || col.gameObject.layer == 18)
        {
            Explode();
        }

        if (col.gameObject.layer == 16)
        {
            if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_DESTROYMINES)
            {
                TutorialEventManager.instance.CheckTutorialConditions();
            }
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            Explode();
        }

    }

    void Explode()
    {
        if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_DESTROYMINES)
        {
            TutorialEventManager.instance.CheckTutorialConditions();
        }
        GameObject obj = Instantiate(explosion, transform.position += new Vector3(0, 60, 0), transform.rotation);
        ParticleSystem system = obj.GetComponent<ParticleSystem>();
        system.Emit(new ParticleSystem.EmitParams(), 10);
        system.Play();
        ExplosionDamage(transform.position, 15);
        gameObject.SetActive(false);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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
                if (hitCollider.gameObject.layer == 11 && hitCollider.gameObject != gameObject)
                {
                    hitCollider.GetComponent<TutorialShip>().Hit(20);
                }
                hitCollider.GetComponent<Rigidbody>().AddForce(-gameObject.transform.forward * 20, ForceMode.Impulse);
            }
        }
    }
}
