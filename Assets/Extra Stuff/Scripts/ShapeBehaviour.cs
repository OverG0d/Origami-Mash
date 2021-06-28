using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Derek
{
    public class ShapeBehaviour : MonoBehaviour
    {
        private LineRenderer renderer;
        private GameObject selectedObject;

        private bool hasConnected = false;

        private void Start()
        {
            renderer = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
            renderer.startWidth = 0.25f;
            renderer.endWidth = 0.25f;
            renderer.startColor = new Vector4(1, 0, 0, 1);
            renderer.positionCount = 0;

            gameObject.layer = LayerMask.NameToLayer("Canvas");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouse = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                    new Vector3(mouse.x, mouse.y, 0), Camera.MonoOrStereoscopicEye.Mono);

                RaycastHit hit;
                if (!hasConnected && selectedObject && Input.GetKey("f"))
                {
                    if (Physics.Raycast(Camera.main.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), 
                        Camera.MonoOrStereoscopicEye.Mono), Vector3.forward, out hit, 30, 
                        LayerMask.GetMask("Vertex"), QueryTriggerInteraction.Collide) &&
                        hit.collider.gameObject != selectedObject)
                    {
                        renderer.positionCount += 2;
                        renderer.SetPosition(renderer.positionCount-2, selectedObject.transform.position);
                        renderer.SetPosition(renderer.positionCount-1, hit.collider.gameObject.transform.position);
                    }

                    hasConnected = true;   
                }
                else if (Physics.Raycast(worldPos, Vector3.forward, out hit, 30, 
                    LayerMask.GetMask("Vertex"), QueryTriggerInteraction.Collide))
                {
                    selectedObject = hit.collider.gameObject;
                }
                else if (Physics.Raycast(worldPos, Vector3.forward, out hit, 30, 
                    LayerMask.GetMask("Canvas"), QueryTriggerInteraction.Collide))
                {
                    GameObject vertexObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    vertexObj.transform.position = new Vector3(worldPos.x, worldPos.y, 5);
                    vertexObj.name = "vertex";
                    BoxCollider coll = vertexObj.AddComponent(typeof(BoxCollider)) as BoxCollider;
                    coll.isTrigger = true;
                    vertexObj.layer = LayerMask.NameToLayer("Vertex");

                    selectedObject = null;
                }
            }
            
            if (Input.GetMouseButtonUp(0))
                hasConnected = false;
        }
    }
}