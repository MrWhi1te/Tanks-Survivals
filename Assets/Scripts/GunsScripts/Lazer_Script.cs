using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer_Script : MonoBehaviour
{
    public LevelManager LM;
    public bool GameStart;
    public GameObject LazerObj;

    // Start is called before the first frame update
    void Start()
    {
        LM = LevelManager.Instance;
        if (GameStart) { StartCoroutine(StartLazer()); }
    }

    IEnumerator StartLazer()
    {
        yield return new WaitForSeconds(3);
        if (LM.ActiveShoot) { StartCoroutine(LazerAttack()); }
        yield break;
    }
    IEnumerator LazerAttack()
    {
        if (LM.ActiveShoot)
        {
            LazerObj.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            int r = 0;
            while (true)
            {
                if (r < 360)
                {
                    r += 2;
                    transform.rotation = Quaternion.Euler(0f, 0f, r);
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    LazerObj.SetActive(false);
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    StartCoroutine(StartLazer());
                    yield break;
                }
            }
        }
        else
        {
            StartCoroutine(StartLazer());
            yield break;
        }
    }
}
