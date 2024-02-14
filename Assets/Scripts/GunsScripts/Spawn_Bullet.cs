using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Bullet : MonoBehaviour
{
    public LevelManager LM;

    public bool GameStart;
    public float SecShoot;
    public GameObject PrefabBullet;

    public GameObject effect;
    public GameObject Target;
    public GameObject ShootEffect;

    private List<GameObject> BulletList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        LM = LevelManager.Instance;
        if (GameStart)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject obj = Instantiate(PrefabBullet);
                obj.SetActive(false);
                BulletList.Add(obj);
            }
            StartCoroutine(Shoot());
            //if(LM.GM.DeviceGame == "mobile") { if (Target != null) { Target.SetActive(true); } }
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    IEnumerator ExtraShoot()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject obj in BulletList)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.GetComponent<Bullet>().effect = effect;
                obj.GetComponent<Bullet>().LM = LevelManager.Instance;
                ShootEffect.SetActive(false);ShootEffect.SetActive(true);
                if (LM.GM.ActiveAudio) { LM.PL.ShootBulletSource.Play(); }
                break;
            }
        }
        yield break;
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            if (LM.ActiveShoot)
            {
                foreach (GameObject obj in BulletList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.SetActive(true);
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                        obj.GetComponent<Bullet>().effect = effect;
                        obj.GetComponent<Bullet>().LM = LevelManager.Instance;
                        ShootEffect.SetActive(false); ShootEffect.SetActive(true);
                        if (LM.GM.ActiveAudio) { LM.PL.ShootBulletSource.Play(); }
                        break;
                    }
                }
                if (LM.ExtraShoot)
                {
                    StartCoroutine(ExtraShoot());
                }
                yield return new WaitForSeconds(SecShoot-LM.SpeedShoot);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
}
