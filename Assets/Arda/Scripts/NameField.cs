using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameField : MonoBehaviour
{
    public static NameField instance;
    public string playerName;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
