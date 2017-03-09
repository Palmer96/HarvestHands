using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public static Blueprint instance = null;
    public GameObject currentConstruct;
    public float constructionMaxDist = 20;
    public float GridDist = 1;
    public bool inUse;
    public int selectedConstruct;
    public List<GameObject> Constructs;

    float scrollTimer;
    int rotations;

    public bool held;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("2 blue prints in scene");
    }

    // Use this for initialization
    void Start()
    {
        held = false;
        scrollTimer = 0.1f;
        if (instance == null)
            instance = this;
        //else
        //    Debug.Log("2 blue prints in scene");
        Debug.Log("Inside Blueprint Start()");
        SaveAndLoadManager.OnSave += Save;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rotations++;
        }


        if (PlayerInventory.instance.selectedItemNum == 0 && currentConstruct == null)
            ChangeSelect();


        //transform.GetChild(0).GetComponent<TextMesh>().text = Constructs[selectedConstruct].name;


        if (currentConstruct != null)
        {

            currentConstruct.SetActive(true);
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, constructionMaxDist))
            {
                //if (!hit.transform.CompareTag("Ground"))
                //{
                //    currentConstruct.GetComponent<Construct>().canBuild = false;
                //}
                currentConstruct.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                currentConstruct.transform.up = hit.normal;
                currentConstruct.transform.Rotate(0, 90 * rotations, 0);
                if (hit.transform.CompareTag("Ground"))
                {
                    currentConstruct.GetComponent<Construct>().onGround = true;
                }
                else
                {
                    currentConstruct.GetComponent<Construct>().onGround = false;
                }
            }
            else if (Physics.Raycast(currentConstruct.transform.position, -transform.up, out hit, constructionMaxDist))
            {
                currentConstruct.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                currentConstruct.transform.up = hit.normal;
            }
        }

    }
    public  void PrimaryUse()
    {
        if (currentConstruct == null)
        {
            currentConstruct = Instantiate(Constructs[selectedConstruct]);
            currentConstruct.SetActive(true);
        }
        else
        {
            ConstructionPlace();
        }
    }

    public  void SecondaryUse()
    {
        if (currentConstruct != null)
            ConstructionCancel();
    }

    Vector3 GridPos(Vector3 pos)
    {
        float x = pos.x;
        float z = pos.z;

        x = (Mathf.Round(x / GridDist)) * GridDist;
        z = (Mathf.Round(z / GridDist)) * GridDist;

        return new Vector3(x, pos.y, z);
    }


    public bool AddBuild(GameObject item)
    {
        for (int i = 0; i < Constructs.Count; i++)
        {
            if (Constructs[i] == null)
            {
                Constructs[i] = item;

                Constructs[i].layer = 2;

                Constructs[i].transform.rotation = transform.GetChild(0).rotation;
                Constructs[i].SetActive(false);
                return true;
            }
        }

        Constructs.Add(item);
        return false;
    }

    void ConstructionPlace()
    {
        if (currentConstruct.GetComponent<Construct>().canBuild)
        {
            currentConstruct.GetComponent<Construct>().Place();
            currentConstruct = null;
        }
    }

    public void ConstructionCancel()
    {
        if (currentConstruct.GetComponent<Construct>().isNew)
            Destroy(currentConstruct);
        else
            currentConstruct.GetComponent<Construct>().Cancel();

    }


   public void ChangeSelect()
    {
        if (inUse)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (selectedConstruct < Constructs.Count - 1)
                {
                    selectedConstruct++;
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (selectedConstruct > 0)
                {
                    selectedConstruct--;
                }
            }
        }
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.blueprintSave = new BlueprintSave(this);
        //Debug.Log("Saved item = " + name);
    }

}


[System.Serializable]
public class BlueprintSave
{
    int constructsCount;
    List<string> constructNames;

    public BlueprintSave(Blueprint blueprint)
    {
        constructsCount = blueprint.Constructs.Count;
        constructNames = new List<string>();
        foreach (GameObject constructPrefab in blueprint.Constructs)
        {
            Construct construct = constructPrefab.GetComponent<Construct>();
            if (construct == null)
                continue;

            constructNames.Add(construct.constructName);
        }
    }

    public GameObject LoadObject()
    {
        if (Blueprint.instance == null)
        {
            Debug.Log("Cant load Blueprint, Blueprint.instance = null");
            return null;
        }

        Blueprint.instance.Constructs = new List<GameObject>();
        foreach (string constructName in constructNames)
        {
            foreach (GameObject constructPrefabType in SaveAndLoadManager.instance.instantiateableConstructs)
            {
                Construct construct = constructPrefabType.GetComponent<Construct>();
                if (construct == null)
                    continue;

                if (constructName == construct.constructName)
                { 
                    Blueprint.instance.Constructs.Add(constructPrefabType);
                    break;
                }
            }
        }
        if (constructsCount != Blueprint.instance.Constructs.Count)
        {
            Debug.Log("Blueprint couldn't load all Constructs, Saved = " + constructsCount.ToString() + " vs Loaded = " + Blueprint.instance.Constructs.Count.ToString());
        }

        return CraftingManager.instance.gameObject;
    }
}
