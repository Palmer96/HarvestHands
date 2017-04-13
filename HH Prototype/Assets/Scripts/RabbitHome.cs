using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitHome : MonoBehaviour
{

    public GameObject Rabbit;
    public float spawnTimer;
    public float spawnRate;

    public GameObject current;
    // Use this for initialization
    void Start()
    {
        GameObject rabbit = Instantiate(Rabbit, transform.position, transform.rotation);
        rabbit.GetComponent<Rabbit>().home = gameObject;
        spawnTimer = spawnRate*60;

        current = rabbit;
    }

    // Update is called once per frame
    void Update()
    {
        if (current == null)
        {

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            spawnTimer = spawnRate*60;
            GameObject rabbit = Instantiate(Rabbit, transform.position, transform.rotation);
            rabbit.GetComponent<Rabbit>().home = gameObject;
                current = rabbit;
            }
        }
    }
}
