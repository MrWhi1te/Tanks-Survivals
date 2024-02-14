using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperCircle : MonoBehaviour
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

    public int CycleShoot;
    public GameObject Obj;
    public GameObject[] Gun;
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
            PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks]++;
            if (PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] > 10 * (PL.GM.LevelGun[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] + 1)) { PL.GM.UpdatePart[2] = true; }
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

    void GunBullet()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - Players.transform.position.y, transform.position.x - Players.transform.position.x) * Mathf.Rad2Deg + 90);
        for (int i = 0; i < Gun.Length; i++)
        {
            foreach (GameObject obj in P.BulletEnemyList1)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    obj.transform.position = Gun[i].transform.position;
                    obj.transform.rotation = Gun[i].transform.rotation;
                    break;
                }
            }
        }
    }
    void GunMachineBullet()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - Players.transform.position.y, transform.position.x - Players.transform.position.x) * Mathf.Rad2Deg + 90);
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
    IEnumerator Cycle()
    {
        int c = 0;
        while (true)
        {
            if (c < CycleShoot)
            {
                if (GunMachine.Length > 0) { GunMachineBullet(); }
                if (Gun.Length > 0) { GunBullet(); }
                c++;
                yield return new WaitForSeconds(2);
            }
            else
            {
                StartCoroutine(MoveObj()); yield break;
            }
        }
    }
    IEnumerator SizeHead(int V)
    {
        int sec = 0;
        while (true)
        {
            if (sec < 5)
            {
                if (V == 0) { transform.localScale = new Vector2(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f); sec++; }
                if (V == 1) { transform.localScale = new Vector2(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f); sec++; }
                yield return new WaitForSeconds(0.2f);
            }
            else 
            { 
                sec = 0;
                if (V == 0) { yield break; }
                if (V == 1){ gameObject.GetComponent<CircleCollider2D>().enabled = true; StartCoroutine(Cycle()); yield break;}
            }
        }
    }
    IEnumerator MoveObj()
    {
        int sec = 0;
        RandomPoint = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));
        StartCoroutine(SizeHead(0));
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        while (true)
        {
            if(sec < 200)
            {
                Obj.transform.position += (RandomPoint - transform.position).normalized * Speed * Time.deltaTime;
                sec++;
                yield return new WaitForSeconds(0.01f);
            }
            else { StartCoroutine(SizeHead(1)); yield break; }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            P.effectEnemy[0].SetActive(false); P.effectEnemy[0].transform.position = transform.position; P.effectEnemy[0].SetActive(true);
            LM.CountEnemy[LM.Level]--; LM.CheckEnemy();
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Bullet")
        {
            if (Health > 0)
            {
                Health -= 10;
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = PL.Damage; P.DamageVisual[0].SetActive(true);
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
