using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{

    public GameObject Wood;
    public int woodAvaliable;
    public GameObject stump;
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
        Instantiate(Wood, transform.GetChild(0).position, transform.GetChild(0).rotation);

        woodAvaliable--;
        if (woodAvaliable == 0)
        {
            Instantiate(stump, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
