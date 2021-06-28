using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayer : TutorialShip
{
    public GameObject waveSpawn;
    public GameObject attackIndicator;
    public GameObject playerChargeBar;
    public Text cooldownTextIn, cooldownTextOut;
    public ParticleSystem powerupAura;
    IEnumerator powerUpCoroutine;
    IEnumerator indicatorCoroutine;
    public GameObject nonWave;
    public GameObject terrainIndicator;
    public Material seethroughMat;
    public static int matRef = Shader.PropertyToID("_Position");
    public static int matSize = Shader.PropertyToID("_Size");
    public LayerMask mask;
    public bool heldIn, heldOut;
    public float chargeAmount, numfloat;

    RaycastHit hitCircle;

    public ParticleSystem[] trails;
    public ParticleSystem frictionParticles;

    private void Start()
    {
        InitShip();
    }
    protected override void InitShip()
    {
        startPlayerHealth = 100;
        base.InitShip();
        attackIndicator.SetActive(false);
        nonWave.name = "NoWave";
        nonWave.SetActive(true);
        powerupAura.Stop(true);
        waveSpawn.name = "WaveSpawn";
        waveSpawn.SetActive(true);
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0f, 70f, 0);
        Camera.main.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        rigid.velocity = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Return) && TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_MOVEFORWARD)
        {
            TutorialEventManager.instance.CheckTutorialConditions();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);

            if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_ROTATELEFT)
            {
                TutorialEventManager.instance.CheckTutorialConditions();
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);

            if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_ROTATERIGHT)
            {
                TutorialEventManager.instance.CheckTutorialConditions();
            }
        }

        if (!hasFiredOut)
        {
            if (Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

                float _tempAmount = chargeAmount + (Time.deltaTime * numfloat);

                if (_tempAmount > 1)
                    chargeAmount = 1;
                else
                    chargeAmount = _tempAmount;

                heldOut = true;
                attackIndicator.SetActive(true);
            }

            if (heldOut)
            {
                Vector3 mouseDirVec = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
                attackIndicator.transform.forward = new Vector3(mouseDirVec.x, attackIndicator.transform.position.y, mouseDirVec.z);
                playerChargeBar.transform.localScale = new Vector3(chargeAmount, transform.localScale.y, transform.localScale.z);
            }

            if (Input.GetMouseButtonUp(1) && heldOut)
            {
                if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_RIGHTCLICK)
                {
                    TutorialEventManager.instance.CheckTutorialConditions();
                }
                Vector2 mouseVec = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
                Vector2 mouseDirVec = (mouseVec - new Vector2(transform.position.x, transform.position.z)).normalized;
                SpawnWaveOutwards(transform.position + new Vector3(mouseDirVec.x, transform.position.y, mouseDirVec.y).normalized * 5, chargeAmount);
                heldOut = false;
                attackCounterOut = 0;
                attackIndicator.SetActive(false);
                hasFiredOut = true;
                chargeAmount = 0;
                playerChargeBar.transform.localScale = new Vector3(chargeAmount, transform.localScale.y, transform.localScale.z);
            }

        }
        else
        {
            float cooldownValue = (attackCooldownOut - attackCounterOut);
            if (cooldownValue <= 0)
            {
                hasFiredOut = false;
                cooldownTextOut.text = "";
            }
            else
            {
                cooldownTextOut.text = "" + cooldownValue.ToString("f1");
            }

        }

        if (!hasFiredIn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NoSpawn"))
                    {
                        hitCircle = hit;
                        return;
                    }
                }

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.transform.parent == transform && hit.collider.gameObject.layer == LayerMask.NameToLayer("Spawn"))
                    {
                        if (TutorialEventManager.instance.currentHint == TutorialEventManager.TutorialCount.HINT_LEFTCLICK)
                        {
                            TutorialEventManager.instance.CheckTutorialConditions();
                        }
                        hitCircle = hit;
                        hasFiredIn = true;
                        attackCounterIn = 0;
                        SpawnWave(hit.point);
                    }
                }
            }
        }
        else
        {
            float cooldownValue = (attackCooldownIn - attackCounterIn);
            if (cooldownValue <= 0)
            {
                hasFiredIn = false;
                cooldownTextIn.text = "";
            }
            else
            {
                cooldownTextIn.text = "" + cooldownValue.ToString("f1");
            }
        }

        //speed friction particles
        if (rigid.velocity.magnitude > 10)
        {
            ParticleSystem.EmissionModule frictionEM = frictionParticles.emission;
            frictionEM.enabled = true;
            frictionParticles.transform.forward = rigid.velocity.normalized;
        }
        else
        {
            ParticleSystem.EmissionModule frictionEM = frictionParticles.emission;
            frictionEM.enabled = false;
        }

        //trail particles
        if (rigid.velocity.magnitude > 1)
        {

            foreach (ParticleSystem trail in trails)
            {
                ParticleSystem.EmissionModule trailEM = trail.emission;
                trailEM.enabled = true;
                trail.transform.forward = rigid.velocity.normalized;
            }
        }
        else
        {
            foreach (ParticleSystem trail in trails)
            {
                ParticleSystem.EmissionModule trailEM = trail.emission;
                trailEM.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12 || collision.gameObject.layer == 18)
        {
            StartBlinkIndicator();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Powerup"))
        {
            DamageMultiplier();
        }
    }
    protected override void DestroyShip()
    {
        Camera.main.transform.SetParent(null);
        base.DestroyShip();
    }

    void DamageMultiplier()
    {
        ParticleSystem.MainModule main = powerupAura.main;
        main.prewarm = true;
        powerupAura.Play();
        if (powerUpCoroutine == null)
        {
            powerUpCoroutine = DamageMultiplierCoroutine();
            StartCoroutine(powerUpCoroutine);
        }
        else
        {
            StopCoroutine(powerUpCoroutine);
            powerUpCoroutine = DamageMultiplierCoroutine();
            StartCoroutine(powerUpCoroutine);
        }
    }

    public void StartBlinkIndicator()
    {
        if (indicatorCoroutine == null)
        {
            indicatorCoroutine = BlinkIndicator();
            StartCoroutine(indicatorCoroutine);
        }
    }

    IEnumerator DamageMultiplierCoroutine()
    {
        damageMultiplier = 2;
        yield return new WaitForSeconds(10f);
        damageMultiplier = 1;
        powerupAura.Stop(true);
        powerUpCoroutine = null;
    }

    //coroutine for indicator
    IEnumerator BlinkIndicator()
    {
        terrainIndicator.SetActive(true);
        int counter = 0;
        float blinkCounter = 0;
        while (counter < 5)
        {
            blinkCounter += Time.deltaTime;
            if (blinkCounter > 0.5f)
            {
                blinkCounter = 0;
                counter++;
                terrainIndicator.SetActive(!terrainIndicator.activeSelf);
            }
            yield return null;
        }
        terrainIndicator.SetActive(false);
        indicatorCoroutine = null;
    }

}
