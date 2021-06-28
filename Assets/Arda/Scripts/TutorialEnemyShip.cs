using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyShip : TutorialShip
{
    public Transform target;

    protected override void InitShip()
    {
        startPlayerHealth = 100;
        base.InitShip();
    }

    protected override void DestroyShip()
    {
        if (TutorialEventManager.TutorialCount.HINT_FIGHTENEMYSHIP == TutorialEventManager.instance.currentHint)
        {
            TutorialEventManager.instance.CheckTutorialConditions();
        }

        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();

        if (attackCounterOut > attackCooldownOut)
        {
            hasFiredOut = false;
        }

        if (target != null && !hasFiredOut)
        {
            transform.LookAt(target.position);
            SpawnWaveOutwards(transform.position + transform.forward * 5, 1);
            attackCounterOut = 0;
            hasFiredOut = true;
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Seamine"))
        {
            Hit(col.gameObject.GetComponent<TutorialSeamine>().attackDamage);
        }

        if (col.gameObject.layer == 16)
        {
            Hit(col.gameObject.GetComponent<TutorialWave>().attackDamage);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 11)
        {
            target = col.transform;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == 11)
        {
            target = null;
        }
    }
}
