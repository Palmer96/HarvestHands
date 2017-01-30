using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour
{
    public GameObject Object1;
    public GameObject Object2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {





        if (Input.GetKeyDown(KeyCode.O))
            Instantiate(Object2, new Vector3(0, 0, 0), transform.rotation);

        if (Input.GetKeyDown(KeyCode.P))
            Instantiate(Object1, new Vector3(0, 0.25f, -12), transform.rotation);
    }
}
