using System.Collections;
using UnityEngine;

public class EnemyGunTanks : MonoBehaviour
{
    public EnemyShootMove ESM;
    public Player PL;

    public GameObject Gun;
    public bool ActiveGunMachine;
    public GameObject[] GunMachine;

    // Start is called before the first frame update
    void Start()
    {
        PL = Player.Instance;
        if (Gun != null) { StartCoroutine(Shoot()); } 
        if (ActiveGunMachine) { StartCoroutine(BulletStart()); }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (ESM.PlayerZone & !ESM.Freeze)
            {
                foreach (GameObject obj in ESM.P.BulletEnemyList0)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true);
                        obj.transform.position = Gun.transform.position;
                        obj.transform.rotation = Gun.transform.rotation;
                        break;
                    }
                }
                yield return new WaitForSeconds(2);
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator BulletStart() //
    {
        int sec = 0;
        int msec = 0;
        GunMachine[0].SetActive(true); GunMachine[1].SetActive(true);
        while (true)
        {
            sec++;
            if (sec >= 3 & ESM.LM.ActiveMovePlayer)
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
                    foreach (GameObject obj in ESM.P.BulletEnemyList)
                    {
                        if (!obj.activeInHierarchy)
                        {
                            obj.SetActive(true);
                            obj.transform.position = GunMachine[0].transform.position;
                            obj.transform.rotation = GunMachine[0].transform.rotation;
                            break;
                        }
                    }
                    foreach (GameObject obj in ESM.P.BulletEnemyList)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ESM.Players = collision.gameObject;
            ESM.PlayerZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ESM.PlayerZone = false;
        }
    }
}
