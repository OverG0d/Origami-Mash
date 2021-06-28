using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TsunamiSpawner : NetworkBehaviour
{
    public static TsunamiSpawner instance;
    public GameObject tsunami;
    public Transform[] positions;
    public int counter = 50;
    public GameObject pathOne;
    public GameObject pathTwo;

    public override void OnStartServer()
    {
        base.OnStartServer();
        pathOne.SetActive(false);
        pathTwo.SetActive(false);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        pathOne.SetActive(false);
        pathTwo.SetActive(false);
    }


   void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }    
    }

    void Timer()
    {
        counter--;
        if (counter == 0)
        {
            counter = 50;
        }
    }

    [ServerCallback]
    void SpawnTsunami()
    {      
        int random = Random.Range(0, 4);
        if(random == 1 || random == 3)
        {
            RpcCreatePath(random);          
        }
        else
        {
            RpcCreatePath(random);
        }
        if (ProgressManager.instance.timer > 0)
        {
            GameObject tsunami_Clone = Instantiate(tsunami, positions[random].position, positions[random].rotation);
            NetworkServer.Spawn(tsunami_Clone);
        }
    }

    [ClientRpc]
    void RpcCreatePath(int ranNum)
    {
        StartCoroutine(CreatePath(ranNum));       
    }

    IEnumerator CreatePath(int ranNum)
    {
        if (ranNum == 1 || ranNum == 3)
        {
            pathTwo.SetActive(true);
        }
        else
        {
            pathOne.SetActive(true);
        }              
        yield return new WaitForSeconds(10f);
        pathOne.SetActive(false);
        pathTwo.SetActive(false);
    }
}
