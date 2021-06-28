using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMineSpawn : MonoBehaviour
{
    public Transform player;
    public GameObject minePrefab;
    public float cooldown;
    private GameObject currentMine;

    private void Awake()
    {
        currentMine = Instantiate(minePrefab, transform.position + new Vector3(0,5,0), Quaternion.identity);
    }
    private void Update()
    {
        cooldown += Time.deltaTime;
        if (cooldown > 5)
        {
            cooldown = 0;
            if (player != null)
            {
                if (!IsWithinSpawn(player))
                {
                    SpawnMine();
                }
            }
        }
    }
    public void SpawnMine() 
    {
        if (!currentMine.activeSelf)
        {
            Vector3 spawnLoc = transform.position + new Vector3(0,5,0);
            currentMine.transform.position = spawnLoc;
            currentMine.SetActive(true);
            currentMine.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    public bool IsWithinSpawn(Transform target) 
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, target.position)) < 4)
        {
            return true;
        }
        return false;
    }
}
