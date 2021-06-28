using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShip : MonoBehaviour
{
    protected Color playerColor;

    protected Rigidbody rigid;

    public bool rotate = true;
    public int damageMultiplier;
    public GameObject model;
    public GameObject wave;
    public GameObject seaMine;
    public float pushSpeed;
    public float turnSpeed;

    public GameObject healthBar;

    private int playerHealth;
    public int startPlayerHealth;


    public float attackCounterIn, attackCounterOut;

    public bool hasFiredIn, hasFiredOut;
    public int attackCooldownIn, attackCooldownOut;

    public float respawnCooldown;
    public bool isDestroyed;

    private void Start()
    {
        InitShip();
    }

    protected virtual void Update()
    {
        attackCounterIn += Time.deltaTime;
        attackCounterOut += Time.deltaTime;

        if (rigid.velocity.magnitude > 20)
        {
            rigid.velocity = rigid.velocity.normalized * 20;
        }

        if (isDestroyed)
        {
            respawnCooldown += Time.deltaTime;

            if (respawnCooldown > 5)
            {
                InitShip();
            }
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        ShipCollision(col);
    }

    protected virtual void InitShip()
    {
        playerHealth = startPlayerHealth;
        isDestroyed = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        playerColor = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        model.GetComponent<MeshRenderer>().material.color = playerColor;
        rigid = GetComponent<Rigidbody>();
        
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        healthBar.transform.localScale = new Vector3(1, 1, 1);
    }

    protected virtual void DestroyShip()
    {
        isDestroyed = true;
        respawnCooldown = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    protected virtual void ShipCollision(Collision col)
    {


        if (col.gameObject.CompareTag("Seamine"))
        {
            Hit(col.gameObject.GetComponent<TutorialSeamine>().attackDamage);
        }

        if (col.gameObject.layer == 16)
        {
            if (!(col.gameObject.GetComponent<TutorialWave>().source == this))
                Hit(col.gameObject.GetComponent<TutorialWave>().attackDamage);
        }
    }

    public void Hit(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            DestroyShip();
        }
        healthBar.transform.localScale -= new Vector3((float) damage / startPlayerHealth, 0, 0);
    }

    public void SpawnWave(Vector3 spawnLoc)
    {
        if (wave != null)
        {
            GameObject waveClone = Instantiate(wave, new Vector3(spawnLoc.x, spawnLoc.y, spawnLoc.z), wave.transform.rotation);
            waveClone.GetComponent<TutorialWave>().RotateTowards(transform.position);
            waveClone.GetComponent<TutorialWave>().attackDamage *= damageMultiplier;
            waveClone.GetComponent<TutorialWave>().source = this;
        }
    }
    public void SpawnWaveOutwards(Vector3 spawnLoc, float damage)
    {
        if (wave != null)
        {
            GameObject waveClone = Instantiate(wave, new Vector3(spawnLoc.x, spawnLoc.y, spawnLoc.z), wave.transform.rotation);
            waveClone.GetComponent<TutorialWave>().RotateOutwards(transform.position);
            waveClone.GetComponent<TutorialWave>().attackDamage += (int) (damage * 20);
            waveClone.GetComponent<TutorialWave>().attackDamage *= damageMultiplier;
            waveClone.GetComponent<TutorialWave>().source = this;
        }
    }
}
