using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyManager : NetworkBehaviour
{
    public int numberOfPlayers;
    public GameObject menu;
    public static LobbyManager instance;
    public bool gameStarted;
    public GameObject startGame;
    public GameObject stopHost;
    public GameObject stopClient;
    public GameObject waitingStartNotif;

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

    void Start()
    {

        if (NetworkServer.active && NetworkClient.isConnected)
        {
            startGame.SetActive(true);
            stopHost.SetActive(true);
        }
       
        if (NetworkClient.isConnected && !isServer)
        {
            stopClient.SetActive(true);
            waitingStartNotif.SetActive(true);
        }


    }

    public void StartGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            if (!gameStarted && ProgressManager.instance.alivePlayers.Count >= 1)
            {
                RpcStartGame();
            }
        }   
    }

    public void BackToLobby()
    {
        menu.SetActive(true);
    }

    [ClientRpc]
    void RpcStartGame()
    {
        waitingStartNotif.SetActive(false);
        startGame.SetActive(false);
        gameStarted = true;
        TsunamiSpawner.instance.InvokeRepeating("Timer", 0f, 1f);
        TsunamiSpawner.instance.InvokeRepeating("SpawnTsunami", 50f, 50f);
        ProgressManager.instance.InvokeRepeating("Timer", 0f, 1f);
        menu.SetActive(false);
    }


    public void StopHost()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Camera.main.orthographic = false;
            NetworkManager.singleton.StopHost();
        }

    }

    public void StopClient()
    {
        // stop client if client-only
        if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
