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

    // Use this for initialization
    void Start()
    {
        toolID = 5;
        scrollTimer = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
        if (inUse)
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = Constructs[selectedConstruct].name;
        }

        if(currentConstruct != null)
            {
            currentConstruct.SetActive(true);
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(ray, out hit, constructionMaxDist))
            {
                currentConstruct.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                currentConstruct.transform.up = hit.normal;
            }
            else if (Physics.Raycast(currentConstruct.transform.position, -transform.up, out hit, constructionMaxDist))
            {
                currentConstruct.transform.position = GridPos(new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z));
                currentConstruct.transform.up = hit.normal;
            }
        }


    }

    Vector3 GridPos(Vector3 pos)
    {
        float x = pos.x;
        float z = pos.z;

        x = (Mathf.Round(x / GridDist)) * GridDist;
        z = (Mathf.Round(z / GridDist)) * GridDist;

        return new Vector3(x, pos.y, z);
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

    void ConstructionCancel()
    {
       // ConstructionMode = false;
      //  transform.GetChild(0).DetachChildren();
      //  heldItem.layer = 0;
     //   heldItem = null;
    }


    void Scroll()
    {
      //  if (scrollTimer < 0)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (selectedConstruct < Constructs.Count - 1)
                {
                    selectedConstruct++;
       //             scrollTimer = 0.1f;
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (selectedConstruct > 0)
                {
                    selectedConstruct--;
                  //  scrollTimer = 0.1f;
                }
            }
        }
    }

}
