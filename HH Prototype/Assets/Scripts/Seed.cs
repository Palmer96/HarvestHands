using UnityEngine;
using System.Collections;

public class Seed : Item
{
    public GameObject plantPrefab;

    // Use this for initialization
    void Start()
    {
        if (singleMesh == null)
            singleMesh = GetComponent<MeshFilter>().mesh;
        if (multiMesh == null)
            multiMesh = GetComponent<MeshFilter>().mesh;
        if (singleMaterial == null)
            singleMaterial = GetComponent<MeshRenderer>().material;
        if (multiMaterial == null)
            multiMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UseItem()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {
            if (hit.transform.CompareTag("Soil"))
            {
                Debug.Log(hit.transform.name);
                Soil soil = hit.transform.GetComponent<Soil>();
                if (soil.occupied == false)
                {
                    quantity--;
                    soil.PlantSeed(plantPrefab);
                    EventManager.PlantEvent(plantPrefab.GetComponent<Plant>().plantName);
                    //TODO: if 0 seeds, play staff animation
                }
            }
        }
        if (quantity <= 0)
        {
            PlayerInventory.instance.DestroyItem();
        }
    }

    public override void UseItem(GameObject gameObj)
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit, rayMaxDist))
        {

            if (hit.transform.CompareTag("Soil"))
            {
                Soil soil = hit.transform.GetComponent<Soil>();
                Debug.Log(soil.name);
                if (soil.occupied == false)
                {
                    soil.PlantSeed(plantPrefab);
                    quantity--;

                    //TODO: if 0 seeds, play staff animation
                }
            }
        }
        if (quantity <= 0)
        {
            PlayerInventory.instance.DestroyItem();
        }
    }

    void OnTriggerStay(Collider col)
    {
  //      if (col.gameObject.CompareTag("Soil"))
  //      {
  //          Soil soil = col.GetComponent<Soil>();
  //          if (soil.occupied == false)
  //          {
  //              soil.PlantSeed(plantPrefab);
  //              NumberOfSeeds--;
  //          }
  //      }
  //
  //      if (quantity <= 0)
  //      {
  //          Destroy(gameObject);
  //          return;
  //      }
    }
}
