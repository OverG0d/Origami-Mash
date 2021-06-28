using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LudumDare_Greg
{
    public class Vertex : MonoBehaviour,IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
    {
        [SerializeField]
        private GameObject connectedVertexPrefab;
        [SerializeField]
        private GameObject line;

        private GameObject connectedVertexRef;
        private GameObject connectedLineRef;

        public void OnBeginDrag(PointerEventData eventData)
        {
            CreateConnectedLine(transform.position, VertexManager.instance.canvas);
        }

        public void OnDrag(PointerEventData eventData)
        {
            connectedLineRef.GetComponent<RectTransform>().sizeDelta = new Vector2((eventData.position - (Vector2)transform.position).magnitude, connectedLineRef.GetComponent<RectTransform>().sizeDelta.y);
            Vector3 targetDir = eventData.position - (Vector2)transform.position;
            float angle = Vector3.SignedAngle(targetDir, Vector3.right, Vector3.back);
            connectedLineRef.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, angle);
            connectedLineRef.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.CompareTag("Vertex"))
            {
                eventData.pointerDrag.GetComponent<Vertex>().connectedVertexRef = gameObject;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            connectedLineRef.GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (connectedVertexRef == null)
            {
                Destroy(connectedLineRef);
            }
            else
            {
                connectedLineRef.GetComponent<RectTransform>().sizeDelta = new Vector2((connectedVertexRef.transform.position - transform.position).magnitude, connectedLineRef.GetComponent<RectTransform>().sizeDelta.y);
                Vector3 targetDir = connectedVertexRef.transform.position - transform.position;
                float angle = Vector3.SignedAngle(targetDir, Vector3.right, Vector3.back);
                connectedLineRef.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, angle);
            }
        }

        private void CreateConnectedLine(Vector3 startPos, Canvas canvas)
        {
            connectedLineRef = Instantiate(line);
            connectedLineRef.transform.position = startPos;
            connectedLineRef.transform.SetParent(canvas.transform);
        }
    }
}
