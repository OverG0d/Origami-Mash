using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Lilypad : NetworkBehaviour
{

    public Vector3 startPosition;

    public override void OnStartServer()
    {
        startPosition = transform.position;
        ProgressManager.instance.levelObjects.Add(gameObject);
    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            startPosition = transform.position;
            ProgressManager.instance.levelObjects.Add(gameObject);
        }          
    }

    void Start()
    {
        if (isServer)   // isLocalPlayer if you're doing this on players
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
