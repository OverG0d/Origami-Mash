using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderInputs : MonoBehaviour
{
    public Camera _camera;
    public Shader _drawShader;

    private RenderTexture _splatMap;
    private Material _snowMaterial, _drawMaterial;
    private RaycastHit _hit;

    void Start()
    {
        _drawMaterial = new Material(_drawShader);
        _drawMaterial.SetVector("_Color", Color.red);

        _snowMaterial = GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatMap);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                _drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, temp);
                Graphics.Blit(temp, _splatMap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}
