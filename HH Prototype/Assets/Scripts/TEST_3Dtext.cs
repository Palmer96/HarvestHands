using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_3Dtext : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         transform.rotation = Quaternion.identity;
         transform.Rotate(0,GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles.y, 0);
       // transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
    }
}
