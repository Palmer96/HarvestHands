using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item
{

    public int level = 1;
    public GameObject wood;

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

                Debug.Log("Axe");
                if (Physics.Raycast(ray, out hit, rayMaxDist))
                {
                    if (hit.transform.CompareTag("Tree"))
                    {
                        used = true;
                        useTimer = useRate;
                        hit.transform.GetComponent<Tree>().Harvest();
                        if (level > 1)
                            hit.transform.GetComponent<Tree>().Harvest();
                        if (level > 2)
                            hit.transform.GetComponent<Tree>().Harvest();
                        //    Instantiate(wood, hit.point, transform.rotation);
                    }
                }
                break;

            case ClickType.Hold:
                if (!used)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Axe");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Tree"))
                        {
                            used = true;
                            useTimer = useRate;
                            hit.transform.GetComponent<Tree>().Harvest();
                            if (level > 1)
                                hit.transform.GetComponent<Tree>().Harvest();
                            if (level > 2)
                                hit.transform.GetComponent<Tree>().Harvest();
                            //    Instantiate(wood, hit.point, transform.rotation);
                        }
                    }
                }
                break;
        }

    }

}
