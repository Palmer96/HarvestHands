using UnityEngine;
using System.Collections;

public class Rabbit : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent nav;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

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
}
