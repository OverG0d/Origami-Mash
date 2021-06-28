using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPowerUp : MonoBehaviour
{

    void Start()
    {
        //StartCoroutine(Destroy());
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 11)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
