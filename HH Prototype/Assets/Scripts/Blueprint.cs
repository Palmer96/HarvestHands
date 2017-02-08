using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : Tool
{
    public GameObject currentConstruct;
    public float constructionMaxDist = 20;
    public float GridDist = 1;
    public bool inUse;
    public int selectedConstruct;
    public List<GameObject> Constructs;

    float scrollTimer;
    int rotations;

    public bool held;

    // Use this for initialization
    void Start()
    {
        held = false;
        toolID = 5;
        scrollTimer = 0.1f;
    }


    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.R))
        {
            rotations++;
        }
        ChangeSelect();

        if (held)
        transform.GetChild(0).GetComponent<TextMesh>().text = Constructs[selectedConstruct].name;
           else
            transform.GetChild(0).GetComponent<TextMesh>().text = "";

        if (currentConstruct != null)
        { 
            
            currentConstruct.SetActive(true);
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, constructionMaxDist))
            {
                if (!hit.transform.CompareTag("Ground"))
                {
                    currentConstruct.GetComponent<Construct>().canBuild = false;
                }
                currentConstruct.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                currentConstruct.transform.up = hit.normal;
                currentConstruct.transform.Rotate(0, 90*rotations, 0);
            }
            else if (Physics.Raycast(currentConstruct.transform.position, -transform.up, out hit, constructionMaxDist))
            {
                currentConstruct.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                currentConstruct.transform.up = hit.normal;
            }
        }
        held = false;

    }
    public override void UseTool()
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

    public override void SecondaryToolUse()
    {
        if (currentConstruct != null)
        ConstructionCancel();
        else
        base.SecondaryToolUse();
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
        Destroy(currentConstruct);
    }


    void ChangeSelect()
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
