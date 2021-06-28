using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBarrel : MonoBehaviour
{
    public GameObject powerUp;
    public GameObject barrel;
    public SpawnPoint spawnPoint;
    protected Vector3 startPosition;
    protected Rigidbody rigid;
    public bool spawned;


    public int health;

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
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 16)
        {
            Destroy(col.gameObject);
            health--;
            rigid.AddForce(col.gameObject.transform.forward * 20f, ForceMode.Impulse);
            if (health <= 0)
            {
                Instantiate(powerUp, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
            
        if (col.gameObject.CompareTag("Seamine"))
        {
            //StartCoroutine(Spawn());
            Instantiate(powerUp, transform.position, transform.rotation);
        }

    }
}
