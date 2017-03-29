using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour
{

    public enum State
    {
        Wander,
        Chase,
        Flee,
        FleeOnReturn,
        Eating,
        Return
    };

    public GameObject home;
    public UnityEngine.AI.NavMeshAgent nav;
    GameObject Player;

    public float timer;
    public float timerRate;
    public bool eating;
    public GameObject plant;

    public bool isAlive;

    public State state;

    public GameObject scraps;
    public bool holdingPlant = false;

    [Header("Movement")]
    public float roamRadius = 10f;
    public float minMoveTime = 5f;
    public float maxMoveTime = 10f;
    public float movementTimer = 5f;
    public float viewRadius = 20f;
    public float playerViewRadius = 10;

    
    // Use this for initialization
    void Awake()
    {
        eating = false;
        timer = timerRate;
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        state = State.Wander;

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!nav.isActiveAndEnabled)
            return;

        GameObject target = FindPlant();

        switch (state)
        {
            case State.Eating:
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    plant = FindPlant();
                    if (plant != null)
                    {
                        plant.GetComponent<Plant>().readyToHarvest = true;
                        plant.GetComponent<Plant>().isAlive = false;
                        plant.GetComponent<Plant>().HarvestPlant();
                        plant.transform.GetComponentInParent<Soil>().occupied = false;
                        state = State.Return;
                    }
                    if (Vector3.Distance(PlayerInventory.instance.transform.position, transform.position) < playerViewRadius)
                    {
                        state = State.Flee;
                    }
                }
                break;

            case State.Chase:
                if (nav.isActiveAndEnabled)
                {
                    if (target != null)
                    {
                        nav.SetDestination(target.transform.position);
                    if (Vector3.Distance(target.transform.position, transform.position) < 2)
                    {
                        state = State.Eating;
                    }
                    }
                    if (Vector3.Distance(PlayerInventory.instance.transform.position, transform.position) < playerViewRadius)
                    {
                        state = State.Flee;
                    }

                    if (target != null)
                    {
                        if (Vector3.Distance(target.transform.position, transform.position) > viewRadius)
                        {
                            state = State.Wander;
                        }
                    }
                }
                break;

            case State.Flee:
                nav.SetDestination(transform.position + (transform.position - PlayerInventory.instance.transform.position));
                if (Vector3.Distance(PlayerInventory.instance.transform.position, transform.position) > playerViewRadius)
                {
                    state = State.Wander;
                }
                break;

            case State.FleeOnReturn:
                nav.SetDestination(transform.position + (transform.position - PlayerInventory.instance.transform.position));
                if (Vector3.Distance(PlayerInventory.instance.transform.position, transform.position) > playerViewRadius)
                {
                    state = State.Return;
                }
                break;


            case State.Wander:
                movementTimer -= Time.deltaTime;
                if (movementTimer < 0)
                {
                    MoveRandomPosition();
                }
                if (target != null)
                {
                    if (Vector3.Distance(target.transform.position, transform.position) < viewRadius)
                    {
                        state = State.Chase;
                        nav.SetDestination(target.transform.position);
                    }
                }
                if (Vector3.Distance(PlayerInventory.instance.transform.position, transform.position) < playerViewRadius)
                {
                    state = State.Flee;
                }
                break;

            case State.Return:
                nav.SetDestination(home.transform.position);
                if (Vector3.Distance(home.transform.position, transform.position) < 2)
                {
                    state = State.Wander;
                }
                if (Vector3.Distance(PlayerInventory.instance.transform.position, transform.position) < playerViewRadius)
                {
                    state = State.FleeOnReturn;
                }
                break;

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

    public virtual void MoveRandomPosition()
    {
        movementTimer = Random.Range(minMoveTime, maxMoveTime);
        Vector3 moveToPos = transform.position + new Vector3(Random.Range(-roamRadius, roamRadius), 0, Random.Range(-roamRadius, roamRadius));
        nav.SetDestination(moveToPos);
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
            state = State.Eating;
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

            if (holdingPlant)
            {
                Instantiate(scraps, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
