using LudumDare_Greg;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{

    public static RespawnManager respawnManager;

    [SerializeField]
    private List<SpawnPoint<Component>> spawnpoints;

    private float respawnCounter;

    private void Awake()
    {
        spawnpoints = new List<SpawnPoint<Component>>();
        respawnManager = this;
    }

    private void Start()
    {
        foreach (SpawnPoint<Component> sp in spawnpoints)
        {
            Debug.Log(sp.transform.position);
        }
    }

  

    public void AddSpawn(SpawnPoint<Component> spawnPoint)
    {
        spawnpoints.Add(spawnPoint);
    }

    public void RespawnObject()
    {
        //ideally, the counter should only start when a spawnpoint is available
        //problem is if respawncounter is 4.9 secs, then it will still pass thru
        if (respawnCounter > 5)
        {
            respawnCounter = 0;

            foreach (SpawnPoint<Component> sp in spawnpoints)
            {
                foreach (GameObject playerG in ProgressManager.instance.players)
                {
                    if (!sp.Occupied && !sp.IsInSpawn(playerG.transform) && sp.spawnTimer > sp.respawnCooldown)
                    {
                        Component ourGameObj = sp.Get();
                        ((SpawnableObject) ourGameObj).spawnPoint = sp;
                        NetworkServer.Spawn(ourGameObj.gameObject);
                        ourGameObj.transform.position = sp.transform.position;
                        ourGameObj.gameObject.SetActive(true);
                        sp.Occupied = true;
                        return;
                    }
                }
            }
        }

    }

    private void Update()
    {
        foreach (SpawnPoint<Component> sp in spawnpoints)
        {
            sp.spawnTimer += Time.deltaTime;
        }
        respawnCounter += Time.deltaTime;
        RespawnObject();
    }

}