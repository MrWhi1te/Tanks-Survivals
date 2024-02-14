using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoom : MonoBehaviour
{
    public GameObject[] SpawnEnemy;
    public Transform StartPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Spawn(int index)
    {
        SpawnEnemy[index].SetActive(true); SpawnEnemy[index].transform.position = StartPoint.position;
    }
}
