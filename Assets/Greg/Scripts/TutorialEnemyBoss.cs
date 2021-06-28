using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyBoss : TutorialEnemyShip
{
    protected override void DestroyShip()
    {
        if (TutorialEventManager.TutorialCount.HINT_FIGHTBOSSSHIP == TutorialEventManager.instance.currentHint)
        {
            TutorialEventManager.instance.CheckTutorialConditions();
        }

        Destroy(gameObject);
    }
}
