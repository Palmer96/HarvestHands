using UnityEngine;
using System.Collections;

public class Same : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            agent.SetDestination(player.transform.position);
	}
}
