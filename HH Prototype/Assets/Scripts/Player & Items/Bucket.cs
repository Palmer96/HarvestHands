using UnityEngine;
using System.Collections;

public class Bucket : Item
{
    public int level = 1;
    public int currentWaterLevel = 10;
    public int maxWaterLevel = 10;
    public int waterDrain = 3;

    public GameObject waterDrop;



    // Use this for initialization
    void Start()
    {
        itemID = 5;
        itemCap = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(1).GetComponent<TextMesh>().text = currentWaterLevel.ToString();
        if (used)
        {
            useTimer -= Time.deltaTime;
            if (useTimer < 0)
            {
                used = false;
            }
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("WaterSource"))
        {
            currentWaterLevel = maxWaterLevel;
        }
    }


    public override void PrimaryUse(ClickType click)
    {
        switch (click)
        {
            case ClickType.Single:
                ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                if (Physics.Raycast(ray, out hit, rayMaxDist))
                {
                    if (hit.transform.CompareTag("WaterSource"))
                        currentWaterLevel = maxWaterLevel;
                    else if (hit.transform.CompareTag("Plant"))
                    {
                        if (currentWaterLevel > 0)
                        {
                            if (hit.transform.GetComponent<Plant>().WaterPlant(waterDrain))
                            {
                                currentWaterLevel -= waterDrain;
                                EventManager.WaterEvent(hit.transform.GetComponent<Plant>().plantName.ToString());
                                used = true;
                                useTimer = useRate;
                            }
                        }
                    }
                    else if (hit.transform.CompareTag("Soil"))
                    {
                        if (currentWaterLevel > 0)
                        {
                            if (hit.transform.GetChild(0).GetComponent<Plant>().WaterPlant(waterDrain))
                            {
                                currentWaterLevel -= waterDrain;
                                EventManager.WaterEvent(hit.transform.GetComponent<Plant>().plantName.ToString());
                                used = true;
                                useTimer = useRate;
                            }
                        }
                    }
                }
                break;

            case ClickType.Hold:
                if (!used)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("WaterSource"))
                            currentWaterLevel = maxWaterLevel;
                        else if (hit.transform.CompareTag("Plant"))
                        {
                            if (currentWaterLevel > 0)
                            {
                                if (hit.transform.GetComponent<Plant>().WaterPlant(waterDrain))
                                {
                                    currentWaterLevel -= waterDrain;
                                    EventManager.WaterEvent(hit.transform.GetComponent<Plant>().plantName.ToString());
                                    used = true;
                                    useTimer = useRate;
                                }
                            }
                        }
                    }
                }
                break;
        }

    }


    public override void PrimaryUse()
    {
        if (!used)
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                if (hit.transform.CompareTag("WaterSource"))
                    currentWaterLevel = maxWaterLevel;
                else if (hit.transform.CompareTag("Plant"))
                {
                    if (currentWaterLevel > 0)
                    {
                        if (hit.transform.GetComponent<Plant>().WaterPlant(waterDrain))
                        {
                            currentWaterLevel -= waterDrain;
                            EventManager.WaterEvent(hit.transform.GetComponent<Plant>().plantName.ToString());
                            used = true;
                            useTimer = useRate;
                        }
                    }
                }
            }
        }
    }
}
