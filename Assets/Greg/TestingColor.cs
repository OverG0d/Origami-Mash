using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingColor : MonoBehaviour
{


    void Update()
    {
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.blue);
    }
}
