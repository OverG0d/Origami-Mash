using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare_Greg
{
    public class VertexManager : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject testVertex; //instance
        public static VertexManager instance;

        private void Awake()
        {
            instance = this;
        }

        public static GameObject CreateVertex(Vector3 startPos, Canvas canvas, GameObject testVertex)
        {
            GameObject vertex = Instantiate(testVertex);
            vertex.transform.position = startPos;
            vertex.transform.SetParent(canvas.transform);
            Debug.Log(vertex.transform.position);
            return vertex;
        }
    }
}
