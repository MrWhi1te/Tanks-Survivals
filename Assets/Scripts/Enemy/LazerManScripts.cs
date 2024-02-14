using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerManScripts : MonoBehaviour
{
    public Player PL;
    public Pool P;
    public LevelManager LM;
    public GameObject Players;

    public Vector3 RandomPoint;
    public int Health;
    public float Speed;
    public bool Freeze;
    bool Fire;
    public TextMesh TextHealth;

    public GameObject Obj;
    public GameObject LazerGun;
    public GameObject Lazer;
    public GameObject[] GunMachine;

    // Start is called before the first frame update
    void Start()
    {
        PL = Player.Instance;
        P = Pool.Instance;
        LM = LevelManager.Instance;
    }
    private void OnEnable()
    {
        HealthCheck();
        StartCoroutine(CheckPlayer());
        StartCoroutine(MoveObj());
    }
    IEnumerator CheckPlayer()
    {
        int sec = 0;
        while (true)
        {
            sec++;
            if (sec >= 10) { sec = 0; RandomPoint = Players.transform.position; yield return new WaitForSeconds(1); }
            else { yield return new WaitForSeconds(1); }
        }
    }
    void HealthCheck()
    {
        TextHealth.text = Health.ToString();
        if (Health <= 0)
        {
            if (!PL.GM.TankList[PL.GM.SlotTankActive].PremTanks) { PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks]++; if (PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] > 10 * (PL.GM.LevelGun[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] + 1)) { PL.GM.UpdatePart[2] = true; } }
            LM.CountEnemy[LM.Level]--; LM.CheckEnemy();
            P.effectEnemy[1].SetActive(false); P.effectEnemy[1].transform.position = transform.position; P.effectEnemy[1].SetActive(true);
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
            foreach (GameObject obj in P.ScrapElectricList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                    break;
                }
            }
            int r = Random.Range(0, 100);
            if (r >= 20 & r <= 35) { foreach (GameObject obj in P.GunPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            if (r >= 40 & r <= 55) { foreach (GameObject obj in P.HeadPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            if (r >= 60 & r <= 75) { foreach (GameObject obj in P.TrackPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            LM.GM.ProgressAchives[2]++;
            Obj.gameObject.SetActive(false);
        }
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

    void LazerShoot()
    {
        foreach (GameObject obj in P.LazerEnemyList)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.position = LazerGun.transform.position;
                obj.transform.rotation = LazerGun.transform.rotation;
                break;
            }
        }
        StartCoroutine(MoveObj());
    }
    void GunMachineBullet()
    {
        for (int i = 0; i < GunMachine.Length; i++)
        {
            foreach (GameObject obj in P.BulletEnemyList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    obj.transform.position = GunMachine[i].transform.position;
                    obj.transform.rotation = GunMachine[i].transform.rotation;
                    break;
                }
            }
        }
    }
    IEnumerator SizeAim()
    {
        Lazer.SetActive(true); Lazer.transform.localScale = new Vector2(50,100);
        int sec = 0;
        while (true)
        {
            if (sec < 5)
            {
                Lazer.transform.localScale = new Vector2(Lazer.transform.localScale.x - 10f, 100); sec++;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - Players.transform.position.y, transform.position.x - Players.transform.position.x) * Mathf.Rad2Deg + 90);
                yield return new WaitForSeconds(0.8f);
            }
            else { Lazer.transform.localScale = new Vector2(5, 100); transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - Players.transform.position.y, transform.position.x - Players.transform.position.x) * Mathf.Rad2Deg + 90); LazerShoot(); yield break; }
        }
    }
    IEnumerator MoveObj()
    {
        int sec = 0;
        RandomPoint = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));
        Lazer.SetActive(false);
        while (true)
        {
            if (sec < 200)
            {
                Obj.transform.position += (RandomPoint - transform.position).normalized * Speed * Time.deltaTime;
                sec++;
                yield return new WaitForSeconds(0.01f);
            }
            else { StartCoroutine(SizeAim()); yield break; }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (Health > 0)
            {
                Health -= 10;
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 10; P.DamageVisual[0].SetActive(true);
                HealthCheck();
            }
        }
        if (collision.gameObject.tag == "Rocket_Tank")
        {
            if (Health > 0)
            {
                Health -= 60 * (LM.GM.LevelGameChoice + 1);
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 60 * (LM.GM.LevelGameChoice + 1); P.DamageVisual[0].SetActive(true);
                if (LM.GM.ActiveAudio) { LM.DamageTank.Play(); }
                HealthCheck();
            }
        }
        if (collision.gameObject.tag == "Bullet_Tank_0" || collision.gameObject.tag == "Bullet_Tank_1" || collision.gameObject.tag == "Bullet_Tank_2" || collision.gameObject.tag == "Plazm_Tank")
        {
            if (Health > 0)
            {
                Health -= PL.Damage;
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
            Health -= 30 * (LM.GM.LevelGameChoice + 1);
            P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 30 * (LM.GM.LevelGameChoice + 1); P.DamageVisual[0].SetActive(true);
            if (LM.GM.ActiveAudio) { LM.DamageTank.Play(); }
            HealthCheck();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            RandomPoint = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Enemy_Tank_Bomber")
        {
            RandomPoint = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));
        }
    }
}
