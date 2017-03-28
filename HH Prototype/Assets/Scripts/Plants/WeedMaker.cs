using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedMaker : Item
{
    public GameObject weed;

    // Use this for initialization
    void Start()
    {
        itemID = 302378;
        itemCap = 1;
        SaveAndLoadManager.OnSave += Save;
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

    public override void PrimaryUse()
    {

        //   if (!used)
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            //Debug.Log("Axe");
            if (Physics.Raycast(ray, out hit, rayMaxDist))
            {
                if (hit.transform.CompareTag("Soil") || hit.transform.CompareTag("Plant"))
                {
                    used = true;
                    useTimer = useRate;
                    //Get soil
                    Soil soil;
                    if (hit.transform.CompareTag("Soil"))
                        soil = hit.transform.GetComponent<Soil>();
                    else
                        soil = hit.transform.parent.GetComponent<Soil>();
                    //Add Weed
                    if (soil != null)
                        if (soil.weedInfestation == null)
                        {
                            GameObject newWeed = Instantiate(weed);
                            newWeed.GetComponent<Weed>().InfestSoil(soil);
                        }
                }
                else
                    ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
            }
        }

    }

   // public override void SecondaryUse()
   // {
   //     //   if (!used)
   //     {
   //         ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
   //         if (Physics.Raycast(ray, out hit, rayMaxDist))
   //         {
   //             if (hit.transform.CompareTag("Soil") || hit.transform.CompareTag("Plant"))
   //             {
   //                 used = true;
   //                 useTimer = useRate;
   //                 //Get soil
   //                 Soil soil;
   //                 if (hit.transform.CompareTag("Soil"))
   //                     soil = hit.transform.GetComponent<Soil>();
   //                 else
   //                     soil = hit.transform.parent.GetComponent<Soil>();
   //                 //Add Weed
   //                 if (soil != null)
   //                     if (soil.weedInfestation == null)
   //                     {
   //                         GameObject newWeed = Instantiate(weed);
   //                         newWeed.GetComponent<Weed>().InfestSoil(soil);
   //                     }
   //             }
   //             else
   //                 ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
   //         }
   //     }
   //
   // }
}
