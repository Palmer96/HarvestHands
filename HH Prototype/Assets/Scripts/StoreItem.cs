using UnityEngine;
using System.Collections;

public class StoreItem : MonoBehaviour
{
    public GameObject objectToBuy;
    public string Name;
    public int price;
    public float respawnTime = 3f;

    [Header("public for when the bug occurrs")]
    public GameObject boughtItem = null;

    // Use this for initialization
    void Start()
    {
        transform.GetChild(1).GetComponent<TextMesh>().text = Name;
        transform.GetChild(2).GetComponent<TextMesh>().text = price.ToString();
    }

    void Update()
    {
        if (boughtItem != null)
        {
            if (Vector3.Distance(transform.position, boughtItem.transform.position) > 1)
            {
                boughtItem = null;
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void BuyObject()
    {
        //if (boughtItem != null)
        //    return;

        if (price <= PlayerInventory.instance.money)
        {
            PlayerInventory.instance.money -= price;

            GameObject newObject = (GameObject)Instantiate(objectToBuy);
            newObject.transform.position = transform.position;
            GameObject.FindObjectOfType<HandTool>().PickUp(newObject);

            transform.GetChild(0).gameObject.SetActive(false);
            boughtItem = newObject;
        }

        if (boughtItem != null)
            if (Vector3.Distance(transform.position, boughtItem.transform.position) > 3)
        {
            boughtItem = null;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == boughtItem)
        {
            boughtItem = null;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
