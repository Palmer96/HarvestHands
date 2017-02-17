using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNode : MonoBehaviour
{

    public GameObject toSpawn;
    public int daysTill;

    // Use this for initialization
    void Start()
    {
        daysTill = Random.Range(3, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTree()
    {
        daysTill--;
            if (daysTill <= 0)
        {
            Instantiate(toSpawn, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
