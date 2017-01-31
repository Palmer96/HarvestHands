using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tool : MonoBehaviour
{

    public GameObject produce;
    float rayMaxDist = 5;

    RaycastHit hit;
    Ray ray;

    public enum ToolType
    {
        Hand,
        Axe,
        Bucket,
        Scythe,
        Shovel
    }


    delegate void ToolFunc();
    ToolFunc useTool;

    public ToolType type;
    // Use this for initialization
    void Start()
    {
        switch (type)
        {
            case ToolType.Hand:
                useTool = Hand;
                break;
            case ToolType.Axe:
                useTool = Axe;
                break;
            case ToolType.Bucket:
                useTool = Bucket;
                break;
            case ToolType.Scythe:
                useTool = Scythe;
                break;
            case ToolType.Shovel:
                useTool = Shovel;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Hand() { }

    public void UseTool()
    {

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        useTool();
    }


    void Axe()
    {
        Debug.Log("Axe");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Tree"))
            {
                hit.transform.GetComponent<Tree>().Harvest();
                Instantiate(produce, hit.transform.position, transform.rotation);
            }
        }
    }
    void Bucket()
    {
        Debug.Log("Bucket");
        //if (Physics.Raycast(ray, out hit))
        {
            GameObject water = Instantiate(produce, (transform.position + transform.parent.forward), transform.rotation);
            water.GetComponent<Rigidbody>().AddForce(transform.parent.forward * 100);
        }
    }
    void Scythe()
    {
        Debug.Log("Scythe");

    }
    void Shovel()
    {
        Debug.Log("Shovel");
        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                Instantiate(produce, hit.point, transform.rotation);
            }
        }
    }



}
