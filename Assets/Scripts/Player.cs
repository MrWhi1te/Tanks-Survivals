using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public Game GM;
    public Pool P;
    public LevelManager LM;
    public PlayerAuto PA;

    [Header("Main")]
    public int[] Resource;

    [Header("TanksPlayer")]
    public GameObject[] TrackTanks;
    public List<HeaderTanks> HeaderTanks = new List<HeaderTanks>();
    public List<HeaderTanksPrem> HeaderTanksPrem = new List<HeaderTanksPrem>();
    public GameObject[] SheildObj;
    public GameObject[] GunsPrefab;
    public GameObject[] GunsOptionalPrefab;

    [Header("TankSpecification")]
    public int MaxLife;
    public int Life;
    public int Sheild;
    public float SpeedPlayer;
    public int Damage;

    [Header("BullPrefabs")]
    public GameObject[] BulletObj;

    [Header("UI")]
    public Image LifeSlider;
    public Text LifeText;
    public Text SheildText;
    public Text[] ResourceText;

    [Header("Audio")]
    public AudioSource ShootBulletSource;
    public AudioSource MachineGunSource;
    public AudioSource DamageTankSource;
    public AudioSource DeadTankSource;
    public AudioSource DamageTankMachineGunSource;
    public AudioSource TakeItemSource;
    public AudioSource SheildSource;

    //
    public GameObject AIM;
    public GameObject Target;
    public VariableJoystick VJMove;
    public VariableJoystick VJHead;
    public GameObject[] Joystick;

    public float joy;

    private void Awake()
    {
        GM = Game.Instance;
        P = Pool.Instance;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LM = LevelManager.Instance;
        if (GM.PlayerPosition.x != 0 || GM.PlayerPosition.y != 0)
        {
            transform.position = GM.PlayerPosition;
        }
        StartGame();
        if (GM.DeviceGame == "mobile" || GM.DeviceGame == "tablet") { Joystick[0].SetActive(true); Joystick[1].SetActive(true); }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LM.ActiveMovePlayer)
        {
            if(GM.DeviceGame == "mobile" || GM.DeviceGame == "tablet")
            {
                Vector3 direction = Vector3.up * VJMove.Vertical + Vector3.right * VJMove.Horizontal;
                transform.Translate(direction * SpeedPlayer * Time.deltaTime);
                Vector2 Dir = (Vector3.down * VJHead.Vertical + Vector3.right * VJHead.Horizontal);
                if (VJHead.Horizontal != 0 || VJHead.Vertical != 0)
                {
                    if (GM.TankList[GM.SlotTankActive].PremTanks) { HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform.rotation = Quaternion.LookRotation(Vector3.forward, Dir); }
                    else { HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform.rotation = Quaternion.LookRotation(Vector3.forward, Dir); }
                }
                if (AIM.activeInHierarchy & PA.TargetEnemy != null) { if (PA.TargetEnemy.activeInHierarchy) { Target.SetActive(true); Target.transform.position = PA.TargetEnemy.transform.position; HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(PA.TargetEnemy.transform.position.y - transform.position.y, PA.TargetEnemy.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90); } else { Target.SetActive(false); } }
                else { Target.SetActive(false); }
            }
            else
            {
                Vector2 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg - 90f;
                if (GM.TankList[GM.SlotTankActive].PremTanks) { HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform.rotation = Quaternion.Euler(0f, 0f, rotateZ); }
                else { HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform.rotation = Quaternion.Euler(0f, 0f, rotateZ); }
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Translate(Vector3.up * SpeedPlayer * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    transform.Translate(Vector3.down * SpeedPlayer * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    transform.Translate(Vector3.right * SpeedPlayer * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    transform.Translate(Vector3.left * SpeedPlayer * Time.deltaTime);
                }
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    void StartGame()
    {
        if (GM.TankList[GM.SlotTankActive].PremTanks)
        {
            TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].SetActive(true);
            TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].GetComponent<SpriteRenderer>().color = new Color32((byte)HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].ColorTrack[0], (byte)HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].ColorTrack[1], (byte)HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].ColorTrack[2], 255);
            HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.SetActive(true);
            for(int i=0;i< HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].Guns.Length; i++)
            {
                if (HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].PlazmGun) { HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].Guns[i].GetComponentInChildren<Spawn_Bullet>().effect = P.effectPlayer[1]; }
                else { HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].Guns[i].GetComponentInChildren<Spawn_Bullet>().effect = P.effectPlayer[0]; }
            }
            MaxLife = GM.TankList[GM.SlotTankActive].LifeTank;
            SpeedPlayer = GM.TankList[GM.SlotTankActive].SpeedTank;
            Damage = GM.TankList[GM.SlotTankActive].DamageGun;
        }
        else
        {
            TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].SetActive(true);
            TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].GetComponent<SpriteRenderer>().color = new Color32((byte)GM.ColorA[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorB[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorC[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], 255);
            HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.SetActive(true);
            HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.GetComponent<SpriteRenderer>().color = new Color32((byte)GM.ColorA[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorB[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorC[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], 255);
            for (int i = 0; i < GM.TankList[GM.SlotTankActive].MachineGunTanks; i++)
            {
                HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].MachineTanks[i].SetActive(true);
            }
            for (int i = 0; i < HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length; i++)
            {
                GameObject obj = Instantiate(GunsPrefab[GM.TankList[GM.SlotTankActive].GunsTanks]);
                Spawn_Bullet[] child = obj.GetComponentsInChildren<Spawn_Bullet>();
                SpriteRenderer[] childSpr = obj.GetComponentsInChildren<SpriteRenderer>();
                foreach (Spawn_Bullet spawn_ in child)
                {
                    spawn_.GameStart = true;
                    spawn_.effect = P.effectPlayer[0];
                }
                foreach (SpriteRenderer spawn_ in childSpr)
                {
                    spawn_.color = new Color32((byte)GM.ColorA[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], (byte)GM.ColorB[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], (byte)GM.ColorC[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], 255);
                }
                obj.transform.parent = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform;
                obj.transform.position = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader[i].transform.position;
                if (GM.TankList[GM.SlotTankActive].GunsTanks == 8) { obj.GetComponentInChildren<Spawn_Bullet>().effect = P.effectPlayer[1]; }
            }
            if (GM.TankList[GM.SlotTankActive].LazerTanks)
            {
                GameObject obj = Instantiate(GunsOptionalPrefab[0]);
                obj.GetComponent<Lazer_Script>().GameStart = true;
                obj.transform.parent = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform;
                obj.transform.position = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].PointLazer.transform.position;
            }
            if (GM.TankList[GM.SlotTankActive].RocketTanks)
            {
                GameObject obj = Instantiate(GunsOptionalPrefab[1]);
                obj.GetComponent<Spawn_LazerPlazm>().GameStart = true;
                obj.GetComponent<Spawn_LazerPlazm>().effect = P.effectPlayer[0];
                obj.transform.parent = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTanks.transform;
                obj.transform.position = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].PointRocket.transform.position;
            }
            if (GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks] > 0) { MaxLife = GM.TankList[GM.SlotTankActive].LifeTank + (GM.TankList[GM.SlotTankActive].LifeTank / 100 * (GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks] * 10)); }
            else { MaxLife = GM.TankList[GM.SlotTankActive].LifeTank; }
            if (GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks] > 0) { SpeedPlayer = GM.TankList[GM.SlotTankActive].SpeedTank + (GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks] * 0.1f); }
            else { SpeedPlayer = GM.TankList[GM.SlotTankActive].SpeedTank; }
            if (GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks] > 0)
            {
                if (GM.TankList[GM.SlotTankActive].DamageGun < 100) { Damage = GM.TankList[GM.SlotTankActive].DamageGun + (GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks] * 8); }
                else { Damage = GM.TankList[GM.SlotTankActive].DamageGun + (GM.TankList[GM.SlotTankActive].DamageGun / 100 * (GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks] * 10)); }
            }
            else { Damage = GM.TankList[GM.SlotTankActive].DamageGun; }
            StartCoroutine(TimeInBattle());
        }
        LifeSlider.fillAmount = (float)Life/MaxLife; Life = MaxLife;
        Sheild = GM.TankList[GM.SlotTankActive].Sheild;
        StartCoroutine(MachineGunBullet());
        if (GM.TankList[GM.SlotTankActive].SizeTank == 0) { GetComponent<BoxCollider2D>().size = new Vector2(0.7f, 0.7f); }
        if (GM.TankList[GM.SlotTankActive].SizeTank == 1) { GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f); }
        if (GM.TankList[GM.SlotTankActive].SizeTank == 2) { GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f); }
        CheckHealth(); CheckSheild(); ResourceUpdate();
    }

    IEnumerator MachineGunBullet() //
    {
        int sec = 0;
        int msec = 0;
        while (true)
        {
            sec++;
            if (sec >= 3 & LM.ActiveMovePlayer & LM.ActiveShoot)
            {
                msec++;
                if (msec >= 5)
                {
                    sec = 0;
                    msec = 0;
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    for (int i = 0; i < GM.TankList[GM.SlotTankActive].MachineGunTanks; i++)
                    {
                        foreach (GameObject obj in P.BulletPlayerList)
                        {
                            if (!obj.activeInHierarchy)
                            {
                                obj.SetActive(true);
                                if (!GM.TankList[GM.SlotTankActive].PremTanks)
                                {
                                    obj.transform.position = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].MachineTanks[i].transform.position;
                                    obj.transform.rotation = HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].MachineTanks[i].transform.rotation;
                                }
                                else 
                                {
                                    obj.transform.position = HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].MachineTanks[i].transform.position;
                                    obj.transform.rotation = HeaderTanksPrem[GM.TankList[GM.SlotTankActive].HeaderTanks].MachineTanks[i].transform.rotation;
                                }
                                obj.GetComponent<Bullet>().LM = LevelManager.Instance;
                                MachineGunSource.Play();
                                break;
                            }
                        }
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }

    public void CheckHealth()
    {
        LifeText.text = Life.ToString();
        if (Life <= 0)
        {
            Life = 0;
            if (LM.ExtraLife) { LM.ExtraLife = false; Life = MaxLife; LifeText.text = Life.ToString(); LifeSlider.fillAmount = (float)Life / MaxLife; LM.ReturnGame = true; }
            else { if (GM.ActiveAudio) { DeadTankSource.Play(); } LM.Result(1); LM.EndGamePan[0].SetActive(true); Time.timeScale = 0; }
        }
        else{LifeSlider.fillAmount = (float)Life/MaxLife;}
    }

    public void CheckSheild()
    {
        if (Sheild > 0){SheildObj[GM.TankList[GM.SlotTankActive].SizeTank].SetActive(true); }
        else { SheildObj[GM.TankList[GM.SlotTankActive].SizeTank].SetActive(false); }
        SheildText.text = Sheild.ToString();
    }

    public void ResourceUpdate()
    {
        for(int i = 0; i < Resource.Length; i++) { ResourceText[i].text = Resource[i].ToString(); }
    }

    public void OffObject()
    {
        foreach (GameObject obj in P.ScrapList){if (obj.activeInHierarchy){obj.SetActive(false);}}
        foreach (GameObject obj in P.ScrapElectricList) { if (obj.activeInHierarchy) { obj.SetActive(false); } }
        foreach (GameObject obj in P.MoneyList) { if (obj.activeInHierarchy) { obj.SetActive(false); } }
        foreach (GameObject obj in P.HealthList) { if (obj.activeInHierarchy) { obj.SetActive(false); } }
        foreach (GameObject obj in P.GunPartList) { if (obj.activeInHierarchy) { obj.SetActive(false); } }
        foreach (GameObject obj in P.TrackPartList) { if (obj.activeInHierarchy) { obj.SetActive(false); } }
        foreach (GameObject obj in P.HeadPartList) { if (obj.activeInHierarchy) { obj.SetActive(false); } }
        foreach (GameObject obj in P.BulletEnemyList0) { if (obj.activeInHierarchy) { obj.SetActive(false); } }
        P.Diamond.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy_Bullet_0")
        {
            if (!GM.TankList[GM.SlotTankActive].PremTanks) { GM.HeadUP[GM.TankList[GM.SlotTankActive].HeaderTanks]++; if (GM.HeadUP[GM.TankList[GM.SlotTankActive].HeaderTanks] > 10 * (GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks] + 1)) { GM.UpdatePart[1] = true; } } 
            if (Sheild > 0) { Sheild--; CheckSheild(); if (GM.ActiveAudio) { SheildSource.Play(); } }
            else {
                Life -= 80; P.DamageVisual[1].transform.position = transform.position; P.DamageVisual[1].GetComponent<DamageVisual>().Damage = 80; P.DamageVisual[1].SetActive(true);
                CheckHealth(); if (GM.ActiveAudio) { DamageTankSource.Play(); }
            }
        }
        if (collision.gameObject.tag == "Enemy_Bullet_1")
        {
            GM.HeadUP[GM.TankList[GM.SlotTankActive].HeaderTanks]++;
            if (GM.HeadUP[GM.TankList[GM.SlotTankActive].HeaderTanks] > 10 * (GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks] + 1)) { GM.UpdatePart[1] = true; }
            if (Sheild > 0) { Sheild--; CheckSheild(); if (GM.ActiveAudio) { SheildSource.Play(); } }
            else
            {
                Life -= 140; P.DamageVisual[1].transform.position = transform.position; P.DamageVisual[1].GetComponent<DamageVisual>().Damage = 140; P.DamageVisual[1].SetActive(true);
                CheckHealth(); if (GM.ActiveAudio) { DamageTankSource.Play(); }
            }
        }
        if (collision.gameObject.tag == "Enemy_Bullet")
        {
            if (Sheild > 0) { Sheild--; CheckSheild(); if (GM.ActiveAudio) { SheildSource.Play(); } }
            else
            {
                Life -= 10; P.DamageVisual[1].transform.position = transform.position; P.DamageVisual[1].GetComponent<DamageVisual>().Damage = 10; P.DamageVisual[1].SetActive(true);
                CheckHealth(); if (GM.ActiveAudio) { DamageTankMachineGunSource.Play(); } 
            }
        }
        if (collision.gameObject.tag == "Enemy_Tank_Bomber")
        {
            GM.HeadUP[GM.TankList[GM.SlotTankActive].HeaderTanks]++;
            if (Sheild > 0) { Sheild--; CheckSheild(); if (GM.ActiveAudio) { SheildSource.Play(); } }
            else
            {
                Life -= 120; P.DamageVisual[1].transform.position = transform.position; P.DamageVisual[1].GetComponent<DamageVisual>().Damage = 120; P.DamageVisual[1].SetActive(true);
                CheckHealth(); if (GM.ActiveAudio) { DamageTankSource.Play(); }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Scrap")
        {
            Resource[1] += (int)(UnityEngine.Random.Range(5, 16)* (float)(1 + (0.2 * GM.LevelGameChoice))); ResourceUpdate();
            collision.gameObject.SetActive(false); LM.ResourceItemTake[1].SetActive(false); LM.ResourceItemTake[1].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
            if (GM.TaskList[5].ActiveTask) { GM.TaskList[5].Perform++; }
            if (GM.TaskList[7].ActiveTask) { GM.TaskList[7].Perform++; }
            GM.ProgressAchives[3]++;
        }
        if (collision.tag == "ScrapElectric")
        {
            Resource[2] += (int)(UnityEngine.Random.Range(3, 12) * (float)(1 + (0.2*GM.LevelGameChoice))); ResourceUpdate();
            collision.gameObject.SetActive(false); LM.ResourceItemTake[2].SetActive(false); LM.ResourceItemTake[2].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
            if (GM.TaskList[5].ActiveTask) { GM.TaskList[5].Perform++; }
            GM.ProgressAchives[3]++;
        }
        if (collision.tag == "Money")
        {
            Resource[0] += (int)(UnityEngine.Random.Range(8, 20) * (float)(1 + (0.2 * GM.LevelGameChoice))); ResourceUpdate();
            collision.gameObject.SetActive(false); LM.ResourceItemTake[0].SetActive(false); LM.ResourceItemTake[0].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
            if (GM.TaskList[5].ActiveTask) { GM.TaskList[5].Perform++; }
            if (GM.TaskList[6].ActiveTask) { GM.TaskList[6].Perform++; }
            GM.ProgressAchives[3]++;
        }
        if (collision.tag == "Diamond")
        {
            Resource[3]++; ResourceUpdate();
            collision.gameObject.SetActive(false); LM.ResourceItemTake[3].SetActive(false); LM.ResourceItemTake[3].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
            if (GM.TaskList[5].ActiveTask) { GM.TaskList[5].Perform++; }
            GM.ProgressAchives[3]++;
        }
        if (collision.tag == "Health")
        {
            int r = UnityEngine.Random.Range(18, 38) * (GM.LevelGameChoice + 1);
            if (Life + r <= MaxLife) { Life += r; CheckHealth(); }
            else { Life = MaxLife; CheckHealth(); }
            collision.gameObject.SetActive(false); LM.ResourceItemTake[4].SetActive(false); LM.ResourceItemTake[4].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
        }
        if (collision.tag == "GunPart")
        {
            GM.ScienceScore[2]++;
            collision.gameObject.SetActive(false); LM.ResourceItemTake[7].SetActive(false); LM.ResourceItemTake[7].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
        }
        if (collision.tag == "HeadPart")
        {
            GM.ScienceScore[1]++;
            collision.gameObject.SetActive(false); LM.ResourceItemTake[6].SetActive(false); LM.ResourceItemTake[6].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
        }
        if (collision.tag == "TrackPart")
        {
            GM.ScienceScore[0]++;
            collision.gameObject.SetActive(false); LM.ResourceItemTake[5].SetActive(false); LM.ResourceItemTake[5].SetActive(true);
            if (GM.ActiveAudio) { TakeItemSource.Play(); }
        }
        if (collision.tag == "EnemyRocket")
        {
            GM.HeadUP[GM.TankList[GM.SlotTankActive].HeaderTanks]++;
            if (Sheild > 0) { Sheild--; CheckSheild(); }
            else
            {
                Life -= 120; P.DamageVisual[1].transform.position = transform.position; P.DamageVisual[1].GetComponent<DamageVisual>().Damage = 120; P.DamageVisual[1].SetActive(true);
                CheckHealth(); if (GM.ActiveAudio) { DamageTankSource.Play(); }
            }
        }
        if (collision.tag == "EnemyLazer")
        {
            GM.HeadUP[GM.TankList[GM.SlotTankActive].HeaderTanks]++;
            if (Sheild > 0) { Sheild--; CheckSheild(); }
            else
            {
                Life -= 120; P.DamageVisual[1].transform.position = transform.position; P.DamageVisual[1].GetComponent<DamageVisual>().Damage = 120; P.DamageVisual[1].SetActive(true);
                CheckHealth(); if (GM.ActiveAudio) { DamageTankSource.Play(); }
            }
        }
    }

    IEnumerator TimeInBattle()
    {
        int sec = 0;
        int secA = 0;
        while (true)
        {
            sec++;
            if (sec >= 10)
            {
                secA++;
                sec = 0; GM.TrackUP[GM.TankList[GM.SlotTankActive].TrackTanks]++;
                if (secA >= 6) { GM.ProgressAchives[5]++; secA = 0; }
                if (GM.TrackUP[GM.TankList[GM.SlotTankActive].TrackTanks] > 20 * (GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks] + 1)) { GM.UpdatePart[0] = true; yield break; }
                yield return new WaitForSeconds(1);
            }
            else { yield return new WaitForSeconds(1); }
        }
    }
}

[Serializable]
public class HeaderTanks
{
    public GameObject HeadTanks;
    public GameObject[] PointHeader;
    public GameObject PointLazer;
    public GameObject PointRocket;
    public GameObject[] MachineTanks;
}
[Serializable]
public class HeaderTanksPrem
{
    public GameObject HeadTanks;
    public GameObject[] Guns;
    public GameObject Lazer;
    public GameObject[] Rocket;
    public GameObject[] MachineTanks;
    public bool PlazmGun;
    public int[] ColorTrack;
}