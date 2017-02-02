using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Item
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Plant"))
        {
           if( col.GetComponent<Plant>().WaterPlant())
            {
                Destroy(gameObject);
            }
        }
    }
}
