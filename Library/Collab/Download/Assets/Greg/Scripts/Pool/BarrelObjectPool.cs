using LudumDare_Greg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare_Greg
{
    public class BarrelObjectPool : ObjectPool
    {
        //The singleton instance to our object pool.
        public static BarrelObjectPool PoolInstance { get; private set; }
        //specify the amount you want to put in the object pool
        protected override void InitializePool()
        {
            base.InitializePool();
            if (PoolInstance != null)
            {
                throw new System.Exception("Singleton already exists!");
            }
            else
            {
                PoolInstance = this;
            }
        }
    }
}