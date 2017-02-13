﻿using UnityEngine;
using System.Collections;

public class Construct : MonoBehaviour
{
    public GameObject selfObject;
    public bool canBuild;
    public Material[] mat;

   public bool colliding;
   public bool upRight;
    public bool onGround;
    // Use this for initialization
    void Start()
    {
        mat = new Material[transform.childCount + 1];
        for (int i = 0; i < transform.childCount; i++)
        {
            mat[i] = transform.GetChild(i).GetComponent<Renderer>().material;
        }
        mat[transform.childCount] = transform.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.eulerAngles.x > 10 &&
              transform.eulerAngles.x < 350 ||
              transform.eulerAngles.z > 10 &&
              transform.eulerAngles.z < 350)
        {
            upRight = false;
        }
        else
        {
            upRight = true;
        }




        if (colliding && upRight)
            canBuild = true;
        else
            canBuild = false;



        if (canBuild)
        {
            for (int i = 0; i < mat.Length; i++)
                mat[i].color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            for (int i = 0; i < mat.Length; i++)
                mat[i].color = new Color(1, 0, 0, 0.5f);
        }
    }

    public void Place()
    {
        Instantiate(selfObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void OnTriggerStay(Collider col)
    {
            colliding = false;

        if (col.transform.CompareTag("Ground"))
            colliding = true;

            
    //  else if (col.transform.CompareTag("Ground"))
    //  {
    //      if (transform.eulerAngles.x > 10 &&
    //          transform.eulerAngles.x < 350 ||
    //          transform.eulerAngles.z > 10 &&
    //          transform.eulerAngles.z < 350)
    //      {
    //          canBuild = false;
    //      }
    //      else
    //      {
    //          canBuild = true;
    //      }
    //  }

    }
    void OnTriggerExit(Collider col)
    {
        if (col.transform.CompareTag("Ground"))
            colliding = true;
    }

}
