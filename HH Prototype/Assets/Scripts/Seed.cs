using UnityEngine;
using System.Collections;

public class Seed : MonoBehaviour
{
    public int NumberOfSeeds = 1;
    public GameObject plantPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerStay(Collider col)
    {
        if (NumberOfSeeds <= 0)
        {        
            Destroy(gameObject);
            return;
        }

        if (col.gameObject.CompareTag("Soil"))
        {
            Soil soil = col.GetComponent<Soil>();
            if (soil.occupied == false)
            {
                soil.PlantSeed(plantPrefab);
                NumberOfSeeds--;



                //TODO: if 0 seeds, play staff animation
            }
        }
    }
}
