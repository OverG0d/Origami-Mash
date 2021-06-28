using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PS : NetworkBehaviour
{

    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1f);
        NetworkServer.Destroy(gameObject);
    }
}
