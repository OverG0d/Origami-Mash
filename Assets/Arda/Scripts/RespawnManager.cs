using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RespawnManager : NetworkBehaviour
{
    public IEnumerator coroutine;
    public static RespawnManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [ClientRpc]
    public void RpcDeSpawn(GameObject obj, Vector3 spawnPos)
    {
        obj.SetActive(false);
        obj.transform.position = spawnPos;
        if(obj.GetComponent<Seamine>() != null)
        {
            obj.GetComponent<Seamine>().id = null;
        }
        StartCoroutine(Spawn(obj));
    }

    IEnumerator Spawn(GameObject obj)
    {
        yield return new WaitForSeconds(8f);
        bool allplayersnotaround = true;
        foreach (GameObject player in ProgressManager.instance.alivePlayers)
        {
            if (Mathf.Abs(player.transform.position.magnitude - obj.transform.position.magnitude) < 10)
            {
                allplayersnotaround = false;
                break;
            }
        }

        if (allplayersnotaround)
        {
            obj.SetActive(true);
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        yield return StartCoroutine(Spawn(obj));

    }
}

