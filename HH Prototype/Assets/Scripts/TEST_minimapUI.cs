using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_minimapUI : MonoBehaviour
{

    public Transform follow;
    Camera cam;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (follow == null)
            Destroy(gameObject);     
        else
        transform.position = follow.transform.position;// new Vector3(follow.position.x,-40, follow.position.z);
        // GetComponent<RectTransform>().position = new Vector3(follow.position.x, follow.position.z, -40);
                                                       //  GetComponent<RectTransform>().position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }


}