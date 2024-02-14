using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocket : MonoBehaviour
{

    public int speed;
    public GameObject effect;
    public Vector3 target;

    public bool Lazer;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(transform.position.y - target.y, transform.position.x - target.x) * Mathf.Rad2Deg + 90);
        StartCoroutine(Dead());
    }
    // Update is called once per frame
    void Update()
    {
        if (Lazer){transform.Translate(Vector3.up * speed * Time.deltaTime);}
        else
        {
            if (transform.position == target) { gameObject.SetActive(false); effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true); }
            else { transform.position += (target - transform.position).normalized * speed * Time.deltaTime; }
        }
    }
    IEnumerator Dead()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        yield break;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "AimRocket")
        {
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        }
        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            effect.SetActive(false); effect.transform.position = transform.position; effect.transform.rotation = transform.rotation; effect.SetActive(true);
        }
    }
}
