using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare_Greg
{
    public class TestVectorShaderPos : MonoBehaviour
    {
        public Transform testObj;
        public MeshFilter thisMF;
        public float width;
        public float length;
        public float offsetx;
        public float offsetz;
        public float objOffsetX;
        public float objOffsetZ;


        private void Start()
        {
            thisMF = GetComponent<MeshFilter>();
            width = thisMF.mesh.bounds.size.x;
            length = thisMF.mesh.bounds.size.z;
            offsetx = transform.position.x;
            offsetz = transform.position.z;
        }

        void Update()
        {
            Vector4 testVector = new Vector4((testObj.position.x - offsetx) * 10 / width / transform.lossyScale.x + objOffsetX, 0, (testObj.position.z - offsetz) * 10 / length / transform.lossyScale.z + objOffsetZ);
            //Debug.Log((testObj.position.x - offsetx) + "," + (testObj.position.z - offsetz) + " || " + GetComponent<MeshRenderer>().material.GetVector("_Mid"));
            GetComponent<MeshRenderer>().material.SetVector("_Mid", testVector);
        }
    }
}