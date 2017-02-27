using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance = null;
    public GameObject[] resources;


    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetResource(string item)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i].GetComponent<Item>().itemName == item)
            {
                return resources[i];
            }
        }
        return null;
    }
}
