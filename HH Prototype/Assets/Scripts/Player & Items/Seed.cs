﻿using UnityEngine;
using System.Collections;

public class Seed : Item
{
    public GameObject plantPrefab;

    // Use this for initialization
    void Start()
    {
        startScale = transform.lossyScale;
        UpdateMesh();

        MinimapManager.instance.CreateImage(transform, new Color(0.1f, 1f, 1.1f));

        //  if (GetComponent<MeshFilter>().mesh != null)
        //      ownMesh = GetComponent<MeshFilter>();
        //  if (ownMesh == null)
        //      ownMesh = transform.GetChild(0).GetComponent<MeshFilter>();
        //
        //  if (GetComponent<MeshRenderer>().material != null)
        //      ownMaterial = GetComponent<MeshRenderer>().material;
        //  if (ownMaterial == null)
        //      ownMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        //
        //  if (GetComponent<MeshCollider>() != null)
        //      ownMeshCollider = GetComponent<MeshCollider>();
        //  if (ownMeshCollider == null)
        //      ownMeshCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        //
        //  if (!dontUpdate)
        //  {
        //      if (singleMesh == null)
        //          singleMesh = ownMesh.mesh;
        //      if (multiMesh == null)
        //          multiMesh = ownMesh.mesh;
        //      if (singleMaterial == null)
        //          singleMaterial = ownMaterial;
        //      if (multiMaterial == null)
        //          multiMaterial = ownMaterial;
        //  }
        //
        //  if (itemCap == 0)
        //  {
        //      itemCap = 20;
        //  }
        SaveAndLoadManager.OnSave += Save;
    }

    // Update is called once per frame
    void Update()
    {
        if (!beingHeld)
            return;
        if (moveing)
        {
            if (moveBack)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2), 0.1f);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1.6f, -0.8f, 2.5f), 0.2f);

            if (transform.localPosition.z > 2.4f)
            {
                moveBack = true;
                PrimaryUse();
            }
            if (moveBack)
            {
                if (transform.localPosition.z < 2.01f)
                {
                    transform.localPosition = new Vector3(1.6f, -0.8f, 2);
                    moveing = false;
                    moveBack = false;
                }
            }
        }
    }

    public override void Move()
    {
        base.Move();
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

 //   public override void PrimaryUse(ClickType click)
 //   {
 //       if (AttemptInteract(click))
 //           return;
 //       PrimaryUse();
 //   }
    public override void PrimaryUse()
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
                    UpdateMesh();
                }
            }
            else
            ScreenMessage.instance.CreateMessage("You cannot use " + itemName + " here");
        }
        if (quantity <= 0)
        {
            PlayerInventory.instance.DestroyItem();
        }
    }

//  public override void PrimaryUse()
//  {
//      ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
//
//      if (Physics.Raycast(ray, out hit, rayMaxDist))
//      {
//
//          if (hit.transform.CompareTag("Soil"))
//          {
//              Soil soil = hit.transform.GetComponent<Soil>();
//              Debug.Log(soil.name);
//              if (soil.occupied == false)
//              {
//                  soil.PlantSeed(plantPrefab);
//                  quantity--;
//
//                  //TODO: if 0 seeds, play staff animation
//              }
//          }
//      }
//      if (quantity <= 0)
//      {
//          PlayerInventory.instance.DestroyItem();
//      }
//  }

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
