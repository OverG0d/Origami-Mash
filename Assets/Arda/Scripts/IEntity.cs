using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IEntity
{
    public interface ISpawnable<T>
    {
        void Spawn(T spawnPoint);
    }

    public interface IDestroyable
    {
        void Destroy();
    }

    public interface IDamagable<T>
    {
        void TakenDamage(T damageTaken);
    }
}


