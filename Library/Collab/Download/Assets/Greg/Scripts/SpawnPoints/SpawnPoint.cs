using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudumDare_Greg;

[System.Serializable]
public abstract class SpawnPoint: MonoBehaviour
{
    //how big the range of the spawn protection is   
    public int spawnProtectionRadius;

    public int respawnCooldown;

    public float spawnTimer;
    //public bool Occupied { get; set; }

    //public bool IsInSpawn(Transform target) 
    //{
    //    Debug.Log((target.position - transform.position).magnitude);
    //    if ((target.position - transform.position).magnitude <= spawnProtectionRadius)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

}