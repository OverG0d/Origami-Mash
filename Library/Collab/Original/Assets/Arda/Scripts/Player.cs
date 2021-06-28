using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using IEntity;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor))]
    Color playerColor;

    [SyncVar(hook = nameof(NameChange))]
    public string playerName;

    [SyncVar(hook = nameof(HealthChange))]
    public int playerHealth;

    [SyncVar(hook = nameof(KillChange))]
    public int killCount;

    public float chargeProgressLevel;

    [SyncVar(hook = nameof(ChargeChange))]
    public float chargeAmount;

    [SyncVar(hook = nameof(TrailParticle))]
    public float shipMagnitude;

    [Header("Components")]
    Rigidbody rigid;

    [Header("General Info")]
    public bool rotate = true;
    public int damageMultiplier;
    public GameObject model;
    public GameObject waveSpawn;
    public GameObject wave;
    public GameObject nonWave;
    public GameObject seaMine;
    public GameObject minimapIcon;
    public bool noWave;
    public float pushSpeed;
    public float turnSpeed;
    public float attackCounterIn, attackCounterOut;
    public Vector3 bulletSpawn;
    public bool isDead;


    public float chargeLevel;

    public bool heldIn, heldOut, hasFiredIn, hasFiredOut;

    private float attackCooldownIn, attackCooldownOut;

    [Header("UIDisplay")]
    public Text playerHealthUI;
    public Text playerNameUI;
    public Text playerKillsUI;
    public Text winner;
    public Text timer;
    public Text cooldownTextIn, cooldownTextOut;
    public Text numOfPlayers;
    public Text tsunamiTimer;
    public GameObject playerHealthBar;
    public GameObject bgHealthBar;
    public GameObject playerChargeBar;
    public GameObject bgChargeBar;
    public Text killNotif;
    public GameObject attackIndicator;
    public GameObject miniMap;
    public GameObject coolDownOne;
    public GameObject coolDownTwo;
    public GameObject outlineWarning;
    public bool blinker;

    public static int matRef = Shader.PropertyToID("_Position");

    public Material minimapvisionMat;

    [Header("PlayerVisualEffects")]
    public ParticleSystem powerupAura;
    public ParticleSystem frictionParticles;
    public ParticleSystem[] trails;

    float timeStampOne;
    float timeStampTwo;

    public GameObject id;
    public bool isIconSet;
    public GameObject playerOnCamera; //the player being spectated when you are dead
    public int spectatingCounter;
    public Vector3 startPosition;
    public Vector3 rotation;
    public int speed;
    Camera minimapCamera;
    public GameObject terrainIndicator;
    public GameObject[] particles;
    RaycastHit hitCircle;
    Material playerMaterial;
    Material cachedMaterial;

    public GameObject spectatedPlayer;

    IEnumerator powerUpCoroutine;
    IEnumerator indicatorCoroutine;

    public override void OnStartServer()
    {
        base.OnStartServer();
        transform.SetAsLastSibling();
        ProgressManager.instance.alivePlayers.Add(gameObject);
        ProgressManager.instance.allPlayers.Add(gameObject);
        LobbyManager.instance.numberOfPlayers++;
        //playerName = NameField.instance.playerName;
        ProgressManager.instance.timer = 200;
        startPosition = transform.position;
        rotation = transform.rotation.eulerAngles;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        ProgressManager.instance.timer = 200;
        if (!isServer)
        {
            LobbyManager.instance.numberOfPlayers++;
            ProgressManager.instance.numberOfPlayersLeft++;
            startPosition = transform.position;
            rotation = transform.rotation.eulerAngles;
            //playerName = NameField.instance.playerName;
            ProgressManager.instance.alivePlayers.Add(gameObject);
            ProgressManager.instance.allPlayers.Add(gameObject);
        }

        gameObject.transform.Find("PlayerUI").gameObject.SetActive(false); //turn off to prevent cooldown UI from stacking
        powerupAura.Stop(true);

    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        gameObject.transform.Find("PlayerUI").gameObject.SetActive(true); //turn on to prevent cooldown UI from stacking
        killCount = 0;
        playerKillsUI.text = "Kills: " + " " + killCount;
        attackCooldownIn = 1.2f;
        attackCooldownOut = 1.2f;
        spectatingCounter = 0;
        rigid = GetComponent<Rigidbody>();
        //Camera.main.transform.SetParent(transform);
        Camera.main.transform.position = new Vector3(transform.position.x, 200f, transform.position.z);
        Camera.main.transform.eulerAngles = new Vector3(90f, 0f, -transform.eulerAngles.y);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 25;

        CmdPlayerInfo();
        waveSpawn.SetActive(true);
        nonWave.SetActive(true);
        powerupAura.Stop(true);
        minimapIcon.SetActive(true);
        playerChargeBar.SetActive(true);
        bgChargeBar.SetActive(true);



        minimapCamera = MinimapCamera.instance.minimapCamera;

        minimapvisionMat.SetVector(matRef, minimapCamera.WorldToViewportPoint(transform.position));
        string playerName = NameField.instance.playerName;
        minimapCamera.transform.position = new Vector3(transform.position.x, minimapCamera.transform.position.y, transform.position.z);
        minimapCamera.transform.eulerAngles = new Vector3(minimapCamera.transform.eulerAngles.x, transform.eulerAngles.y, minimapCamera.transform.eulerAngles.z);
        CmdName(playerName);
    }

    public void CameraFollow(Transform objToFollow)
    {
        Camera.main.transform.position = new Vector3(objToFollow.position.x, Camera.main.transform.position.y, objToFollow.position.z);
        Camera.main.transform.eulerAngles = new Vector3(90, 0, -objToFollow.eulerAngles.y);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 25;
    }

    [Command]
    void CmdName(string _playerName)
    {
        playerName = _playerName;
    }

    void NameChange(string oldName, string newName)
    {
        if (isLocalPlayer)
        {
            playerNameUI.text = "Player: " + " " + playerName;
        }
    }

    void HealthChange(int oldHealth, int newHealth)
    {
        if (isLocalPlayer)
        {
            playerHealthUI.text = "Health: " + " " + playerHealth;
        }
        playerHealthBar.transform.localScale = new Vector3(((float)playerHealth / 100), playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
    }

    void KillChange(int oldKills, int newKills)
    {
        if (isLocalPlayer)
        {
            playerKillsUI.text = "Kills: " + " " + killCount;
        }
    }

    void ChargeChange(float oldNum, float newNum)
    {
        chargeProgressLevel = newNum * chargeLevel;
        playerChargeBar.transform.localScale = new Vector3(newNum, transform.localScale.y, transform.localScale.z);
    }

    [Command]
    void CmdPlayerInfo()
    {
        playerHealth = 100;
        playerHealthBar.transform.localScale = new Vector3((((float)playerHealth / 100)), transform.localScale.y, transform.localScale.z);
        playerChargeBar.transform.localScale = new Vector3(chargeAmount, transform.localScale.y, transform.localScale.z);
    }

    void SetColor(Color oldColor, Color newColor)
    {
        if (cachedMaterial == null)
        {
            cachedMaterial = model.GetComponent<Renderer>().material;
        }

        cachedMaterial.color = newColor;
    }


    void Update()
    {
        //need to check if has authority so that client/host can run code in update
        if (!hasAuthority)
        {
            return;
        }

        if (isServer)
        {
            //deals with situation when list member is null
            //-when someone quits
            //-when someone lags out
            for (int i = 0; i < ProgressManager.instance.allPlayers.Count; i++)
            {
                if (ProgressManager.instance.allPlayers[i] == null)
                {
                    ProgressManager.instance.allPlayers.RemoveAt(i);
                }
            }

            for (int i = 0; i < ProgressManager.instance.alivePlayers.Count; i++)
            {
                if (ProgressManager.instance.alivePlayers[i] == null)
                {
                    ProgressManager.instance.alivePlayers.RemoveAt(i);
                }
            }
        }

        numOfPlayers.text = "Number of players: " + ProgressManager.instance.alivePlayers.Count;
        tsunamiTimer.text = "Next Tsunami: " + TsunamiSpawner.instance.counter;

        if (TsunamiSpawner.instance.counter <= 10 && !blinker)
        {
            blinker = true;
            StartCoroutine(Blinker());
        }

        if (LobbyManager.instance.gameStarted)
        {
            if (!isIconSet)
            {

                for (int i = 0; i < ProgressManager.instance.alivePlayers.Count; i++)
                {
                    Player currentPlayerScript = ProgressManager.instance.alivePlayers[i].GetComponent<Player>();
                    currentPlayerScript.playerMaterial = ColorManager.instance.colorMaterials[i];
                    currentPlayerScript.model.GetComponent<MeshRenderer>().material = currentPlayerScript.playerMaterial;

                    Debug.Log(currentPlayerScript.playerName);
                    Debug.Log(currentPlayerScript.model.GetComponent<MeshRenderer>().material.name);
                    Debug.Log(currentPlayerScript.model.GetComponent<MeshRenderer>().material.color);

                    if (currentPlayerScript != this)
                    {
                        currentPlayerScript.minimapIcon.GetComponent<MeshRenderer>().material.SetColor("Color_4E58465B", Color.red);
                    }
                    else
                    {
                        currentPlayerScript.minimapIcon.GetComponent<MeshRenderer>().material = currentPlayerScript.playerMaterial;
                        currentPlayerScript.minimapIcon.GetComponent<Renderer>().material.SetTexture("_BaseMap", null);
                    }

                }
                for (int i = 0; i < ProgressManager.instance.alivePlayers.Count; i++)
                {
                    if (ProgressManager.instance.alivePlayers[i] != null && ProgressManager.instance.alivePlayers[i].GetComponent<Player>().playerHealth > 0)
                        ProgressManager.instance.alivePlayers[i].transform.Find("MinimapIconMesh").gameObject.SetActive(true);
                }
                isIconSet = true;
            }
            if (playerHealth > 0)
            {
                CameraFollow(transform);
            }
            else if (spectatedPlayer != null)
            {
                CameraFollow(spectatedPlayer.transform);
            }

            if (Input.GetKeyDown(KeyCode.C) && playerHealth <= 0)
            {
                spectatingCounter++;
                if (spectatingCounter == ProgressManager.instance.alivePlayers.Count)
                {
                    spectatingCounter = 0;
                }

                spectatedPlayer = ProgressManager.instance.alivePlayers[spectatingCounter];
                //Camera.main.transform.parent = null;
                //Camera.main.transform.parent = ProgressManager.instance.alivePlayers[spectatingCounter].gameObject.transform;
                //Camera.main.transform.localPosition = new Vector3(0f, 40f, 0);
                //Camera.main.transform.localEulerAngles = new Vector3(90f, 180f, 180f);
            }

            if (spectatedPlayer != null)
            {
                if (spectatedPlayer.GetComponent<Player>().playerHealth <= 0)
                {
                    for (int i = 0; i < ProgressManager.instance.alivePlayers.Count; i++)
                    {
                        if (gameObject != ProgressManager.instance.alivePlayers[i] && ProgressManager.instance.alivePlayers[i].GetComponent<Player>().playerHealth > 0)
                        {
                            //Camera.main.transform.parent = ProgressManager.instance.alivePlayers[i].gameObject.transform;
                            //Camera.main.transform.localPosition = new Vector3(0f, 40f, 0);
                            //Camera.main.transform.localEulerAngles = new Vector3(90f, 180f, 180f);
                            //Camera.main.orthographic = true;
                            //spectatedPlayer = ProgressManager.instance.alivePlayers[i];

                            spectatingCounter = i;
                            spectatedPlayer = ProgressManager.instance.alivePlayers[spectatingCounter];
                            break;
                        }
                    }
                }
            }

            if (isDead)
            {
                return;
            }
            if (rigid.velocity.magnitude > 20)
            {
                rigid.velocity = rigid.velocity.normalized * 20;
            }

            minimapvisionMat.SetVector(matRef, minimapCamera.WorldToViewportPoint(transform.position));
            minimapCamera.transform.position = new Vector3(transform.position.x, minimapCamera.transform.position.y, transform.position.z);

            attackCounterIn += Time.deltaTime;
            attackCounterOut += Time.deltaTime;

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
                minimapCamera.transform.eulerAngles = new Vector3(minimapCamera.transform.eulerAngles.x, transform.eulerAngles.y, minimapCamera.transform.eulerAngles.z);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
                minimapCamera.transform.eulerAngles = new Vector3(minimapCamera.transform.eulerAngles.x, transform.eulerAngles.y, minimapCamera.transform.eulerAngles.z);
            }

            if (!hasFiredOut)
            {
                if (Input.GetMouseButton(1))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

                    CmdIncrement(0.3f);

                    heldOut = true;
                    attackIndicator.SetActive(true);
                }

                if (heldOut)
                {
                    Vector3 mouseDirVec = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
                    attackIndicator.transform.forward = new Vector3(mouseDirVec.x, attackIndicator.transform.position.y, mouseDirVec.z);
                }

                if (Input.GetMouseButtonUp(1) && heldOut)
                {
                    Vector2 mouseVec = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
                    Vector2 mouseDirVec = (mouseVec - new Vector2(transform.position.x, transform.position.z)).normalized;
                    CmdSpawnWaveOutwards(transform.position + new Vector3(mouseDirVec.x, transform.position.y, mouseDirVec.y).normalized * 5);
                    heldOut = false;
                    attackCounterOut = 0;
                    attackIndicator.SetActive(false);
                    //if (ProgressManager.instance.timer > 0)
                    hasFiredOut = true;
                    CmdResetCharge();
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
                            hitCircle = hit;
                            hasFiredIn = true;
                            attackCounterIn = 0;
                            if (ProgressManager.instance.timer > 0)
                                CmdSpawnWave(hit.point);
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (NetworkServer.active && NetworkClient.isConnected)
                {
                    NetworkManager.singleton.StopHost();
                }

                // stop client if client-only
                else if (NetworkClient.isConnected)
                {
                    NetworkManager.singleton.StopClient();
                }
            }

            CmdTrailParticles(rigid.velocity.magnitude);
            //speed friction particles
            /*if (shipMagnitude > 10)
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
            if (shipMagnitude > 1)
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
            }*/
        }
    }


    [Command]
    void CmdTrailParticles(float magnitude)
    {
        shipMagnitude = magnitude;
        /*if (shipMagnitude > 10)
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
        if (shipMagnitude > 1)
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
        }*/
        RpcPlayParticles();
    }
    

    [ClientRpc]
    void RpcPlayParticles()
    {
        if (shipMagnitude > 10)
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
        if (shipMagnitude > 1)
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

    [Command]
    void CmdIncrement(float numfloat)
    {
        float _tempAmount = chargeAmount + (Time.deltaTime * numfloat);

        if (_tempAmount > 1)
            chargeAmount = 1;
        else
            chargeAmount = _tempAmount;
    }

    [Command]
    void CmdResetCharge()
    {
        chargeAmount = 0;
        playerChargeBar.transform.localScale = new Vector3(chargeAmount, transform.localScale.y, transform.localScale.z);
    }

    void TrailParticle(float oldNum, float newNum) { }

    IEnumerator Blinker()
    {
        Color color = tsunamiTimer.color;
        while (TsunamiSpawner.instance.counter <= 10)
        {
            tsunamiTimer.color = color;
            outlineWarning.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            tsunamiTimer.color = Color.green;
            outlineWarning.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
        blinker = false;
        tsunamiTimer.color = color;
    }

    [Command]
    void CmdSpawnWave(Vector3 hit2)
    {
        if (wave != null)
        {
            GameObject waveClone = Instantiate(wave, new Vector3(hit2.x, transform.position.y, hit2.z), wave.transform.rotation);
            waveClone.GetComponent<Wave>().RotateTowards(transform.position);
            waveClone.layer = 17;
            NetworkServer.Spawn(waveClone);
            RpcId(waveClone);
        }
    }

    [Command]
    void CmdSpawnWaveOutwards(Vector3 hit2)
    {
        if (wave != null)
        {
            GameObject waveClone = Instantiate(wave, new Vector3(hit2.x, transform.position.y, hit2.z), wave.transform.rotation);
            waveClone.GetComponent<Rigidbody>().AddRelativeForce(gameObject.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
            waveClone.GetComponent<Wave>().RotateOutwards(transform.position);

            if ((int)chargeProgressLevel == 0)
            {
                waveClone.GetComponent<Wave>().attackDamage *= damageMultiplier;
            }
            else
            {
                waveClone.GetComponent<Wave>().attackDamage *= damageMultiplier * (int)chargeProgressLevel;
            }

            if (waveClone != null)
            {
                waveClone.GetComponent<Wave>().id = gameObject;
            }
            NetworkServer.Spawn(waveClone);
            RpcId(waveClone);
            RpcCharge(waveClone);
        }
    }

    [ClientRpc]
    void RpcCharge(GameObject obj)
    {
        //obj.GetComponent<Wave>().attackDamage *= damageMultiplier * (int)chargeProgressLevel;
        /*if ((int)chargeProgressLevel == 0)
        {
            obj.GetComponent<Wave>().attackDamage *= damageMultiplier;
        }
        else
        {
            obj.GetComponent<Wave>().attackDamage *= damageMultiplier * (int)chargeProgressLevel;
        }*/
    }

    [ClientRpc]
    void RpcId(GameObject ob)
    {
        if (ob != null)
        {
            ob.GetComponent<Wave>().id = gameObject;
        }
    }

    [ServerCallback]
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Seamine"))
        {
            id = col.gameObject.GetComponent<Seamine>().id;
            if (playerHealth - col.gameObject.GetComponent<Seamine>().attackDamage < 0 && col.gameObject.GetComponent<Seamine>().id != gameObject)
            {
                playerHealth = 0;
            }
            else
            {
                if(col.gameObject.GetComponent<Seamine>().id != gameObject)
                    playerHealth -= col.gameObject.GetComponent<Seamine>().attackDamage;
            }


            if (col.gameObject.GetComponent<Seamine>().id != null)
            {
                //EventManager.instance.PlayerDeathNotif(gameObject, col.gameObject.GetComponent<Seamine>().id);
                //EventManager.instance.PlayerKillNotif(col.gameObject.GetComponent<Seamine>().id, gameObject);
            }

            EventManager.instance.RemovePlayerCheckWinner(gameObject);
            ProgressManager.instance.CheckPlayerAlive(gameObject);
        }
    }

    [ServerCallback]
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Powerup"))
        {
            RpcDamageIncrease();
        }

        if (col.gameObject.layer == 16)
        {
            //RpcGetId(col.gameObject);
            id = col.gameObject.GetComponent<Wave>().id;
            if (playerHealth - col.gameObject.GetComponent<Wave>().attackDamage < 0 && col.gameObject.GetComponent<Wave>().id != gameObject)
            {
                playerHealth = 0;
            }
            else
            {
                if (col.gameObject.GetComponent<Wave>().id != gameObject)
                    playerHealth -= col.gameObject.GetComponent<Wave>().attackDamage;
            }

            if (col.gameObject.GetComponent<Wave>().id != null)
            {
                EventManager.instance.PlayerDeathNotif(gameObject, col.gameObject.GetComponent<Wave>().id);
                EventManager.instance.PlayerKillNotif(col.gameObject.GetComponent<Wave>().id, gameObject);
            }
            gameObject.GetComponent<Rigidbody>().AddForce(col.transform.forward * 20, ForceMode.Impulse);
            EventManager.instance.RemovePlayerCheckWinner(gameObject);
            ProgressManager.instance.CheckPlayerAlive(gameObject);
            NetworkServer.Destroy(col.gameObject);
        }

        if (col.gameObject.layer == 12 || col.gameObject.layer == 18)
        {
            TargetStartBlinkIndicator(GetComponent<NetworkIdentity>().connectionToClient);
        }

        if (col.gameObject.layer == 21)
        {
            if (playerHealth - col.gameObject.GetComponent<Tsunami>().attackDamage < 0)
            {
                playerHealth = 0;
            }
            else
            {
                playerHealth -= col.gameObject.GetComponent<Tsunami>().attackDamage;
            }
            EventManager.instance.RemovePlayerCheckWinner(gameObject);
            ProgressManager.instance.CheckPlayerAlive(gameObject);
        }
    }

    /*[ClientRpc]
    void RpcGetId(GameObject col)
    {
        id = col.gameObject.GetComponent<Wave>().id;
    }*/

    [ClientRpc]
    void RpcDamageIncrease()
    {
        ParticleSystem.MainModule main = powerupAura.main;
        main.prewarm = true;
        powerupAura.Play();
        if (powerUpCoroutine == null)
        {
            powerUpCoroutine = DamageAmpCoroutine();
            StartCoroutine(powerUpCoroutine);
        }
        else
        {
            StopCoroutine(powerUpCoroutine);
            powerUpCoroutine = DamageAmpCoroutine();
            StartCoroutine(powerUpCoroutine);
        }
    }

    IEnumerator DamageAmpCoroutine()
    {
        damageMultiplier = 2;
        yield return new WaitForSeconds(10f);
        damageMultiplier = 1;
        powerupAura.Stop(true);
        powerUpCoroutine = null;
    }

    void OnDestroy()
    {
        Destroy(cachedMaterial);
    }

    [TargetRpc]
    public void TargetStartBlinkIndicator(NetworkConnection targetPlayer)
    {
        if (indicatorCoroutine == null)
        {
            indicatorCoroutine = BlinkIndicator();
            StartCoroutine(indicatorCoroutine);
        }
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