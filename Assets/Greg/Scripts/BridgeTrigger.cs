using UnityEngine;

public class BridgeTrigger : MonoBehaviour
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

    private void Update()
    {
        if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_MOVETOMINES && movedToPlatform)
        { 
            TutorialEventManager.instance.CheckTutorialConditions();
            door.OpenDoor();
            Destroy(gameObject);
        }
    }
}
