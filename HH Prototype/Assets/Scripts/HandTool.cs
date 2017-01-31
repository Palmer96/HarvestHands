using UnityEngine;
using System.Collections;

public class HandTool : MonoBehaviour
{

    public GameObject heldItem;
    public float rayMaxDist;
    public float constructionMaxDist;
    public float constructionGridDist;


    public bool ConstructionMode;
    public GameObject Dirt;

    public float stuff;
    // Use this for initialization
    void Start()
    {
        ConstructionMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        //  if (heldItem)
        //      heldItem.transform.rotation =  transform.GetChild(0).rotation;
        // heldItem.transform.rotation = transform.GetChild(0).rotation;


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            if (!heldItem)
            {
                if (Physics.Raycast(ray, out hit, rayMaxDist))
                {
                    switch (hit.transform.tag)
                    {
                        case "Item":
                            PickUp(hit.transform.gameObject);
                            break;

                        case "Wood":
                            PickUp(hit.transform.gameObject);
                            break;

                        case "Axe":
                            PickUp(hit.transform.gameObject);
                            break;
                        case "Dirt":
                            PickUp(hit.transform.gameObject);
                            break;

                        case "Shovel":
                            PickUp(hit.transform.gameObject);
                            break;

                        case "ConstructZone":
                            ConstructionMode = true;
                            PickUp(hit.transform.gameObject);
                            break;
                        case "StoreItem":
                            hit.transform.GetComponent<StoreItem>().BuyObject();
                            Debug.Log("hit.transform.tag = StoreItem" + !heldItem);
                            break;
                        case "Bed":
                            DayNightController.instance.DayJump();
                            break;

                    }
                }
            }
            else
            {
                switch (heldItem.tag)
                {
                    case "ConstructZone":
                        if (ConstructionMode)
                            ConstructionPlace();
                        break;
                    case "Axe":
                        if (Physics.Raycast(ray, out hit, rayMaxDist))
                        {
                            if (hit.transform.CompareTag("Tree"))
                            {
                                hit.transform.GetComponent<Tree>().Harvest();
                            }
                        }
                        break;
                    case "Shovel":
                        if (Physics.Raycast(ray, out hit, rayMaxDist))
                        {
                            if (hit.transform.CompareTag("Ground"))
                            {
                                Dig(hit.point);
                            }
                        }
                        break;
                    default:
                        Drop();
                        break;

                }
            }
        }

        if (Input.GetMouseButtonDown(1) && heldItem)
        {
            if (ConstructionMode)
                ConstructionCancel();
            else
                Throw();
        }

        if (ConstructionMode)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, constructionMaxDist))
            {
                heldItem.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                heldItem.transform.up = hit.normal;
            }
            else if (Physics.Raycast(heldItem.transform.position, -transform.up, out hit, constructionMaxDist))
            {
                heldItem.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                heldItem.transform.up = hit.normal;
            }
        }

    }

    Vector3 GridPos(Vector3 pos)
    {
        float x = pos.x;
        float z = pos.z;

        x = (Mathf.Round(x / constructionGridDist)) * constructionGridDist;
        z = (Mathf.Round(z / constructionGridDist)) * constructionGridDist;

        return new Vector3(x, pos.y, z);
    }


    public void PickUp(GameObject item)
    {
        if (!heldItem)
        {
            heldItem = item;
            heldItem.transform.SetParent(transform.GetChild(0));
            heldItem.transform.localPosition = new Vector3(1, 0, 2);
            heldItem.GetComponent<Rigidbody>().isKinematic = true;

            heldItem.layer = 2;

            if (heldItem.GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            {
                //Destroy(heldItem.GetComponent<NavMeshAgent>());
                // heldItem.GetComponent<NavMeshAgent>().updatePosition = false;
                // heldItem.GetComponent<NavMeshAgent>().updateRotation = false;
                heldItem.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            }
        }
    }
    void ConstructionPlace()
    {
        if (heldItem.GetComponent<Construct>().canBuild)
        {
            heldItem.GetComponent<Construct>().Place();
            ConstructionMode = false;
            Drop();
        }
    }

    void Dig(Vector3 pos)
    {
        Instantiate(Dirt, pos, transform.rotation);
    }

    void ConstructionCancel()
    {
        ConstructionMode = false;
        transform.GetChild(0).DetachChildren();
        heldItem.layer = 0;
        heldItem = null;
    }

    void Drop()
    {
        if (heldItem)
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(0).DetachChildren();
            heldItem.layer = 0;


            heldItem = null;
        }
    }
    void Throw()
    {
        if (heldItem)
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 500, ForceMode.Force);
            transform.GetChild(0).DetachChildren();
            heldItem.layer = 0;

            heldItem = null;
        }
    }
}
