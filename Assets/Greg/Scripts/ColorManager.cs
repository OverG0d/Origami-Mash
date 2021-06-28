using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{

    public Material[] colorMaterials;

    public static ColorManager instance;

    private void Awake()
    {
        instance = this;
    }
}
