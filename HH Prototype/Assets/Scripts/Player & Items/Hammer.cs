using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Item
{

    public int level = 1;

    // Use this for initialization
    void Start()
    {
        itemID = 3;
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

                Debug.Log("Hammer");
                if (Physics.Raycast(ray, out hit, rayMaxDist))
                {
                    if (hit.transform.CompareTag("Building"))
                    {
                        Debug.Log("Building");
                        used = true;
                        useTimer = useRate;
                        hit.transform.GetComponent<Building>().Build();
                    }
                }
                break;

            case ClickType.Hold:
                // if (!used)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Hammer");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        switch (hit.transform.tag)
                        {
                            case "BUilding":
                                used = true;
                                useTimer = useRate;
                                hit.transform.GetComponent<Building>().Deconstruct();
                                break;
                            case "Built":
                                Destroy(hit.transform.gameObject);
                                break;
                        }
                        if (hit.transform.CompareTag("Building"))
                        {

                        }
                    }
                }
                break;
        }

    }

}
