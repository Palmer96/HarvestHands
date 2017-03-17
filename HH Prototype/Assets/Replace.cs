using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replace : MonoBehaviour
{
    public GameObject obj;

    // Use this for initialization
    void Start()
    {
        Instantiate(obj, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
