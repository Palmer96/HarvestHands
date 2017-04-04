using UnityEngine;
using System.Collections;

public class StoreItem : MonoBehaviour
{
    public GameObject objectToBuy;
    public GameObject displayObject;
    public string Name;
    public int price;
    public float respawnTime = 3f;

    [Header("public for when the bug occurrs")]
    public GameObject boughtItem = null;

    // Use this for initialization
    void Start()
    {
        if (displayObject == null)
            displayObject = objectToBuy;

        transform.GetChild(1).GetComponent<TextMesh>().text = Name;
        transform.GetChild(2).GetComponent<TextMesh>().text = "$" + price.ToString();


        transform.GetChild(0).GetComponent<mesh>().ShopDisplay();
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

    public GameObject BuyObject()
    {
        //if (boughtItem != null)
        //    return;

        if (price <= PlayerInventory.instance.money)
        {
            GameObject newObject = null;

            switch (objectToBuy.tag)
            {
                case "Item":
                    PlayerInventory.instance.money -= price;
                    //   newObject.transform.position = transform.position;
                    newObject = (GameObject)Instantiate(objectToBuy, transform.position + new Vector3(0, -100, 0), transform.rotation);
                    if (!PlayerInventory.instance.AddItem(newObject))
                    {
                        newObject.transform.position = transform.position + new Vector3(-1.5f, 0, 0);
                    }
                    break;

               // case "ConstructZone":
               //     if (PlayerInventory.instance.HasBook())
               //     {
               //         PlayerInventory.instance.money -= price;
               //
               //         PlayerInventory.instance.AddBlueprint(objectToBuy);
               //     }
               //     break;
            }

            //  GameObject.FindObjectOfType<HandTool>().PickUp(newObject);

            // transform.GetChild(0).gameObject.SetActive(false);
            boughtItem = newObject;
            return newObject;
        }

        if (boughtItem != null)
            if (Vector3.Distance(transform.position, boughtItem.transform.position) > 3)
            {
                boughtItem = null;
                transform.GetChild(0).gameObject.SetActive(true);
            }

        return null;
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
