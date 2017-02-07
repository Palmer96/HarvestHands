using UnityEngine;
using System.Collections;

public class Bucket : Tool
{
    public int currentWaterLevel = 10;
    public int maxWaterLevel = 10;
    public int waterDrain = 1;

    public GameObject waterDrop;



    // Use this for initialization
    void Start()
    {
        toolID = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("WaterSource"))
        {
            currentWaterLevel = maxWaterLevel;
        }
    }

    public override void UseTool()
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
                    if (hit.transform.GetComponent<Plant>().WaterPlant())
                        currentWaterLevel -= waterDrain;
                }
            }
        }
    }
}
