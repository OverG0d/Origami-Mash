using LudumDare_Greg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelObjectPool : ObjectPool<Barrel>
{
    private void Start()
    {
       AddObjects(10);
    }
}
