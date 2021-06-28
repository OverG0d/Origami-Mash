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

    public Image wavePic;
    public readonly float xPosWaveStart = -765;
    public readonly float xPosWaveEnd = 765;
    float timer = 0;

    void Start()
    {
        field.text = NetworkManager.singleton.networkAddress.ToString();
        field.onEndEdit.AddListener(delegate { ValueCheck(); });
        Screen.SetResolution(1920, 1080, true);
    }

    public void ValueCheck()
    {
        NetworkManager.singleton.networkAddress = field.text;
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
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    Debug.Log((Application.platform != RuntimePlatform.WebGLPlayer) + "  network");
                    if (startcoroutine == null)
                    {
                        startcoroutine = TransitionCoroutine(delegate {
                            NetworkManager.singleton.StartHost();
                        });
                        StartCoroutine(startcoroutine);
                    }
                }
            }
        }
    }

    public void StartClient()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                if (startcoroutine == null)
                {
                    startcoroutine = TransitionCoroutine(delegate {
                        NetworkManager.singleton.StartClient();
                        timer = 5;
                    });
                    StartCoroutine(startcoroutine);
                }

            }
        }
    }

    public IEnumerator TransitionCoroutine(Action a)
    {
        while (wavePic.rectTransform.anchoredPosition.x < xPosWaveEnd)
        {
            wavePic.rectTransform.anchoredPosition += new Vector2(500 * Time.deltaTime, 0);
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
//NetworkManager.singleton.ServerChangeScene("NetworkingScene");
