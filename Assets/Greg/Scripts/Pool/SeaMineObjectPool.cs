using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace LudumDare_Greg
{
    public class SeaMineObjectPool : NetworkBehaviour
    {
        //The singleton instance to our object pool.
        public static SeaMineObjectPool PoolInstance { get; private set; }

        //the list of objects
        private static Queue<SpawnableObject> objpool;

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
            SpawnableObject objToGet = objpool.Peek();
            if (!objToGet.gameObject.activeSelf)
            {
                objToGet = objpool.Dequeue();
                objToGet.gameObject.SetActive(true);
                objToGet.transform.position = new Vector3(Random.Range(-170.0f, 170.0f), 0.7f, Random.Range(-170.0f, 170.0f));
                return objToGet;
            }
            return null;
        }
    }
}