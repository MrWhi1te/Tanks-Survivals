using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int speed;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(Dead());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        }
        if (collision.gameObject.tag == "Wall")
        {
            gameObject.SetActive(false);
            effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
        yield break;
    }
}
