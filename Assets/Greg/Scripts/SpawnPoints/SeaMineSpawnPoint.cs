using LudumDare_Greg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace LudumDare_Greg
{
    public class SeaMineSpawnPoint : NetworkBehaviour
    {

        public float respawncounter;

        private void Update()
        {
            respawncounter += Time.deltaTime;
            if (respawncounter > 5)
            {
                //SpawnOne();
                respawncounter = 0;
            }
        }

        public void SpawnOne()
        {
            SpawnableObject objToSpawn = SeaMineObjectPool.PoolInstance.GetFromPool();
            if (objToSpawn != null)
            {
                objToSpawn.gameObject.SetActive(true);
                objToSpawn.transform.position = new Vector3(Random.Range(-170.0f, 170.0f), 0.7f, Random.Range(-170.0f, 170.0f));
            }
        }
    }
}
