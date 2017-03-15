using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitHome : MonoBehaviour
{

    public GameObject Rabbit;
    public float spawnTimer;
    public float spawnRate;
    // Use this for initialization
    void Start()
    {
        GameObject rabbit = Instantiate(Rabbit, transform.position, transform.rotation);
        rabbit.GetComponent<Rabbit>().home = gameObject;
        spawnTimer = spawnRate*60;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            spawnTimer = spawnRate*60;
            GameObject rabbit = Instantiate(Rabbit, transform.position, transform.rotation);
            rabbit.GetComponent<Rabbit>().home = gameObject;
        }
    }
}
