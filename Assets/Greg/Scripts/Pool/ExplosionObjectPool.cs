using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudumDare_Greg;
using Mirror;
public class ExplosionObjectPool : NetworkBehaviour
{
    //The singleton instance to our object pool.
    public static ExplosionObjectPool PoolInstance { get; private set; }

    //the list of objects
    public static Queue<SpawnableObject> objpool;

    private void Awake()
    {
        objpool = new Queue<SpawnableObject>();
    }

    //specify the amount you want to put in the object pool
    private void Start()
    {
        if (PoolInstance != null)
        {
            throw new System.Exception("Singleton already exists!");
        }
        else
        {
            PoolInstance = this;
        }
    }

    public void AddToPool(SpawnableObject objToAdd)
    {
        //objToAdd.gameObject.SetActive(false);
        objpool.Enqueue(objToAdd);
    }

    public SpawnableObject GetFromPool()
    {
        SpawnableObject objToGet = objpool.Dequeue();
        objToGet.gameObject.SetActive(true);
        return objToGet;

    }
}
