using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    //TODO Make UpdatePlant work with multiple harvests (daysSinceLastHarvest)
    public enum PlantState
    {
        Sapling,
        Growing,
        Grown,
        Dead,
    }

    public enum PlantMaterial
    {
        Dry,
        Growing,
        Grown,
        Dead,
    }

    public string plantName = "";

    public int strengthTimer;
    public GameObject finishedShine;
    public bool isWatered = false;
    public bool isAlive = true;
    public bool readyToHarvest = false;

    //Stuff to do with growing
    // public int dayPlanted = 0;
    // public int daysToGrow = 3;
    // public int dryDaysToDie = 2;
    // private int dryStreak = 0;
    // private int dryDays = 0;
    //multiple harvests stuff
    public float harvestsToRemove = 1;
    // public int daysBetweenHarvets = 2;
    // private int daysSinceLastHarvest = 0;

    public PlantState plantState = PlantState.Sapling;
    public PlantMaterial currentPlantMaterial = PlantMaterial.Growing;

    public GameObject harvestProduce;
    public Soil soil;

    public Renderer renderer;
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public Mesh saplingMesh;
    public Mesh growingMesh;
    public Mesh grownMesh;
    public Mesh deadMesh;

    public Material dryMaterial;
    public Material growingMaterial;
    public Material grownMaterial;
    public Material deadMaterial;

    public float waterLevel;
    public float lowWater;
    public float highWater;
    public float maxWater;

    public float dryMultiplier = 1;
    public float slowTimeMultiplier = 1;
    public float harvestTimer = 20;

    public bool highlighted;

    bool particleCreated = false;

    public float deathTimer = 1;
    public float deathTimerRate = 1;

    public float saplingMeshDuration = 1;

    private float startTime;
    public float sliderWidth;
    // Use this for initialization
    void Start()
    {
        PlantManager.instance.AddPlant(this);
        UpdatePlant(1);
        transform.GetChild(1).GetChild(0).GetComponent<Slider>().maxValue = maxWater;
        startTime = harvestTimer;
        //SaveAndLoadManager.OnSave += Save;

        //Get sliders width
        sliderWidth = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta.x;

        //Low Water Zone
        RectTransform rectTrans = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>();
        float rectTransSize = (lowWater / maxWater) * sliderWidth;
        float rectRansPos = rectTransSize / 2;
        rectTrans.anchoredPosition = new Vector3(rectRansPos, 0, 0);
        rectTrans.sizeDelta = new Vector2(rectTransSize, 7);

        //High Water Zone
        rectTrans = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        rectTransSize = ((maxWater - highWater) / maxWater) * sliderWidth;
        rectRansPos = -(rectTransSize / 2); //cuz anchors
        rectTrans.anchoredPosition = new Vector3(rectRansPos, 0, 0);
        rectTrans.sizeDelta = new Vector2(rectTransSize, 7);


        //transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2((lowWater / maxWater) * sliderWidth, 7);
        //transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector3(transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition.x + (lowWater / maxWater) * sliderWidth / 2, 0, 0);
        //
        //sliderWidth = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        //transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(((maxWater - highWater) / maxWater) * sliderWidth, 7);
        //transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition.x + ((maxWater - highWater) / maxWater) * sliderWidth / 2, 0, 0);


        //transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition += new Vector3(0, 10, 0);
        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        //Dead plants
        if (!isAlive)
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = "";
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            transform.parent.GetComponent<MeshRenderer>().enabled = false;
            return;
        }
        //Ready to harvest plants
        if (readyToHarvest)
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = "";
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            transform.parent.GetComponent<MeshRenderer>().enabled = false;
            return;
        }

        harvestTimer -= DayNightController.instance.timePast;
        waterLevel -= DayNightController.instance.timePast * dryMultiplier;

        UpdateStatus();                
    }


    void UpdatePlants()
    {
        switch (plantState)
        {
            case PlantState.Dead:
                {
                    meshFilter.mesh = deadMesh;
                    meshCollider.sharedMesh = deadMesh;
                }
                break;
            case PlantState.Growing:
                {
                    meshFilter.mesh = growingMesh;
                    meshCollider.sharedMesh = growingMesh;
                }
                break;
            case PlantState.Grown:
                {
                    meshFilter.mesh = grownMesh;
                    meshCollider.sharedMesh = grownMesh;
                }
                break;
            case PlantState.Sapling:
                {
                    meshFilter.mesh = saplingMesh;
                    meshCollider.sharedMesh = saplingMesh;
                }
                break;
            default:
                {
                    Debug.Log(name + " Plant script tried to .UpdatePlantMesh with enum: " + plantState);
                }
                break;
        }

        switch (currentPlantMaterial)
        {
            case PlantMaterial.Dead:
                {
                    renderer.material = deadMaterial;

                }
                break;
            case PlantMaterial.Dry:
                {
                    renderer.material = dryMaterial;
                }
                break;
            case PlantMaterial.Growing:
                {
                    renderer.material = growingMaterial;
                }
                break;
            case PlantMaterial.Grown:
                {
                    renderer.material = grownMaterial;
                }
                break;
            default:
                {
                    Debug.Log(name + " Plant script tried to .UpdatePlantMat with enum: " + currentPlantMaterial);
                }
                break;
        }
    }

    public void TimeJump(float time)
    {
        harvestTimer -= time;
        waterLevel -= time;
        saplingMeshDuration -= time;
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        //If still sapling, check if need to grow up
        if (saplingMeshDuration > 0)
        {
            saplingMeshDuration -= DayNightController.instance.timePast;
            if (saplingMeshDuration <= 0)
            {
                plantState = PlantState.Growing;
            }
        }

        //If low water
        if (waterLevel < lowWater)
        {
            //If dried to death
            if (waterLevel < 0)
            {
                isAlive = false;
                plantState = PlantState.Dead;
                currentPlantMaterial = PlantMaterial.Dead;
            }
            else
            {
                currentPlantMaterial = PlantMaterial.Dry;
            }
        }
        //If high water
        else if (waterLevel > highWater)
        {
            //If drowned
            if (waterLevel > maxWater)
            {
                isAlive = false;
                plantState = PlantState.Dead;
                currentPlantMaterial = PlantMaterial.Dead;
            }
        }
        //If normal Water
        else
        {
            currentPlantMaterial = PlantMaterial.Grown;
        }

        if (isAlive)
        {
            //If ready to harvest
            if (harvestTimer < 0)
            {
                //Create harvest particles
                particleCreated = true;
                GameObject particle = Instantiate(finishedShine, transform.position, finishedShine.transform.rotation);
                particle.transform.SetParent(transform);

                //Update mesh and stuff
                readyToHarvest = true;
                plantState = PlantState.Grown;
                currentPlantMaterial = PlantMaterial.Grown;
                transform.GetChild(0).GetComponent<TextMesh>().text = "";
            }
        }

        if (highlighted)
        {
            highlighted = false;
            transform.GetChild(0).GetComponent<TextMesh>().text = ((int)waterLevel).ToString();
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).GetChild(0).GetComponent<Slider>().value = waterLevel;
            transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = harvestTimer / startTime;

            transform.parent.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = "";
            transform.GetChild(1).gameObject.SetActive(false);
            transform.parent.GetComponent<MeshRenderer>().enabled = false;
        }

        //Update plant visuals
        UpdatePlants();
    }


    public void HarvestPlant()
    {
        if (!isAlive)
        {
            if (soil != null)
                soil.occupied = false;
            Destroy(gameObject);
            return;
        }
        else if (readyToHarvest)
        {
            EventManager.HarvestEvent(plantName);

            harvestsToRemove--;

            //if creates produce
            if (harvestProduce != null)
            {
                GameObject produce = (GameObject)Instantiate(harvestProduce);
                produce.transform.position = transform.position;
                produce.transform.position += new Vector3(0, 1, 0);
            }

            if (harvestsToRemove <= 0)
            {
                isAlive = false;
                if (soil != null)
                    soil.occupied = false;
                Destroy(gameObject);
            }

            //multiple times harvestable stuff
            //     if (daysBetweenHarvets > 0)
            //     {
            //         UpdatePlantMat(PlantMaterial.Growing);
            //         UpdatePlantMesh(PlantState.Growing);
            //     }
            //     else
            //     {
            //         UpdatePlantMat(PlantMaterial.Grown);
            //         UpdatePlantMesh(PlantState.Grown);
            //     }
        }
    }


    public void HarvestPlant(int level)
    {
        if (!isAlive)
        {
            if (soil != null)
                soil.occupied = false;
            Destroy(gameObject);
            return;
        }
        else if (readyToHarvest)
        {
            EventManager.HarvestEvent(plantName);

            harvestsToRemove--;

            //if creates produce
            if (harvestProduce != null)
            {
                GameObject produce = (GameObject)Instantiate(harvestProduce, transform.position, transform.rotation);
                produce.transform.position = transform.position;
                produce.transform.position += new Vector3(0, 1, 0);
                if (level > 1)
                {
                    GameObject produce2 = (GameObject)Instantiate(harvestProduce, transform.position + transform.up, transform.rotation);
                    produce2.transform.position = transform.position;
                    produce2.transform.position += new Vector3(0, 1, 0);
                }
                if (level > 2)
                {
                    GameObject produce3 = (GameObject)Instantiate(harvestProduce, transform.position + (transform.up * 2), transform.rotation);
                    produce3.transform.position = transform.position;
                    produce3.transform.position += new Vector3(0, 1, 0);
                }
            }

            if (harvestsToRemove <= 0)
            {
                isAlive = false;
                if (soil != null)
                    soil.occupied = false;
                Destroy(gameObject);
            }

            //multiple times harvestable stuff
            //  if (daysBetweenHarvets > 0)
            //  {
            //      UpdatePlantMat(PlantMaterial.Growing);
            //      UpdatePlantMesh(PlantState.Growing);
            //  }
            //  else
            //  {
            //      UpdatePlantMat(PlantMaterial.Grown);
            //      UpdatePlantMesh(PlantState.Grown);
            //  }
        }
    }


    public void UpdatePlant(int ingameDay)
    {
        //if watered
        // if (isWatered)
        //  {
        //   dryStreak = 0;

        //if plant is currently NOT grown
        //    if (!readyToHarvest)
        //   {
        //if plant is now grown
        //    if (ingameDay >= dayPlanted + daysToGrow + dryDays)
        //    {
        //        readyToHarvest = true;
        //        UpdatePlantMesh(PlantState.Grown);
        //        UpdatePlantMat(PlantMaterial.Grown);
        //    }
        //if plant is still growing
        //        else
        //        {
        //            isWatered = false;
        //            UpdatePlantMesh(PlantState.Growing);
        //            UpdatePlantMat(PlantMaterial.Dry);
        //        }
        //    }
        // }
        //if not watered
        // else
        //  {
        //  dryStreak++;
        //  dryDays++;
        //
        //  //check if plant died
        //  if (dryStreak >= dryDaysToDie)
        //  {
        //      readyToHarvest = true;
        //      isAlive = false;
        //      if (plantState != PlantState.Sapling)
        //      {
        //
        //          UpdatePlantMesh(PlantState.Dead);
        //          UpdatePlantMat(PlantMaterial.Dead);
        //      }
        //      GetComponent<Renderer>().material.color = Color.red;
        //  }
        //  }
    }

    void UpdatePlantMesh(PlantState newState)
    {
        plantState = newState;

        switch (plantState)
        {
            case PlantState.Dead:
                {
                    meshFilter.mesh = deadMesh;
                    meshCollider.sharedMesh = deadMesh;
                }
                break;
            case PlantState.Growing:
                {
                    meshFilter.mesh = growingMesh;
                    meshCollider.sharedMesh = growingMesh;
                }
                break;
            case PlantState.Grown:
                {
                    meshFilter.mesh = grownMesh;
                    meshCollider.sharedMesh = grownMesh;
                }
                break;
            case PlantState.Sapling:
                {
                    meshFilter.mesh = saplingMesh;
                    meshCollider.sharedMesh = saplingMesh;
                }
                break;
            default:
                {
                    Debug.Log(name + " Plant script tried to .UpdatePlantMesh with enum: " + newState);
                }
                break;
        }
    }

    void UpdatePlantMat(PlantMaterial newMaterial)
    {
        currentPlantMaterial = newMaterial;
        Debug.Log("newmaterial = " + newMaterial);

        switch (newMaterial)
        {
            case PlantMaterial.Dead:
                {
                    renderer.material = deadMaterial;

                }
                break;
            case PlantMaterial.Dry:
                {
                    renderer.material = dryMaterial;
                }
                break;
            case PlantMaterial.Growing:
                {
                    renderer.material = growingMaterial;
                }
                break;
            case PlantMaterial.Grown:
                {
                    renderer.material = grownMaterial;
                }
                break;
            default:
                {
                    Debug.Log(name + " Plant script tried to .UpdatePlantMat with enum: " + newMaterial);
                }
                break;
        }
    }

    public bool WaterPlant(float water)
    {
        if (isAlive)
        {
            waterLevel += water;
            //    if (!isWatered)
            //   {
            isWatered = true;
            // UpdatePlantMat(PlantMaterial.Growing);
            GetComponent<Renderer>().material.color = Color.green;
            return true;
            //    }
        }
        return false;
        //   return false;
    }

    public bool RainPlant(float water)
    {
        if (isAlive)
        {
            if (waterLevel < highWater)
            {

                waterLevel += water;
                isWatered = true;
                GetComponent<Renderer>().material.color = Color.green;
                return true;
            }
        }
        return false;
    }

    /* Plot Covers soil and plant saving
    public override void Save()
    {
        SaveAndLoadManager.instance.plotSaveList.Add(new PlotSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }
    */
}

