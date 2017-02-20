using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Livestock : MonoBehaviour
{
    public string animalName = "";
    public float currentHunger = 100f;
    public float currentHappiness = 100f;
    public int petHappinessIncrease = 5;
    public float maxHunger = 100f;
    public float hungerDecayRate = 1f;
    //public float hungerDecayAmount = 1f;
    public float maxHappiness = 100f;
    public float happinessDecayRate = 1f;
    //public float happinessDecayAmount = 1f;

    public Transform target;
    private NavMeshAgent navMeshAgent;
    public GameObject produce;
    
	// Use this for initialization
	void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentHunger -= Time.deltaTime * hungerDecayRate;
        currentHunger = Mathf.Clamp(currentHappiness, 0f, maxHunger);
        currentHappiness -= Time.deltaTime * happinessDecayRate;
        currentHappiness = Mathf.Clamp(currentHappiness, 0f, maxHappiness);
    }

    public void Interact()
    {
        IncreaseHappiness((float)petHappinessIncrease);
    }

    public void Feed(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHappiness, 0f, maxHunger);
    }

    public void IncreaseHappiness(float amount)
    {
        currentHappiness += amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0f, maxHappiness);
    }

    public void Seek()
    {
        if (target == null)
            return;
        navMeshAgent.SetDestination(target.position);
    }

    public void ProduceProduce()
    {
        GameObject newObject = Instantiate(produce, transform.position, transform.rotation);
    }
}
