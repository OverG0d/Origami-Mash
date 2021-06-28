using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare_Greg
{
    public class TestWaves : MonoBehaviour
    {
        /*-dimensions refer to 'tile' length/width
         *-remember that each row/column of 'tiles' is equal to 'dimensions + 1' vertices long
         *-for example: a tilemap with length of 10 will be (10 + 1) vertices long*/
        public int dimensions = 10;
        public float UVScale;
        public Transform[] floatingObjects;
        public Transform testFloatObjRef;

        MeshFilter meshFilter; //the component that determines what mesh is being used
        public Mesh mesh; //the source mesh
        public MeshRenderer meshRenderer;
        public Octave[] octaves;

        public float width;
        public float length;
        public float offsetx;
        public float offsetz;
        public float objOffsetX;
        public float objOffsetZ;
        public float amountMult;
        public float rise;
        public float riseIncrement;


        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            mesh = new Mesh();
            mesh.name = "TestMesh";

            mesh.vertices = GenerateVertices();
            mesh.triangles = GenerateTriangles();
            mesh.uv = GenerateUVs();
            mesh.RecalculateNormals();

            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            width = mesh.bounds.size.x;
            length = mesh.bounds.size.z;
            offsetx = transform.position.x;
            offsetz = transform.position.z;

            gameObject.AddComponent<BoxCollider>();

            transform.position = new Vector3(transform.position.x - dimensions * transform.lossyScale.x / 2, transform.position.y, transform.position.z - dimensions * transform.lossyScale.z / 2);
        }

        public float GetHeight(Vector3 position)
        {
            /*Returns height value of the point taking into account scaling*/

            Vector3 scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
            Vector3 localPos = Vector3.Scale((position - transform.position), scale);

            Vector3 p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
            Vector3 p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
            Vector3 p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
            Vector3 p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

            p1.x = Mathf.Clamp(p1.x, 0, dimensions);
            p1.z = Mathf.Clamp(p1.z, 0, dimensions);
            p2.x = Mathf.Clamp(p2.x, 0, dimensions);
            p2.z = Mathf.Clamp(p2.z, 0, dimensions);
            p3.x = Mathf.Clamp(p3.x, 0, dimensions);
            p3.z = Mathf.Clamp(p3.z, 0, dimensions);
            p4.x = Mathf.Clamp(p4.x, 0, dimensions);
            p4.z = Mathf.Clamp(p4.z, 0, dimensions);

            float max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
            float dist = (max - Vector3.Distance(p1, localPos)) + (max - Vector3.Distance(p2, localPos)) + (max - Vector3.Distance(p3, localPos)) + (max - Vector3.Distance(p4, localPos));
            float height = mesh.vertices[XZ_Index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos)) +
                mesh.vertices[XZ_Index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos)) +
                mesh.vertices[XZ_Index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos)) +
                mesh.vertices[XZ_Index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

            return height * transform.lossyScale.y / dist;
        }

        private Vector2[] GenerateUVs()
        {
            Vector2[] uvs = new Vector2[mesh.vertices.Length];

            //loop through all the vertices
            for (int x = 0; x <= dimensions; x++)
            {
                for (int z = 0; z <= dimensions; z++)
                {
                    /*Vectors will have a value from 0 to 1 regardless of the UVScale value.
                     *UV's get flipped for each time a texture is fully drawn.*/
                    Vector2 vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                    uvs[XZ_Index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);


                    //Debug.Log("Index: " + XZ_Index(x, z) + " | UV Vector: " + vec);
                }
            }

            return uvs;
        }

        private void Update()
        {
            UpdateVertices();
        }

        private Vector3[] GenerateVertices()
        {
            Vector3[] verts = new Vector3[(dimensions + 1) * (dimensions + 1)];

            for (int x = 0; x <= dimensions; x++)
            {
                for (int z = 0; z <= dimensions; z++)
                {
                    verts[x * (dimensions + 1) + z] = new Vector3(x, 0, z);
                }
            }
            return verts;
        }

        private int[] GenerateTriangles()
        {
            int[] triangles = new int[mesh.vertices.Length * 6];

            for (int x = 0; x < dimensions; x++)
            {
                for (int z = 0; z < dimensions; z++)
                {
                    /*The order of the vertices in the array is very important as
                     * it determines how the mesh is rendered in the scene.
                     * If ordered incorrectly, it can make the mesh disappear.
                     */

                    int tri1Vert1 = XZ_Index(x, z);
                    int tri1Vert2 = XZ_Index(x + 1, z + 1);
                    int tri1Vert3 = XZ_Index(x + 1, z);

                    int tri2Vert1 = XZ_Index(x, z);
                    int tri2Vert2 = XZ_Index(x, z + 1);
                    int tri2Vert3 = XZ_Index(x + 1, z + 1);


                    triangles[XZ_Index(x, z) * 6] = tri1Vert1;
                    triangles[XZ_Index(x, z) * 6 + 1] = tri1Vert2;
                    triangles[XZ_Index(x, z) * 6 + 2] = tri1Vert3;
                    triangles[XZ_Index(x, z) * 6 + 3] = tri2Vert1;
                    triangles[XZ_Index(x, z) * 6 + 4] = tri2Vert2;
                    triangles[XZ_Index(x, z) * 6 + 5] = tri2Vert3;
                }
            }
            //DebugTriangles(triangles);
            return triangles;
        }

        private int XZ_Index(int x, int z)
        {
            return x * (dimensions + 1) + z;
        }

        private void UpdateVertices()
        {
            Vector3[] verts = mesh.vertices;
            for (int x = 0; x <= dimensions; x++)
            {
                for (int z = 0; z <= dimensions; z++)
                {
                    float y = 0;
                    for (int o = 0; o < octaves.Length; o++)
                    {
                        if (octaves[o].alternate) //loop through the collection of octaves
                        {
                            //first iteration of perlin noise
                            double perlin = Mathf.PerlinNoise(x * octaves[o].scale.x / dimensions, (z * octaves[o].scale.y) / dimensions) * Math.PI * 2f;
                            y += Mathf.Cos((float)perlin + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                        }
                        else
                        {
                            //alternating iteration of perlin noise
                            double perlin = Mathf.PerlinNoise((x * octaves[o].scale.x + Time.time * octaves[o].speed.x) / dimensions, (z * octaves[o].scale.y + Time.time * octaves[o].speed.y) / dimensions) - 0.5f;
                            y += Mathf.Cos((float)perlin + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                        }
                    }
                    verts[x * (dimensions + 1) + z] = new Vector3(x, y, z);
                }
            }


            Vector4 testVector = new Vector4((testFloatObjRef.position.x - offsetx) * mesh.bounds.size.x / width / transform.lossyScale.x + objOffsetX, 0, (testFloatObjRef.position.z - offsetz) * mesh.bounds.size.z / length / transform.lossyScale.z + objOffsetZ);
            //Debug.Log((testFloatObjRef.position.x - offsetx) + "," + (testFloatObjRef.position.z - offsetz) + " || " + meshRenderer.material.GetVector("_Mid"));
            meshRenderer.material.SetVector("_Mid", testVector);
            meshRenderer.material.SetFloat("_Amount", amountMult);

            mesh.vertices = verts;
            mesh.RecalculateNormals();


        }

        [Serializable]
        public struct Octave
        {
            public Vector2 speed;
            public Vector2 scale;
            public float height;
            public bool alternate;
        }
    }
}