using System.Collections;
using UnityEngine;

public class EnemyJumper : MonoBehaviour
{
    public Pool P;
    public LevelManager LM;
    public Player PL;


    public GameObject Players;
    public GameObject Obj;
    public GameObject ObjMove;
    public GameObject Gun;
    public int Health = 1;
    public float Speed;
    public Transform[] PointMove;
    public int SelectPoint;
    public TextMesh TextHealth;
    bool Freeze;
    bool Fire;

    private void Awake()
    {
        P = Pool.Instance;
        LM = LevelManager.Instance;
        PL = Player.Instance;
    }

    private void OnEnable()
    {
        SelectPoint = Random.Range(0, PointMove.Length);
        HealthCheck();
        StartCoroutine(BulletStart());
    }

    void FixedUpdate()
    {
        if (!Freeze)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(Players.transform.position.y - transform.position.y, Players.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90);
            ObjMove.transform.position += (PointMove[SelectPoint].position - ObjMove.transform.position).normalized * Speed * Time.deltaTime; }
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
                if (msec >= 5)
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
                            obj.transform.position = Gun.transform.position;
                            obj.transform.rotation = Gun.transform.rotation;
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
            int f = Random.Range(0, 100);
            if (f >= 20 & f <= 35) { foreach (GameObject obj in P.GunPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x + 1, transform.position.y); break; } } }
            if (f >= 40 & f <= 55) { foreach (GameObject obj in P.HeadPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x + 1, transform.position.y); break; } } }
            if (f >= 60 & f <= 75) { foreach (GameObject obj in P.TrackPartList) { if (!obj.activeInHierarchy) { obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x + 1, transform.position.y); break; } } }
            LM.GM.ProgressAchives[2]++;
            Obj.SetActive(false);
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

    void NextPoint()
    {
        int r = SelectPoint; SelectPoint = Random.Range(0, PointMove.Length);
        if(r == SelectPoint) { NextPoint(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Health -= 10;
            P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = PL.Damage; P.DamageVisual[0].SetActive(true);
            HealthCheck();
        }
        if (collision.gameObject.tag == "Lazer_Tank")
        {
            Health -= 10;
            HealthCheck();
        }
        if (collision.gameObject.tag == "Rocket_Tank")
        {
            Health -= 10;
            HealthCheck();
        }
        if (collision.gameObject.tag == "Bullet_Tank_0" || collision.gameObject.tag == "Bullet_Tank_1" || collision.gameObject.tag == "Bullet_Tank_2" || collision.gameObject.tag == "Plazm_Tank")
        {
            Health -= PL.Damage;
            if (LM.ChanceFire > 0) { int r = Random.Range(0, 100); if (LM.ChanceFire >= r & !Fire) { StartCoroutine(FireOff()); } }
            if (LM.ChanceFreeze > 0) { int r = Random.Range(0, 100); if (LM.ChanceFire >= r) { StartCoroutine(FreezeOff()); } }
            P.DamageVisual[0].transform.position = transform.position; P.DamageVisual[0].GetComponent<DamageVisual>().Damage = PL.Damage; P.DamageVisual[0].SetActive(true);
            HealthCheck();
        }
        if (collision.gameObject.tag == "Wall")
        {
            NextPoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PointJumper")
        {
            NextPoint();
        }
    }
}
