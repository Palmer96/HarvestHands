using UnityEngine;
using System.Collections;

public class Sickle : Item
{
    public int level = 1;
    // Use this for initialization
    void Start()
    {
        itemID = 6;
        itemCap = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PrimaryUse()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Debug.Log("Sickle");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Plant"))
            {
                hit.transform.GetComponent<Plant>().HarvestPlant(level);
            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Plant"))
        {
            Plant plant = col.gameObject.GetComponent<Plant>();
            if (plant.readyToHarvest)
            {
                plant.HarvestPlant();
            }
        }
    }
}
