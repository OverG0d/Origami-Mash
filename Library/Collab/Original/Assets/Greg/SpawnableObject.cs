using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace LudumDare_Greg
{

    public abstract class SpawnableObject : NetworkBehaviour
    {
        public SpawnPoint<Component> spawnPoint;
        protected Vector3 startPosition;
        protected Rigidbody rigid;
        public bool spawned;

        public abstract void RpcSpawn();
        public abstract void RpcDeSpawn();
    }
}
