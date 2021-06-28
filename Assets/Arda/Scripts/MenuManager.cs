using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    public InputField field;
    public GameObject connectionMessage;
    private IEnumerator startcoroutine;
    public InputField nameField;
    public GameObject joinGameMenu, mainMenu, settingsMenu;

    public Image wavePic;
    public readonly float xPosWaveStart = -765;
    public readonly float xPosWaveEnd = 765;
    public readonly float xTransitionSpd = 500;
    float timer = 0;

    void Start()
    {
        field.text = NetworkManager.singleton.networkAddress.ToString();
        field.onEndEdit.AddListener(delegate { NetworkManager.singleton.networkAddress = field.text; });
        Screen.SetResolution(1920, 1080, true);
    }

    void Update()
    {
        if (timer <= 5)
        {
            timer -= Time.deltaTime;
            if (!NetworkClient.isConnected)
            {
                connectionMessage.SetActive(true);
            }
            else
            {
                connectionMessage.SetActive(false);
            }

            if (timer <= 0)
            {
                timer = 6;
                connectionMessage.SetActive(false);
            }
        }
    }

    public void StartTutorial()
    {
        if (startcoroutine == null)
        {
            startcoroutine = TransitionCoroutine(delegate { NetworkManager.singleton.ServerChangeScene("Assets/Arda/Scenes/Tutorial.unity"); ; });
            StartCoroutine(startcoroutine);
        }
    }

    public void StartHost()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active &&
            Application.platform != RuntimePlatform.WebGLPlayer && startcoroutine == null)
        {
            // Server + Client
            startcoroutine = TransitionCoroutine(delegate
            {
                NetworkManager.singleton.StartHost();
                NameField.instance.playerName = nameField.text;
            });
            StartCoroutine(startcoroutine);
        }
    }

   

    public void StartClient()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active && startcoroutine == null)
        {
            startcoroutine = TransitionCoroutine(delegate
            {
                NetworkManager.singleton.StartClient();
                NameField.instance.playerName = nameField.text;
                timer = 5;
            });
            StartCoroutine(startcoroutine);
        }
    }

    public void JoinGameMenu()
    {
        joinGameMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void JoinGameToMainMenu()
    {
        joinGameMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void SettingsToMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public IEnumerator TransitionCoroutine(Action a)
    {
        while (wavePic.rectTransform.anchoredPosition.x < xPosWaveEnd)
        {
            wavePic.rectTransform.anchoredPosition += new Vector2(xTransitionSpd * Time.deltaTime, 0);
            yield return null;
        }
        wavePic.rectTransform.anchoredPosition = new Vector2(xPosWaveStart, wavePic.rectTransform.anchoredPosition.y);
        a.Invoke();
        startcoroutine = null;
    }

    public IEnumerator ConnectionMessage()
    {
        connectionMessage.SetActive(true);
        yield return new WaitForSeconds(5f);
        connectionMessage.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}