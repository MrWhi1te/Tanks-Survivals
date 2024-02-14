using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Pool Instance;

    public GameObject BulletPlayer;
    public GameObject[] BulletEnemy;
    public GameObject[] Resources;
    public GameObject[] effectEnemy;
    public GameObject[] effectPlayer;
    public GameObject[] DamageVisual;
    public GameObject Diamond;

    //Player
    public List<GameObject> BulletPlayerList = new List<GameObject>();

    //BulletEnemy
    public List<GameObject> BulletEnemyList = new List<GameObject>();
    public List<GameObject> BulletEnemyList0 = new List<GameObject>();
    public List<GameObject> RocketEnemyList = new List<GameObject>();
    public List<GameObject> BulletEnemyList1 = new List<GameObject>();
    public List<GameObject> LazerEnemyList = new List<GameObject>();

    //Resources
    public List<GameObject> ScrapList = new List<GameObject>();
    public List<GameObject> ScrapElectricList = new List<GameObject>();
    public List<GameObject> MoneyList = new List<GameObject>();
    public List<GameObject> HealthList = new List<GameObject>();
    public List<GameObject> GunPartList = new List<GameObject>();
    public List<GameObject> HeadPartList = new List<GameObject>();
    public List<GameObject> TrackPartList = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(Resources[0]);
            obj.SetActive(false); obj.transform.parent = transform;
            ScrapList.Add(obj);
            GameObject obj1 = Instantiate(Resources[1]);
            obj1.SetActive(false); obj1.transform.parent = transform;
            ScrapElectricList.Add(obj1);
            GameObject obj2 = Instantiate(Resources[2]);
            obj2.SetActive(false); obj2.transform.parent = transform;
            MoneyList.Add(obj2);
            GameObject obj3 = Instantiate(BulletPlayer);
            obj3.SetActive(false); obj3.transform.parent = transform;
            BulletPlayerList.Add(obj3);
            GameObject obj4 = Instantiate(BulletEnemy[1]);
            obj4.SetActive(false); obj4.transform.parent = transform;
            BulletEnemyList.Add(obj4); obj4.GetComponent<EnemyBullet>().effect = effectEnemy[0];
            GameObject obj5 = Instantiate(BulletEnemy[2]);
            obj5.SetActive(false); obj5.transform.parent = transform;
            RocketEnemyList.Add(obj5); obj5.GetComponent<EnemyRocket>().effect = effectEnemy[0];
        }
        for(int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(BulletEnemy[0]);
            obj.SetActive(false); obj.transform.parent = transform;
            BulletEnemyList0.Add(obj); obj.GetComponent<EnemyBullet>().effect = effectEnemy[0];
            GameObject obj1 = Instantiate(BulletEnemy[3]);
            obj1.SetActive(false); obj1.transform.parent = transform;
            BulletEnemyList1.Add(obj1); obj1.GetComponent<EnemyBullet>().effect = effectEnemy[0];
        }
        for (int i = 0; i < 8; i++)
        {
            GameObject obj1 = Instantiate(Resources[3]);
            obj1.SetActive(false); obj1.transform.parent = transform;
            HealthList.Add(obj1);
            GameObject obj2 = Instantiate(Resources[4]);
            obj2.SetActive(false); obj2.transform.parent = transform;
            GunPartList.Add(obj2);
            GameObject obj3 = Instantiate(Resources[5]);
            obj3.SetActive(false); obj3.transform.parent = transform;
            HeadPartList.Add(obj3);
            GameObject obj4 = Instantiate(Resources[6]);
            obj4.SetActive(false); obj4.transform.parent = transform;
            TrackPartList.Add(obj4);
            GameObject obj5 = Instantiate(BulletEnemy[4]);
            obj5.SetActive(false); obj5.transform.parent = transform;
            LazerEnemyList.Add(obj5); obj5.GetComponent<EnemyRocket>().effect = effectEnemy[0];
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
