using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Powerup : NetworkBehaviour
{

    void Start()
    {
        StartCoroutine(Destroy());
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 11)
        {
            NetworkServer.Destroy(gameObject);
        }    
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        NetworkServer.Destroy(gameObject);
    }

}
