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
    
    //    if (col.gameObject.CompareTag("WaterSource"))
    //    {
    //        currentWaterLevel = maxWaterLevel;
    //    }
    }

    public override void UseTool()
    {
        if (currentWaterLevel > 0)
        {
            GameObject drop = Instantiate(waterDrop, (transform.position + transform.parent.forward), transform.rotation);
            drop.GetComponent<Rigidbody>().AddForce(transform.parent.forward * 100);
            currentWaterLevel -= waterDrain;
        }
    }
}
