using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LudumDare_Greg
{
    public class GregDrawShape : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler
    {
        public static GameObject selectedVertex;

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerPress);

            VertexManager.CreateVertex(eventData.position, VertexManager.instance.canvas, VertexManager.instance.testVertex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (selectedVertex != null)
            {
                GameObject connectedVertex = VertexManager.CreateVertex(eventData.position, VertexManager.instance.canvas, VertexManager.instance.testVertex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (selectedVertex != null)
            {
                GameObject connectedVertex = VertexManager.CreateVertex(eventData.position, VertexManager.instance.canvas, VertexManager.instance.testVertex);
            }
        }
    }
}
