using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent nav;
    GameObject Player;

    public float timer;
    public float timerRate;
    public bool eating;
    public GameObject plant;

    public bool isAlive;
    // Use this for initialization
    void Start()
    {
        eating = false;
        timer = timerRate;
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (eating)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (plant != null)
                {
                    plant.GetComponent<Plant>().readyToHarvest = true;
                    plant.GetComponent<Plant>().isAlive = false;
                    plant.GetComponent<Plant>().HarvestPlant();
                    plant.transform.GetComponentInParent<Soil>().occupied = false;
                    eating = false;
                }
            }
        }
        if (nav.isActiveAndEnabled)
        {
            GameObject target = FindPlant();
            if (target != null)
            {
                nav.SetDestination(target.transform.position);
            }

        }
        else
        {
            eating = false;
        }
    }

    GameObject FindPlant()
    {
        GameObject decoy = FindDecoy();
        if (decoy != null)
            return decoy;
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

    GameObject FindDecoy()
    {

        GameObject[] Plants = GameObject.FindGameObjectsWithTag("Decoy");
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
        if (closest != null)
        {
            if (Vector3.Distance(closest.transform.position, position) < 20)
                return closest;
        }
       
            return null;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Plant"))
        {
            eating = true;
            timer = timerRate;
            plant = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Plant"))
        {
            eating = false;
            timer = timerRate;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!nav.isActiveAndEnabled)
        {
            GetComponent<MeshCollider>().enabled = false;
            GameObject[] parts = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                parts[i] = transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Collider>().enabled = true;
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;

                // transform.GetChild(i).GetComponent<Rigidbody>().AddForce(/*(col.transform.position - transform.position).normalized*/ col.transform.up * 1000);
            }
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
