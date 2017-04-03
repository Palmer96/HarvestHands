using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        public int nameA;
        public int nameB;
    }


    public string constructName = "";
    public GameObject builtVersion;
    public List<ResourceRequired> resources;

    private Vector3 oldPosition;
    private Quaternion oldRotation;

    TextMesh text;

    bool moving;

    public GameObject Construct;
    // Use this for initialization
    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMesh>();
        SaveAndLoadManager.OnSave += Save;
        moving = false;

    }

    // Update is called once per frame
    void Update()
    {
        text.text = GetText();
    }

    public void AddResource(GameObject item)
    {

        for (int i = 0; i < resources.Count; i++)
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
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].numHave >= resources[i].numRequired)
            {
                count++;
            }
        }
        if (resources.Count == count)
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
        for (int i = 0; i < resources.Count; i++)
        {
            line += resources[i].resource + ": " + resources[i].numHave.ToString() + "/" + resources[i].numRequired.ToString() + "\n";
        }
        return line;
    }

    public void Deconstruct()
    {
        for (int i = 0; i < resources.Count; i++)
        {
            //   Debug.Log
            Debug.Log(resources[i].resource.ToString() + ": " + resources[i].numHave);
            GameObject obj = null;
            for (int k = 0; k < ResourceManager.instance.resources.Length; k++)
            {
                if (resources[i].resource.ToString() == ResourceManager.instance.resources[k].GetComponent<Item>().itemName)
                    obj = ResourceManager.instance.resources[k];
            }

            //  GameObject obj = SaveAndLoadManager.instance.instantiateableItems[i];



            if (obj != null)
            {
                GameObject item = Instantiate(obj, transform.position + transform.up * 2, transform.rotation);
                item.GetComponent<Item>().quantity = resources[i].numHave;
            }
            else
                Debug.Log("Fail");
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public virtual void Save()
    {
        //   SaveAndLoadManager.instance.saveData.buildingSaveList.Add(new BuildingSave(this));
        //Debug.Log("Saved item = " + name);
    }


    public void Move()
    {
        GameObject obj = Instantiate(Construct, transform.position, transform.rotation);
        obj.GetComponent<Construct>().isNew = false;
        obj.GetComponent<Construct>().resources = resources;
        Blueprint.instance.currentConstruct = obj;
        PlayerInventory.instance.bookOpen = true;
        Destroy(gameObject);
    }

    void Cancel()
    {
        transform.position = oldPosition;
        transform.rotation = oldRotation;
    }
}


[System.Serializable]
public class BuildingSave
{
    string constructName;
    Building.ResourceRequired[] resources;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    //    public BuildingSave(Building building)
    //    {
    //        constructName = building.constructName;
    //        resources = building.resources;
    //        posX = building.transform.position.x;
    //        posY = building.transform.position.y;
    //        posZ = building.transform.position.z;
    //        rotX = building.transform.rotation.x;
    //        rotY = building.transform.rotation.y;
    //        rotZ = building.transform.rotation.z;
    //        rotW = building.transform.rotation.w;
    //    }
    //
    //    public GameObject LoadObject()
    //    {
    //        foreach (GameObject buildingPrefabType in SaveAndLoadManager.instance.instantiateableBuildings)
    //        {
    //            Building buildingPrefab = buildingPrefabType.GetComponent<Building>();
    //            if (buildingPrefab == null)
    //                continue;
    //
    //            if (buildingPrefab.constructName == constructName)
    //            {
    //                //Debug.Log("Loading Axe");
    //                GameObject building = (GameObject)Object.Instantiate(buildingPrefabType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
    //                building.GetComponent<Building>().resources = resources;
    //                return building;
    //            }
    //        }
    //        Debug.Log("Failed to load Building, constructName = " + constructName.ToString());
    //        return null;
    //    }
}