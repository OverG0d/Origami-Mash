using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Camera minimapCamera;

    public static MinimapCamera instance;

    private void Awake()
    {
        instance = this;
    }
}
