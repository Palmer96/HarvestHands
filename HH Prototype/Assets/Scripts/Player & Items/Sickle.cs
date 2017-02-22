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
        if (used)
        {
            useTimer -= Time.deltaTime;
            if (useTimer < 0)
            {
                used = false;
            }
        }
    }

    public override void PrimaryUse(ClickType click)
    {
        switch (click)
        {
            case ClickType.Single:
                ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                Debug.Log("Sickle");
                if (Physics.Raycast(ray, out hit, rayMaxDist))
                {
                    if (hit.transform.CompareTag("Plant"))
                    {
                        hit.transform.GetComponent<Plant>().HarvestPlant(level);
                        used = true;
                        useTimer = useRate;
                    }
                }
                break;

            case ClickType.Hold:
 if (!used)
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            Debug.Log("Sickle");
            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                if (hit.transform.CompareTag("Plant"))
                {
                    hit.transform.GetComponent<Plant>().HarvestPlant(level);
                    used = true;
                    useTimer = useRate;
                }
            }
        }
                break;
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
        if (col.gameObject.CompareTag("Soil"))
        {
            Plant plant = col.transform.GetChild(0).GetComponent<Plant>();
            if (plant.readyToHarvest)
            {
                plant.HarvestPlant();
            }
        }
    }
}
