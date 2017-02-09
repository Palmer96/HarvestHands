﻿using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    public enum ResourceType
    {
        Wood,
        Water,
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
            Build();
        }

        text.text = GetText();
    }

    void OnCollisionEnter(Collision col)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (col.transform.GetComponent<Item>() != null)
            {
                if (col.transform.GetComponent<Item>().itemName == resources[i].resource.ToString())
                {
                    if (resources[i].numRequired > resources[i].numHave)
                    {
                        int num = resources[i].numRequired - resources[i].numHave;

                        if (num < col.transform.GetComponent<Item>().quantity)
                        {
                        resources[i].numHave += num;
                        col.transform.GetComponent<Item>().DecreaseQuantity(num);
                        }
                        else
                        {
                            resources[i].numHave += col.transform.GetComponent<Item>().quantity;
                            col.transform.GetComponent<Item>().DecreaseQuantity(col.transform.GetComponent<Item>().quantity);
                        Destroy(col.gameObject);
                        }

                    }
                }
            }
        }
    }

    void Build()
    {
        Instantiate(builtVersion, transform.position, transform.rotation);
        EventManager.ConstructEvent(constructName);
        Debug.Log("Construcing event - passing in = " + constructName);
        Destroy(gameObject);
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
}
