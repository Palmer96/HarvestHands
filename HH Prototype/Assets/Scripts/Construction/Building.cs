﻿using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    public enum ResourceType
    {
        Wood,
        Rock,
        Dirt
    }
    [System.Serializable]
    public class ResourceRequired
    {
        public ResourceType resource;
        public int numRequired;
        public int numHave;
    }

    public string constructName = "";
    public GameObject builtVersion;
    public ResourceRequired[] resources;

    TextMesh text;
    // Use this for initialization
    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GetText();
    }

    public void AddResource(GameObject item)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (item.GetComponent<Item>() != null)
            {
                if (item.GetComponent<Item>().itemName == resources[i].resource.ToString())
                {
                    if (resources[i].numRequired > resources[i].numHave)
                    {
                        int num = resources[i].numRequired - resources[i].numHave;

                        if (num < item.GetComponent<Item>().quantity)
                        {
                            resources[i].numHave += num;
                            item.GetComponent<Item>().DecreaseQuantity(num);
                        }
                        else
                        {
                            resources[i].numHave += item.GetComponent<Item>().quantity;
                            item.GetComponent<Item>().DecreaseQuantity(item.GetComponent<Item>().quantity);
                            Destroy(item);
                        }

                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        AddResource(col.gameObject);
    }


    public void Build()
    {
        int count = 0;
        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i].numHave >= resources[i].numRequired)
            {
                count++;
            }
        }
        if (resources.Length == count)
        {
            GameObject build = Instantiate(builtVersion, transform.position, transform.rotation);
            build.tag = "Built";
            EventManager.ConstructEvent(constructName);
            Debug.Log("Construcing event - passing in = " + constructName);
            Destroy(gameObject);
        }



    }

    public string GetText()
    {
        string line = "";
        for (int i = 0; i < resources.Length; i++)
        {
            line += resources[i].resource + ": " + resources[i].numHave.ToString() + "/" + resources[i].numRequired.ToString() + "\n";
        }
        return line;
    }

    public void Deconstruct()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            for (int j = 0; j < resources[i].numHave; j++)
            {
                //   Debug.Log
                Debug.Log(resources[i].resource.ToString() + ": " + resources[i].numHave);

                GameObject obj = ResourceManager.instance.GetResource(resources[i].resource.ToString());
                if (obj != null)
                {
                    // if (PlayerInventory.instance.AddItem(obj))
                    Instantiate(obj, transform.position + transform.up * 2, transform.rotation);
                }
                else
                    Debug.Log("Fail");
            }
        }
        Destroy(gameObject);
    }
}
