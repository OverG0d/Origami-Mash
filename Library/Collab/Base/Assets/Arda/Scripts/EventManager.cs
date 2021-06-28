using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventManager : NetworkBehaviour
{
    GameObject winner;
    bool winnerFound;
    public static EventManager instance;
    public GameObject startGame;
    public GameObject resetLevel;
    public GameObject quit;
    public GameObject waitingStartNotif, waitingResetNotif;

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

    [ServerCallback]
    public void PlayerKillNotif(GameObject yourself, GameObject playerYouKilled)
    {
        if (playerYouKilled.GetComponent<Player>().playerHealth <= 0)
        {
            //yourself.GetComponent<Player>().killNotif.text = "You killed " + playerYouKilled.GetComponent<Player>().playerName;
            yourself.GetComponent<Player>().killCount++;
            TargetKillNotif(yourself.GetComponent<NetworkIdentity>().connectionToClient, yourself, playerYouKilled);
            RpcRemovePlayerFromList(playerYouKilled);
            if (ProgressManager.instance.alivePlayers.Count <= 1)
            {
                RpcWinner(yourself);
            }
        }       
    }

    [ServerCallback]
    public void PlayerDeathNotif(GameObject deadPlayer, GameObject killer)
    {
        if (deadPlayer.GetComponent<Player>().playerHealth <= 0)
        {
            //deadPlayer.GetComponent<Player>().killNotif.text = "Killed by " + killer.GetComponent<Player>().playerName;
            TargetDeathNotif(deadPlayer.GetComponent<NetworkIdentity>().connectionToClient, killer, deadPlayer);
            RpcRemovePlayerFromList(deadPlayer);
            if (ProgressManager.instance.alivePlayers.Count <= 1)
            {
                RpcWinner(killer);
            }
        }
    }

    [ServerCallback]
    public void RemovePlayerCheckWinner(GameObject player)
    {
        
        if (player.GetComponent<Player>().playerHealth <= 0)
        {
            ProgressManager.instance.alivePlayers.Remove(player);
            RpcRemovePlayerFromList(player);
        }
       
        if (ProgressManager.instance.alivePlayers.Count <= 1)
        {
            for (int i = 0; i < ProgressManager.instance.allPlayers.Count; i++)
            {
                if (ProgressManager.instance.allPlayers[i] != null && ProgressManager.instance.allPlayers[i].GetComponent<Player>().playerHealth > 0)
                {
                    TsunamiSpawner.instance.CancelInvoke("SpawnTsunami");
                    RpcWinner(ProgressManager.instance.allPlayers[i]);
                }
            }
        }
    }

    [TargetRpc]
    public void TargetKillNotif(NetworkConnection conn, GameObject attackingPlayer, GameObject opposingPlayer)
    {
        attackingPlayer.GetComponent<Player>().killNotif.text = "You killed " + opposingPlayer.GetComponent<Player>().playerName;
        attackingPlayer.GetComponent<Player>().killCount++;
        StartCoroutine(ClearKilledText(attackingPlayer));
        
    }

    [TargetRpc]
    public void TargetDeathNotif(NetworkConnection conn, GameObject attackingPlayer, GameObject opposingPlayer)
    {
        opposingPlayer.GetComponent<Player>().killNotif.text = "Killed by " + attackingPlayer.GetComponent<Player>().playerName;
        //attackingPlayer.GetComponent<Player>().killCount++;
        StartCoroutine(ClearKilledText(opposingPlayer));
        
    }

    IEnumerator ClearKilledText(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        obj.GetComponent<Player>().killNotif.text = "";
    }


    [ClientRpc]
    public void RpcWinner(GameObject player)
    {
        TsunamiSpawner.instance.CancelInvoke("SpawnTsunami");
        for (int i = 0; i < ProgressManager.instance.allPlayers.Count;i++)
        {
            ProgressManager.instance.allPlayers[i].GetComponent<Player>().winner.text = player.GetComponent<Player>().playerName + " wins!";
        }
        StartCoroutine(Winner());
    }

    IEnumerator Winner()
    {
        LobbyManager.instance.gameStarted = false;
        ProgressManager.instance.CancelInvoke("Timer");
        TsunamiSpawner.instance.CancelInvoke("Timer");
        TsunamiSpawner.instance.CancelInvoke("SpawnTsunami");
        TsunamiSpawner.instance.counter = 50;
        ProgressManager.instance.timer = 200;
        
        yield return new WaitForSeconds(3f);
        ProgressManager.instance.gameDone = false;

        for (int i = 0; i < ProgressManager.instance.allPlayers.Count; i++)
        {
            ProgressManager.instance.allPlayers[i].GetComponent<Player>().killNotif.text = "";
        }

        if (isServer)
        {
            resetLevel.SetActive(true);
            quit.SetActive(true);
        }
        else 
        {
            waitingResetNotif.SetActive(true);
        }
    }

    [ServerCallback]
    public void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "NetworkingScene")
        { 
            NetworkManager.singleton.ServerChangeScene(scene.path);
            
        }

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    NetworkManager.singleton.StartHost();
                }
            }
        }

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                NetworkManager.singleton.StartClient();
            }
        }
    }

    public void Quit()
    {
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

    [ClientRpc]
    void RpcRemovePlayerFromList(GameObject opposingPlayer)
    {
        ProgressManager.instance.alivePlayers.Remove(opposingPlayer);
    }

}