[System.Serializable]
public class PlantSave
{
    string plantName;
    float waterLevel;
    float harvestTimer;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;

    public PlantSave(Plant plant)
    {
        plantName = plant.plantName;
        waterLevel = plant.waterLevel;
        harvestTimer = plant.harvestTimer;
        posX = plant.transform.position.x;
        posY = plant.transform.position.y;
        posZ = plant.transform.position.z;
        rotX = plant.transform.rotation.x;
        rotY = plant.transform.rotation.y;
        rotZ = plant.transform.rotation.z;
    }

    public GameObject LoadObject(Transform parent = null)
    {
        foreach (GameObject plantPrefabType in SaveAndLoadManager.instance.instantiateablePlants)
        {
            Plant plantPrefab = plantPrefabType.GetComponent<Plant>();
            if (plantPrefab == null)
                continue;

            if (plantPrefab.plantName == plantName)
            {
                //Debug.Log("Loading Axe");
                GameObject plant = (GameObject)Object.Instantiate(plantPrefabType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, 0));
                Plant newPlant = plant.GetComponent<Plant>();
                newPlant.waterLevel = waterLevel;
                newPlant.harvestTimer = harvestTimer;
                if (parent != null)
                {
                    plant.transform.SetParent(parent);
                    plant.transform.position = new Vector3(posX, posY, posZ);
                    plant.transform.rotation = new Quaternion(rotX, rotY, rotZ, 0);
                }
                return plant;
            }
        }
        Debug.Log("Failed to load Plant, plantName = " + plantName.ToString());
        return null;
    }
}