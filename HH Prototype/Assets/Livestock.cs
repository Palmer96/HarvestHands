using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Livestock : MonoBehaviour
{
    public string animalName = "";
    public int petHappinessIncrease = 5;
    public float currentHunger = 100f;
    public float currentHappiness = 100f;
    public float maxHunger = 100f;
    public float maxHappiness = 100f;
    [Tooltip("Amount currentHunger decays per second")]
    public float hungerDecayRate = 1f;
    [Tooltip("Amount currentHappiness decays per second")]
    public float happinessDecayRate = 1f;

    [Header("Produce")]
    public GameObject produce;
    [Tooltip("Time(seconds) it takes to attempt to produce produce")]
    public float produceTimer = 100f;
    public float currentProduceTimer = 100f;

    [Range(0.0f, 100.0f)]
    public float baseProduceChance = 20f;
    [Tooltip("Increase chance to produce produce by (value * currentHappiness)")]
    public float happinessProduceChance = 1f;

    [Header("Movement")]
    public Transform bed;
    public float roamRadius = 10f;
    public float minMoveTime = 5f;
    public float maxMoveTime = 10f;
    public float movementTimer = 5f;
    public bool approachPlayer = true;
    public float playerApproachRadius = 10f;
    private bool chasingPlayer = false;

    public Transform target;
    private NavMeshAgent navMeshAgent;

    [Header("Prototype text")]
    public TextMesh foodText;
    public TextMesh happinessText;
    public TextMesh produceTimerText;
    
	// Use this for initialization
	void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentProduceTimer = produceTimer;
        if (bed == null)
            bed = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentHunger -= Time.deltaTime * hungerDecayRate;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
        foodText.text = "Hunger: " + ((int)currentHunger).ToString();
        currentHappiness -= Time.deltaTime * happinessDecayRate;
        currentHappiness = Mathf.Clamp(currentHappiness, 0f, maxHappiness);
        happinessText.text = "Happiness: " + ((int)currentHappiness).ToString();
        
        if (currentHunger > 0)
        {
            currentProduceTimer -= Time.deltaTime;
            produceTimerText.text = "Produce: " + ((int)currentProduceTimer).ToString();
            if (currentProduceTimer < 0)
            {
                ProduceProduce();
                currentProduceTimer += produceTimer;
            }
        }

        movementTimer -= Time.deltaTime;        
        if (approachPlayer)
        {
            //If player in range
            if (Vector3.Distance(PlayerInventory.instance.transform.position, transform.position) < playerApproachRadius)
            {
                chasingPlayer = true;
                navMeshAgent.SetDestination(PlayerInventory.instance.transform.position);
            }
            else
            {
                //If player moved out of chase range
                if (chasingPlayer)
                {
                    chasingPlayer = false;
                    MoveRandomPosition();
                }
                else if (movementTimer < 0)
                {
                    MoveRandomPosition();
                }
            }
        }
        //If doesnt approach player
        else if (movementTimer < 0)
        {
            MoveRandomPosition();
        }
    }

    public virtual void Interact()
    {
        IncreaseHappiness((float)petHappinessIncrease);
    }

    public virtual void Feed(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
    }

    public void IncreaseHappiness(float amount)
    {
        currentHappiness += amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0f, maxHappiness);
    }

    public virtual void Seek()
    {
        if (target == null)
            return;
        navMeshAgent.SetDestination(target.position);
    }

    public virtual void ProduceProduce()
    {
        int randNum = Random.Range(0, 100);
        if (randNum <= (baseProduceChance + (currentHappiness * happinessProduceChance)))
        {
            if (bed != null)
            {
                GameObject newObject = Instantiate(produce, bed.position, bed.rotation);
            }
            else
            {
                GameObject newObject = Instantiate(produce, transform.position, transform.rotation);
            }
        }
    }
    
    public virtual void MoveRandomPosition()
    {
        movementTimer = Random.Range(minMoveTime, maxMoveTime);
        Vector3 moveToPos = bed.position + new Vector3(Random.Range(-roamRadius, roamRadius), 0, Random.Range(-roamRadius, roamRadius));
        navMeshAgent.SetDestination(moveToPos);
    }
}
