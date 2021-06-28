using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using IEntity;

[System.Serializable]
public class SyncListPlayerList : SyncList<GameObject> { }

public class ProgressManager : NetworkBehaviour
{

    public int timer;

    public int numberOfPlayersLeft;

    public static ProgressManager instance;

    public List<GameObject> alivePlayers = new List<GameObject>();

    public List<GameObject> allPlayers = new List<GameObject>();

    public List<GameObject> levelObjects = new List<GameObject>();

    public List<GameObject> tsunamis = new List<GameObject>();

    public bool gameDone, setupIcon;

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

    void Update()
    {
        if (!gameDone && LobbyManager.instance.gameStarted)
        {
            for (int i = 0; i < alivePlayers.Count; i++)
            {
                if (alivePlayers[i].GetComponent<Player>().playerHealth <= 0)
                {
                    if (alivePlayers[i].GetComponent<Player>().id != null)
                    {
                        EventManager.instance.PlayerKill(alivePlayers[i].gameObject, alivePlayers[i].GetComponent<Player>().id);
                    }
                    else
                    {
                        EventManager.instance.SelfKill(alivePlayers[i].gameObject);
                    }

                    PlayerProgress(alivePlayers[i].gameObject);
                }
            }
        }

        //COMMENT THIS OUT IF YOU WANT SINGLE PLAYER TESTING
        //if (alivePlayers.Count <= 1 && !gameDone && LobbyManager.instance.gameStarted)
        //{
        //    gameDone = true;
        //    for (int i = 0; i < alivePlayers.Count; i++)
        //    {
        //        if (allPlayers[i].GetComponent<Player>().playerHealth > 0)
        //        {
        //            if (isServer)
        //                EventManager.instance.RpcWinner(allPlayers[i]);
        //            break;
        //        }
        //    }
        //}
    }

    public void PlayerProgress(GameObject player)
    {
        for (int i = 0; i < alivePlayers.Count; i++)
        {
            if (alivePlayers[i] == null)
            {
                continue;
            }

            if (player.GetComponent<Player>().playerHealth <= 0)
            {
                PlayerDeath(player);
            }
        }

    }

    public void PlayerDeath(GameObject player)
    {
        player.GetComponent<Player>().minimapIcon.SetActive(false);
        player.GetComponent<Player>().playerHealth = 0;
        NetworkIdentity identity = player.GetComponent<NetworkIdentity>();
        TargetDead(identity.connectionToClient, player);
        RpcDead(player);
    }

    [TargetRpc]
    void TargetDead(NetworkConnection conn, GameObject player)
    {
        StartCoroutine(DeathToSpecDelay(player));
    }

    [ClientRpc]
    void RpcDead(GameObject player)
    {
        player.GetComponent<Player>().minimapIcon.SetActive(false);
        player.GetComponent<Player>().nonWave.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            player.GetComponent<Player>().particles[i].SetActive(false);
        }
        player.GetComponent<Player>().waveSpawn.SetActive(false);
        player.GetComponent<Player>().model.SetActive(false);
        player.GetComponent<Player>().playerHealthBar.SetActive(false);
        player.GetComponent<Player>().bgHealthBar.SetActive(false);
        player.GetComponent<BoxCollider>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        for (int i = 0; i < 1; i++)
        {
            player.transform.GetChild(i).transform.gameObject.SetActive(false);
        }
        player.GetComponent<Player>().rotate = false;
    }

    void Timer()
    {
        if (timer == 0)
        {
            CancelInvoke("Timer");
            if (alivePlayers.Count > 1)
            {
                timer = 200;
                if (NetworkServer.active && NetworkClient.isConnected)
                {
                    NetworkManager.singleton.StopHost();
                }

                // stop client if client-only
                else if (NetworkClient.isConnected)
                {
                    NetworkManager.singleton.StopClient();
                }
            }
        }

        if (timer > 0)
        {
            timer--;
        }

        for (int i = 0; i < alivePlayers.Count; i++)
        {
            if (alivePlayers[i] != null)
                alivePlayers[i].GetComponent<Player>().timer.text = "Time: " + timer;
        }
    }

    IEnumerator DeathToSpecDelay(GameObject player)
    {
        yield return new WaitForSeconds(3f);
        Camera.main.transform.parent = null;
        player.GetComponent<Player>().playerHealth = 0;
        for (int i = 0; i < alivePlayers.Count; i++)
        {
            if (player != alivePlayers[i] && alivePlayers[i].GetComponent<Player>().playerHealth > 0)
            {
                Camera.main.transform.parent = alivePlayers[i].gameObject.transform;
                Camera.main.transform.localPosition = new Vector3(0f, 40f, 0);
                Camera.main.transform.localEulerAngles = new Vector3(90f, 180f, 180f);
                Camera.main.orthographic = true;               
                player.GetComponent<Player>().playerOnCamera = alivePlayers[i];        
            }
        }       
    }
}
