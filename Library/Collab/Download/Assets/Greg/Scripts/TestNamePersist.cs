using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNamePersist : MonoBehaviour
{
    public static TestNamePersist instance;
    public string playerName;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
