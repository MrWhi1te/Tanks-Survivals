using System.Collections;
using UnityEngine;

public class Bullet_Rocket : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject effect;

    private void OnEnable()
    {
        StartCoroutine(Died());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += (Enemy.transform.position - transform.position).normalized * 4f * Time.deltaTime;
    }
    IEnumerator Died()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
        effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        yield break;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
            effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        }
        if (collision.gameObject.tag == "Enemy_Tank_Bomber")
        {
            gameObject.SetActive(false);
            effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        }
    }
}
