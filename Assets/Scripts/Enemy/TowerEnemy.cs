using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    public Pool P;
    public LevelManager LM;
    public Player PL;
    public GameObject Obj;
    public GameObject ObjMove;
    public GameObject Players;
    public GameObject Head;
    public GameObject Gun;

    public bool Move;
    public Transform[] PointMove;
    public int SelectPoint;
    public int Health;
    public TextMesh TextHealth;
    bool Freeze;
    bool Fire;
   


    private void Awake()
    {
        P = Pool.Instance;
        LM = LevelManager.Instance;
        PL = Player.Instance;
        Players = PL.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        HealthCheck();
        StartCoroutine(BulletStart());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Freeze)
        {
            Head.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(Players.transform.position.y - transform.position.y, Players.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90);
            if (Move)
            {
                ObjMove.transform.position += (PointMove[SelectPoint].position - ObjMove.transform.position).normalized * 1f * Time.deltaTime;
            }
        }
    }
    IEnumerator BulletStart()
    {
        while (true)
        {
            if (!Freeze)
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
                    yield return new WaitForSeconds(1);
                }
            }
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
            int r = Random.Range(0, 100);
            if (r >= 20 & r <= 45)
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PointTower")
        {
            if (SelectPoint + 1 < PointMove.Length) { SelectPoint++; }
            else { SelectPoint = 0; }
        }
    }
}
