using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace LudumDare_Greg
{
    public class PlayerManager : NetworkBehaviour
    {
        public Transform player;
        public GameObject waveObjPref;
        public TestWaves testwaves;
        RaycastHit hit;
        Ray ray;

        private void Awake()
        {
            //player = transform;
            //testwaves = GameObject.FindGameObjectWithTag("Wave").GetComponent<TestWaves>();
        }

        private void Update()
        {

            if (hasAuthority)
            {

            }


        }

        /*[Command]
        void CmdFireInwward()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(testwaves.meshRenderer.material.GetFloat("_Radius") * testwaves.transform.lossyScale.x + "|||" + Mathf.Abs(Vector3.Distance(hit.point, player.position)));
                if (testwaves.meshRenderer.material.GetFloat("_Radius") * testwaves.transform.lossyScale.x > Mathf.Abs(Vector3.Distance(hit.point, player.position)))
                {
                    GameObject waveObj = Instantiate(waveObjPref, hit.point, Quaternion.identity);
                    waveObj.GetComponent<WaveObject>().toward = true;
                    waveObj.GetComponent<WaveObject>().targetDest = player.position;
                    NetworkServer.Spawn(waveObj);

                }
            }


        }

        [Command]
        private void CmdFireOutward()
        { 
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (testwaves.meshRenderer.material.GetFloat("_Radius") * testwaves.transform.lossyScale.x > Mathf.Abs(Vector3.Distance(hit.point, player.position)))
                    {
                        GameObject waveObj = Instantiate(waveObjPref, new Vector3(hit.point.x, player.position.y, hit.point.z), Quaternion.identity);
                        waveObj.GetComponent<WaveObject>().toward = false;
                        waveObj.GetComponent<WaveObject>().targetDest = player.position - new Vector3(ray.direction.x * 50, player.position.y, ray.direction.z * 50);
                        NetworkServer.Spawn(waveObj);
                    }
                }
        }*/

    }
}