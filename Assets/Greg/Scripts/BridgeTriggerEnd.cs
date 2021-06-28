using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTriggerEnd : MonoBehaviour
{
    private bool movedToPlatform;
    public TutorialWall door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            movedToPlatform = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_MOVETOENEMYSHIP && movedToPlatform)
        {
            TutorialEventManager.instance.CheckTutorialConditions();
            door.OpenDoor();
            Destroy(gameObject);
        }
    }
}
