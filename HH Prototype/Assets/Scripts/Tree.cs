using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{

    public GameObject Wood;
    public int woodAvaliable;
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
        Instantiate(Wood, transform.position, transform.rotation);

        woodAvaliable--;
        if (woodAvaliable == 0)
            Destroy(gameObject);
    }
}
