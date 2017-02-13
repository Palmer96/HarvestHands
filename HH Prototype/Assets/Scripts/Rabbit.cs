﻿using UnityEngine;
using System.Collections;

public class Rabbit : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent nav;
    GameObject Player;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.isActiveAndEnabled)
        {
            GameObject target = FindPlant();
            if (target != null)
            {
                nav.SetDestination(target.transform.position);
            }

            //if ()
        }
        else
        {
            // GetComponent<Item>().enabled = true;
        }

     //   nav.avoidancePriority
    }

    GameObject FindPlant()
    {
        GameObject[] Plants = GameObject.FindGameObjectsWithTag("Plant");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject plt in Plants)
        {
            Vector3 diff = plt.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = plt;
                distance = curDistance;
            }
        }
        return closest;

    }

    void OnCollisionEnter(Collision col)
    {
        if (!nav.isActiveAndEnabled)
        {
        GetComponent<BoxCollider>().enabled = false;
            GameObject[] parts = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                parts[i] = transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;

                transform.GetChild(i).GetComponent<Rigidbody>().AddForce((transform.position - transform.GetChild(0).position).normalized * 1000);
            }
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
