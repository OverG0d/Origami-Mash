using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWall : MonoBehaviour
{
    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }
    IEnumerator OpenDoorCoroutine()
    {
        while (transform.localScale.z > 0)
        {
            transform.localScale += new Vector3(0, 0, -0.01f);
            yield return null;
        }
    }
}
