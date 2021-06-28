using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class SpawnableObject : NetworkBehaviour
{
    public SpawnPoint spawnPoint;
    public Vector3 startPosition;
    public Quaternion rotation;
    public Rigidbody rigid;
    public bool spawned;

}
