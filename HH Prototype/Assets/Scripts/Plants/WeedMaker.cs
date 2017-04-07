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
        if (!beingHeld)
            return;
        if (moveing)
        {
            if (moveBack)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2), 0.1f);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2.5f), 0.2f);

            if (transform.localPosition.z > 2.4f)
            {
                moveBack = true;
                PrimaryUse();
            }
            if (moveBack)
            {
                if (transform.localPosition.z < 2.01f)
                {
                    transform.localPosition = new Vector3(1.6f, -0.8f, 2);
                    moveing = false;
                    moveBack = false;
                }
            }
        }
    }

    public override void PrimaryUse()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));        
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
