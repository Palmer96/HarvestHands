using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject rock;
    public int rockAvaliable;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Harvest()
    {
        Instantiate(rock, transform.GetChild(0).position, transform.GetChild(0).rotation);

        rockAvaliable--;
        if (rockAvaliable == 0)
            Destroy(gameObject);
    }
}
