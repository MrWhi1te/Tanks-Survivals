using System.Collections;
using UnityEngine;

public class RocketManScripts : MonoBehaviour
{
    public Player PL;
    public Pool P;
    public LevelManager LM;
    public GameObject Players;

    public int SecStart;
    public int Health;
    public bool Freeze;
    bool Fire;
    public TextMesh TextHealth;

    //
    public GameObject RocketLauncher;
    public GameObject[] CircleAim;

    // Start is called before the first frame update
    void Start()
    {
        PL = Player.Instance;
        P = Pool.Instance;
        LM = LevelManager.Instance;
    }

    private void OnEnable()
    {
        StartCoroutine(StartAim());
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
            gameObject.SetActive(false);
        }
    }

    IEnumerator StartAim()
    {
        CircleAim[0].SetActive(false);
        yield return new WaitForSeconds(SecStart);
        StartCoroutine(AimPlayer());
        yield break;
    }

    IEnumerator AimPlayer()
    {
        int c = 0;
        CircleAim[0].transform.localScale = new Vector2(1.1f, 1.1f);
        CircleAim[0].SetActive(true); CircleAim[0].transform.position = Players.transform.position;
        while (true)
        {
            if(c >= 6) 
            {
                StartCoroutine(AttackPlayer());
                yield break;
            }
            else 
            { 
                CircleAim[0].transform.localScale = new Vector2(CircleAim[0].transform.localScale.x - 0.1f, CircleAim[0].transform.localScale.y - 0.1f); 
                CircleAim[0].transform.position = Players.transform.position; 
                c++;
                RocketLauncher.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - CircleAim[0].transform.position.y, transform.position.x - CircleAim[0].transform.position.x) * Mathf.Rad2Deg + 90);
                yield return new WaitForSeconds(0.5f); 
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        int c = 1; CircleAim[0].transform.position = Players.transform.position;
        CircleAim[1].transform.position = CircleAim[0].transform.position; CircleAim[1].SetActive(true);
        for (int i = 2; i < CircleAim.Length; i++) { CircleAim[i].transform.position = new Vector2(CircleAim[0].transform.position.x + Random.Range(-2, 2), CircleAim[0].transform.position.y + Random.Range(-2, 2)); CircleAim[i].SetActive(true); }
        while (true)
        {
            if (c < CircleAim.Length)
            {
                foreach (GameObject obj in P.RocketEnemyList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.transform.position = RocketLauncher.transform.position; obj.transform.rotation = RocketLauncher.transform.rotation; obj.GetComponent<EnemyRocket>().target = CircleAim[c].transform.position;
                        obj.SetActive(true);
                        c++;
                        break;
                    }
                }
                yield return new WaitForSeconds(0.2f); 
            }
            else { StartCoroutine(StartAim()); yield break; }
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
}
