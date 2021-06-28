using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExplosion : MonoBehaviour
{
    public ParticleSystem explosionPS;

    private void Start()
    {
        //ParticleSystem.EmissionModule psEM = explosionPS.emission;
        //psEM.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ParticleSystem.EmissionModule psEM = explosionPS.emission;
        if (explosionPS.particleCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
