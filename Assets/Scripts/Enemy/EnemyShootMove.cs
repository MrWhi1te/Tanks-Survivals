using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootMove : MonoBehaviour
{
    public Player PL;
    public Pool P;
    public LevelManager LM;

    public GameObject Players;
    public GameObject Obj;
    public int Health = 1;
    public float Speed;
    public TextMesh TextHealth;

    public GameObject[] SpawnObj;
    public bool Freeze;
    bool Fire;


    public bool PlayerZone;

    // Start is called before the first frame update
    void Start()
    {
        PL = Player.Instance;
        P = Pool.Instance;
        LM = LevelManager.Instance;
        Players = PL.gameObject;
    }
    private void OnEnable()
    {
        HealthCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Freeze)
        {
            Obj.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - Players.transform.position.y, transform.position.x - Players.transform.position.x) * Mathf.Rad2Deg + 90);
            if (!PlayerZone) { transform.position += (Players.transform.position - transform.position).normalized * Speed * Time.deltaTime; }
        }
    }

    void HealthCheck()
    {
        TextHealth.text = Health.ToString();
        if (Health <= 0)
        {
            if (!PL.GM.TankList[PL.GM.SlotTankActive].PremTanks) { PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks]++; if (PL.GM.GunUP[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] > 10 * (PL.GM.LevelGun[PL.GM.TankList[PL.GM.SlotTankActive].GunsTanks] + 1)) { PL.GM.UpdatePart[2] = true; } }
            P.effectEnemy[1].SetActive(false); P.effectEnemy[1].transform.position = transform.position; P.effectEnemy[1].SetActive(true);
            foreach (GameObject obj in P.ScrapList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x, transform.position.y+1);
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
            if(r >= 20 & r <= 45)
            {
                foreach (GameObject obj in P.HealthList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1));
                        break;
                    }
                }
            }
            int f = Random.Range(0, 100);
            if (f >= 20 & f <= 35) { foreach (GameObject obj in P.GunPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            if (f >= 40 & f <= 55) { foreach (GameObject obj in P.HeadPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            if (f >= 60 & f <= 75) { foreach (GameObject obj in P.TrackPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            if (SpawnObj != null)
            {
                for(int i = 0; i < SpawnObj.Length; i++)
                {
                    SpawnObj[i].transform.position = new Vector2(transform.position.x +Random.Range(-1,1), transform.position.y+ Random.Range(-1, 1));
                    SpawnObj[i].SetActive(true);
                    LM.CountEnemy[LM.Level]++; LM.CheckEnemy();
                }
            }
            LM.CountEnemy[LM.Level]--; LM.CheckEnemy();
            LM.GM.ProgressAchives[2]++;
            gameObject.SetActive(false);
            //Destroy(gameObject, 0.2f);
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
            if (Health > 0)
            {
                Health -= 10;
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 10; P.DamageVisual[0].SetActive(true);
                HealthCheck();
            }
            sec++;
            if (sec >= 5) { Fire = false; yield break; }
            else { yield return new WaitForSeconds(1); }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 60*(LM.GM.LevelGameChoice + 1); P.DamageVisual[0].SetActive(true);
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
}
