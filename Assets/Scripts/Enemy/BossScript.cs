using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public Player PL;
    public Pool P;
    public LevelManager LM;
    public GameObject Players;

    public int LevelBoss;
    public string NameBoss;
    int maxHealth;
    public int Health;
    public int PartCount;
    public bool PartSetActive;
    public int HealthForPart;
    public GameObject[] PartObj;
    public GameObject Head;
    public GameObject Gun;

    [Header("Gun")]
    public int Bullet;
    public float SpeedShoot;

    public Vector3 PlayerPoint;
    public bool Move;
    bool MovePoint;
    public int SecMove;
    public float Speed;

    public bool Freeze;
    bool Fire;

    [Header("UI")]
    public GameObject HealthBossPan;
    public Image HealthBossImage;
    public Text HealthBossText;
    public Text BossNameText;

    private void Awake()
    {
        P = Pool.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        PL = Player.Instance;
        LM = LevelManager.Instance;
    }
    private void OnEnable()
    {
        PlayerPoint = transform.position;
        maxHealth = Health;
        HealthBossPan.SetActive(true);
        BossNameText.text = NameBoss;
        HealthCheck();
        if (Move) { StartCoroutine(MoveCheck()); }
        StartCoroutine(Shoot());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Head.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - Players.transform.position.y, transform.position.x - Players.transform.position.x) * Mathf.Rad2Deg + 90);
        if (MovePoint){transform.position += (PlayerPoint - transform.position).normalized * Speed * Time.deltaTime;}
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            if (!Freeze)
            {
                if (Bullet == 0)
                {
                    foreach (GameObject obj in P.BulletEnemyList0)
                    {
                        if (!obj.activeInHierarchy)
                        {
                            obj.SetActive(true);
                            obj.transform.position = Gun.transform.position;
                            obj.transform.rotation = Gun.transform.rotation;
                            break;
                        }
                    }
                }
                if (Bullet == 1)
                {
                    foreach (GameObject obj in P.BulletEnemyList1)
                    {
                        if (!obj.activeInHierarchy)
                        {
                            obj.SetActive(true);
                            obj.transform.position = Gun.transform.position;
                            obj.transform.rotation = Gun.transform.rotation;
                            break;
                        }
                    }
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(1);
        }
    }
    public void HealthCheck()
    {

        HealthBossText.text = Health.ToString();
        HealthBossImage.fillAmount = (float)Health / maxHealth;
        if (PartSetActive & Health <= HealthForPart) { PartSetActive = false; for(int i = 0; i < PartObj.Length; i++) { PartObj[i].SetActive(true); PartCount++; } }
        if (Health <= 0)
        {
            if (!PL.GM.TankList[PL.GM.SlotTankActive].PremTanks) { PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks]++; if (PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] > 10 * (PL.GM.LevelGun[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] + 1)) { PL.GM.UpdatePart[2] = true; } }
            LM.CountEnemy[LM.Level]--; LM.CheckEnemy();
            P.effectEnemy[1].SetActive(false); P.effectEnemy[1].transform.position = transform.position; P.effectEnemy[1].SetActive(true);
            for(int i = 0; i < 2; i++)
            {
                foreach (GameObject obj in P.ScrapList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                        break;
                    }
                }
                foreach (GameObject obj in P.MoneyList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                        break;
                    }
                }
            }
            foreach (GameObject obj in P.ScrapElectricList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                    break;
                }
            }
            P.Diamond.SetActive(true); P.Diamond.transform.position = transform.position;
            foreach (GameObject obj in P.GunPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } 
            foreach (GameObject obj in P.HeadPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } 
            foreach (GameObject obj in P.TrackPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } 
            HealthBossPan.SetActive(false);
            LM.GM.ProgressAchives[2]++;
            if(LevelBoss == 0) { LM.GM.Metrica(2); } if (LevelBoss == 1) { LM.GM.Metrica(3); } if (LevelBoss == 2) { LM.GM.Metrica(4); } if (LevelBoss == 3) { LM.GM.Metrica(5); } if (LevelBoss == 4) { LM.GM.Metrica(6); }
            gameObject.SetActive(false);
        }
    }
    IEnumerator MoveCheck()
    {
        int sec = 0;
        while (true)
        {
            sec++;
            if (sec >= SecMove)
            {
                sec = 0;
                MovePoint = true;
                PlayerPoint = Players.transform.position;
                StartCoroutine(MoveOff());
                yield return new WaitForSeconds(1);
            }
            else { yield return new WaitForSeconds(1); }
        }
    }
    IEnumerator MoveOff()
    {
        yield return new WaitForSeconds(3);
        MovePoint = false;
        yield break;
    }
    IEnumerator FreezeOff()
    {
        Freeze = true;
        yield return new WaitForSeconds(2);
        Freeze = false;
        yield break;
    }
    IEnumerator FireOff()
    {
        Fire = true;
        int sec = 0;
        while (true)
        {
            Health -= 10;
            P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 10; P.DamageVisual[0].SetActive(true);
            HealthCheck();
            sec++;
            if (sec >= 5) { Fire = false; yield break; }
            else { yield return new WaitForSeconds(1); }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PL.Sheild > 0) { PL.Sheild--; PL.CheckSheild(); }
            else
            {
                PL.Life -= 80; PL.LifeText.text = PL.Life.ToString(); P.DamageVisual[1].transform.position = PL.transform.position; P.DamageVisual[1].GetComponent<DamageVisual>().Damage = 80; P.DamageVisual[1].SetActive(true);
                PL.CheckHealth();
            }
        }
        if (collision.gameObject.tag == "Bullet")
        {
            if (Health > 0)
            {
                if (PartCount > 0) { Health -= 1; }
                else { Health -= 10; }
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = PL.Damage; P.DamageVisual[0].SetActive(true);
                HealthCheck();
            }
        }
        if (collision.gameObject.tag == "Rocket_Tank")
        {
            if (Health > 0)
            {
                if (PartCount > 0) { Health -= 10; }
                else { Health -= 10; }
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = PL.Damage; P.DamageVisual[0].SetActive(true);
                if (LM.GM.ActiveAudio) { LM.DamageTank.Play(); }
                HealthCheck();
            }
        }
        if (collision.gameObject.tag == "Bullet_Tank_0" || collision.gameObject.tag == "Bullet_Tank_1" || collision.gameObject.tag == "Bullet_Tank_2" || collision.gameObject.tag == "Plazm_Tank")
        {
            if (Health > 0)
            {
                if (PartCount > 0) { Health -= PL.Damage / 10; }
                else { Health -= PL.Damage; }
                if (LM.ChanceFire > 0) { int r = Random.Range(0, 100); if (LM.ChanceFire >= r & !Fire) { StartCoroutine(FireOff()); } }
                if (LM.ChanceFreeze > 0) { int r = Random.Range(0, 100); if (LM.ChanceFire >= r) { StartCoroutine(FreezeOff()); } }
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = PL.Damage; P.DamageVisual[0].SetActive(true);
                if (LM.GM.ActiveAudio) { LM.DamageTank.Play(); }
                HealthCheck();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lazer_Tank")
        {
            if (Health > 0)
            {
                if (PartCount > 0) { Health -= 10; }
                else { Health -= 10; }
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = PL.Damage; P.DamageVisual[0].SetActive(true);
                if (LM.GM.ActiveAudio) { LM.DamageTank.Play(); }
                HealthCheck();
            }
        }
    }
}
