using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_LazerPlazm : MonoBehaviour
{
    public GameObject Enemy;

    public bool GameStart;
    public bool Charg;
    
    public GameObject PrefabBullet;
    public GameObject effect;
    public int SecCharg;

    private List<GameObject> BulletList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (GameStart)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject obj = Instantiate(PrefabBullet);
                obj.SetActive(false);
                BulletList.Add(obj);
            }
            StartCoroutine(ChargBullet());
            StartCoroutine(Charging());
        }
    }

    IEnumerator Charging()
    {
        while (true)
        {
            if(Charg & Enemy != null)
            {
                Charg = false;
                foreach (GameObject obj in BulletList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true);
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - Enemy.transform.position.y, transform.position.x - Enemy.transform.position.x) * Mathf.Rad2Deg + 90);
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                        obj.GetComponent<Bullet_Rocket>().Enemy = Enemy;
                        obj.GetComponent<Bullet_Rocket>().effect = effect;
                        Enemy = null;
                        break;
                    }
                }
                StartCoroutine(ChargBullet());
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
    IEnumerator ChargBullet()
    {
        yield return new WaitForSeconds(SecCharg);
        Charg = true;
        yield break;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Enemy = collision.gameObject;
        }
        if(collision.tag == "Enemy_Tank_Bomber")
        {
            Enemy = collision.gameObject;
        }
    }
}
