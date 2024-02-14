using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LevelManager LM;
    
    public int speed;
    public int type;

    public GameObject effect;


    private void OnEnable()
    {
        StartCoroutine(Disabled());
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator Disabled()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
            if (type == 1){effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);}
        }
        if (collision.gameObject.tag == "Enemy_Tank_Bomber")
        {
            if (type == 1) { effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true); }
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (!LM.ReboundBullet)
            {
                if (type == 1) { effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true); }
                gameObject.SetActive(false);
            }
        }
    }
}
