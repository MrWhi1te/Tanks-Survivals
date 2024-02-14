using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : MonoBehaviour
{
    public Player PL;
    public Pool P;
    public LevelManager LM;
    public GameObject Players;

    public Vector3 RandomPoint;
    public int RandomNumber;
    public GameObject Obj;
    public int Health;
    public float Speed;
    bool Freeze;
    bool Fire;
    public TextMesh TextHealth;

    public bool Circle;
    public GameObject HeaderCircle;
    public GameObject[] GunMachine;

    // Start is called before the first frame update
    void Start()
    {
        PL = Player.Instance;
        P = Pool.Instance;
        LM = LevelManager.Instance;
        RandomNumber = Random.Range(3, 8);
        //Health *= LM.GM.LevelGameChoice + 1;
    }
    private void OnEnable()
    {
        RandomPoint = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));
        StartCoroutine(CheckPlayer());
        if (Circle) { StartCoroutine(BulletStart()); }
        HealthCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Freeze)
        {
            if (Circle) { HeaderCircle.transform.Rotate(0, 0, HeaderCircle.transform.rotation.z + 1f); }
            if (transform.position == RandomPoint) { RandomPoint = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30)); }
            else
            {
                transform.position += (RandomPoint - transform.position).normalized * Speed * Time.deltaTime;
                Obj.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - RandomPoint.y, transform.position.x - RandomPoint.x) * Mathf.Rad2Deg - 90);
            }
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
            int r = Random.Range(0, 100);
            if (r >= 25 & r <= 35){foreach (GameObject obj in P.GunPartList){ if (!obj.activeInHierarchy){obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1));break;}}}
            if (r >= 45 & r <= 55) { foreach (GameObject obj in P.HeadPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            if (r >= 65 & r <= 75) { foreach (GameObject obj in P.TrackPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1)); break; } } }
            if (LM.GM.TaskList[3].ActiveTask) { LM.GM.TaskList[3].Perform++; }
            LM.GM.ProgressAchives[2]++;
            gameObject.SetActive(false);
        }
    }

    IEnumerator CheckPlayer()
    {
        int sec = 0;
        while (true)
        {
            sec++;
            if (sec >= RandomNumber){ sec = 0; RandomPoint = Players.transform.position; yield return new WaitForSeconds(1);}
            else { yield return new WaitForSeconds(1); }
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
    IEnumerator BulletStart() //
    {
        int sec = 0;
        int msec = 0;
        while (true)
        {
            sec++;
            if (sec >= 3)
            {
                msec++;
                if (msec >= 4)
                {
                    sec = 0;
                    msec = 0;
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    foreach (GameObject obj in P.BulletEnemyList)
                    {
                        if (!obj.activeInHierarchy)
                        {
                            obj.SetActive(true);
                            obj.transform.position = GunMachine[0].transform.position;
                            obj.transform.rotation = GunMachine[0].transform.rotation;
                            break;
                        }
                    }
                    foreach (GameObject obj in P.BulletEnemyList)
                    {
                        if (!obj.activeInHierarchy)
                        {
                            obj.SetActive(true);
                            obj.transform.position = GunMachine[1].transform.position;
                            obj.transform.rotation = GunMachine[1].transform.rotation;
                            break;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
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
                P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 10; P.DamageVisual[0].SetActive(true);
                HealthCheck();
            }
        }
        if (collision.gameObject.tag == "Rocket_Tank")
        {
            if (Health > 0)
            {
                Health -= 60*(LM.GM.LevelGameChoice+1);
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
