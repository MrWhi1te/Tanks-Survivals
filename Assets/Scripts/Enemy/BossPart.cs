using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour
{
    public BossScript BS;
    public GameObject Gun;
    public GameObject[] GunMachine;

    public int Health;
    public int MinusBoss;
    public TextMesh HealthText;
    

    // Start is called before the first frame update
    void Start()
    {
        HealthCheck();
        StartCoroutine(Shoot());
        StartCoroutine(Move());
        if (GunMachine.Length > 0) { StartCoroutine(BulletStart()); };
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    IEnumerator Move()
    {
        while (true)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - BS.Players.transform.position.y, transform.position.x - BS.Players.transform.position.x) * Mathf.Rad2Deg + 90);
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            foreach (GameObject obj in BS.P.BulletEnemyList0)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    obj.transform.position = Gun.transform.position;
                    obj.transform.rotation = Gun.transform.rotation;
                    break;
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
    void HealthCheck()
    {
        HealthText.text = Health.ToString();
        if (Health <= 0)
        {
            HealthText.text = "";
            BS.P.effectEnemy[1].SetActive(false); BS.P.effectEnemy[1].transform.position = transform.position; BS.P.effectEnemy[1].SetActive(true);
            foreach (GameObject obj in BS.P.ScrapList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                    break;
                }
            }
            foreach (GameObject obj in BS.P.MoneyList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                    break;
                }
            }
            foreach (GameObject obj in BS.P.ScrapElectricList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                    break;
                }
            }
            int r = Random.Range(0, 100);
            if (r <= 50)
            {
                foreach (GameObject obj in BS.P.HealthList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true); obj.transform.position = new Vector2(transform.position.x + 1, transform.position.y - 1);
                        break;
                    }
                }
            }
            BS.Health -= MinusBoss; BS.PartCount--; BS.HealthCheck();
            gameObject.SetActive(false);
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
                    for(int i = 0; i < GunMachine.Length; i++)
                    {
                        foreach (GameObject obj in BS.P.BulletEnemyList)
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
        if (collision.gameObject.tag == "Bullet")
        {
            if (Health > 0)
            {
                Health -= 10;
                BS.P.DamageVisual[0].transform.position = transform.position; BS.P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 10; BS.P.DamageVisual[0].SetActive(true);
                HealthCheck();
            }
        }
        if (collision.gameObject.tag == "Rocket_Tank")
        {
            if (Health > 0)
            {
                Health -= 60;
                BS.P.DamageVisual[0].transform.position = transform.position; BS.P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 60; BS.P.DamageVisual[0].SetActive(true);
                if (BS.LM.GM.ActiveAudio) { BS.LM.DamageTank.Play(); }
                HealthCheck();
            }
        }
        if (collision.gameObject.tag == "Bullet_Tank_0" || collision.gameObject.tag == "Bullet_Tank_1" || collision.gameObject.tag == "Bullet_Tank_2" || collision.gameObject.tag == "Plazm_Tank")
        {
            if (Health > 0)
            {
                Health -= BS.PL.Damage;
                BS.P.DamageVisual[0].transform.position = transform.position; BS.P.DamageVisual[0].GetComponent<DamageVisual>().Damage = BS.PL.Damage; BS.P.DamageVisual[0].SetActive(true);
                if (BS.LM.GM.ActiveAudio) { BS.LM.DamageTank.Play(); }
                HealthCheck();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lazer_Tank")
        {
            Health -= 50;
            BS.P.DamageVisual[0].transform.position = transform.position; BS.P.DamageVisual[0].GetComponent<DamageVisual>().Damage = 50; BS.P.DamageVisual[0].SetActive(true);
            if (BS.LM.GM.ActiveAudio) { BS.LM.DamageTank.Play(); }
            HealthCheck();
        }
    }
}
