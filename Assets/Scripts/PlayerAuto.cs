using System.Collections;
using UnityEngine;

public class PlayerAuto : MonoBehaviour
{
    public GameObject[] Enemy;
    public float[] Distance;

    public GameObject TargetEnemy;
    int Target;

    private void OnEnable()
    {
        StartCoroutine(SearchEnemy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            for (int i = 0; i < Enemy.Length; i++)
            {
                if (Enemy[i] == null) { Enemy[i] = collision.gameObject; break; }
                if (Enemy[i] != null & !Enemy[i].activeInHierarchy) { Enemy[i] = collision.gameObject; break; }
            }
        }
        if (collision.tag == "Enemy_Tank_Bomber")
        {
            for (int i = 0; i < Enemy.Length; i++)
            {
                if (Enemy[i] == null) { Enemy[i] = collision.gameObject; break; }
                if (Enemy[i] != null & !Enemy[i].activeInHierarchy) { Enemy[i] = collision.gameObject; break; }
            }
        }
    }

    void EnterEnemy()
    {
        Distance[1] = Mathf.Infinity;
        for (int i = 0; i < Enemy.Length; i++)
        {
            if (Enemy[i] != null)
            {
                if (Enemy[i].activeInHierarchy)
                {
                    Distance[0] = Vector3.Distance(Enemy[i].transform.position, transform.position);
                    if (Distance[0] < Distance[1]) { Target = i; Distance[1] = Distance[0]; }
                }
                else { Enemy[i] = null; }
            }
        }
    }

    IEnumerator SearchEnemy()
    {
        while (true)
        {
            EnterEnemy();
            if (Enemy[Target] != null) { 
                if (Enemy[Target].activeInHierarchy) { TargetEnemy = Enemy[Target]; } else { TargetEnemy = null; EnterEnemy(); }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
