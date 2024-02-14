using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisual : MonoBehaviour
{
    private Vector2 randomVector;
    public int Damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        randomVector = new Vector2(Random.Range(-2, 2), Random.Range(1, 2));
        GetComponent<TextMesh>().text = Damage.ToString();
        StartCoroutine(Died());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(randomVector * Time.deltaTime);
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        yield break;
    }
}
