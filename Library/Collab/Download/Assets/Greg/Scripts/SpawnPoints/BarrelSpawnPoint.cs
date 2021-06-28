using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LudumDare_Greg
{
    public class BarrelSpawnPoint : SpawnPoint
    {
        private void Start()
        {
            RespawnManager.respawnManager.AddSpawn(this);
        }

    }
}