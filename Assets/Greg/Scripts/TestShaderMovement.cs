using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShaderMovement : MonoBehaviour
{
    public Material stencilMat;
    void Update()
    {
        stencilMat.SetVector("_Position", Camera.main.WorldToViewportPoint(transform.position));
    }
}
