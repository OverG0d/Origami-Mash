using LudumDare_Greg;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RespawnManager : NetworkBehaviour
{
    public static RespawnManager respawnManager;

    [SerializeField]
    private List<SpawnPoint> spawnpoints;

    private float respawnCounter;

    private void Awake()
    {
        spawnpoints = new List<SpawnPoint>();
        respawnManager = this;
    }

    private void Start()
    {
        foreach (SpawnPoint sp in spawnpoints)
        {
            Debug.Log(sp.transform.position);
        }
    }

    public void AddSpawn(SpawnPoint spawnPoint)
    {
        spawnpoints.Add(spawnPoint);
    }

    [ServerCallback]
    public void RespawnObject()
    {
        if (isServer)
        {
            RpcRespawnObject();
        }
        else
        {
            CmdRespawnObject();
        }
    }

    [Command]
    public void CmdRespawnObject() 
    {
        RpcRespawnObject();
    }

    [ClientRpc]
    public void RpcRespawnObject()
    {
        //ideally, the counter should only start when a spawnpoint is available
        //problem is if respawncounter is 4.9 secs, then it will still pass thru
        if (respawnCounter > 5)
        {
            respawnCounter = 0;

            foreach (SpawnPoint sp in spawnpoints)
            {
                foreach (GameObject playerG in ProgressManager.instance.players)
                {
                    //if (!sp.Occupied && !sp.IsInSpawn(playerG.transform) && sp.spawnTimer > sp.respawnCooldown)
                    //{
                    //    //GameObject ourGameObj = sp.GetFromPool();
                    //    //// tell server to send SpawnMessage, which will call SpawnHandler on client
                    //    //NetworkServer.Spawn(ourGameObj);
                    //    //ourGameObj.GetComponent<SpawnableObject>().spawnPoint = sp;
                    //    //ourGameObj.transform.position = sp.transform.position;
                    //    //sp.Occupied = true;
                    //    return;
                    //}

                }
            }
        }
    }

    private void Update()
    {
        foreach (SpawnPoint sp in spawnpoints)
        {
            sp.spawnTimer += Time.deltaTime;
        }
        respawnCounter += Time.deltaTime;
        RespawnObject();
    }
}