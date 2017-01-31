using UnityEngine;
using System.Collections;

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

    public bool isWatered = false;
    public bool isAlive = true;
    public bool readyToHarvest = false;    

    //Stuff to do with growing
    public int dayPlanted = 0;
    public int daysToGrow = 3;
    public int dryDaysToDie = 2;
    private int dryStreak = 0;
    private int dryDays = 0;
    //multiple harvests stuff
    public float harvestsToRemove = 1;
    public int daysBetweenHarvets = 2;
    private int daysSinceLastHarvest = 0;

    public PlantState plantState = PlantState.Sapling;
    public PlantMaterial currentPlantMaterial = PlantMaterial.Growing;

    public GameObject harvestProduce;
    public Soil soil;

    public Renderer renderer;
    public MeshFilter meshFilter;
    public Mesh saplingMesh;
    public Mesh growingMesh;
    public Mesh grownMesh;
    public Mesh deadMesh;

    public Material dryMaterial;
    public Material growingMaterial;
    public Material grownMaterial;
    public Material deadMaterial;

    // Use this for initialization
    void Start ()
    {
        PlantManager.instance.AddPlant(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HarvestPlant()
    {
        if (readyToHarvest)
        {
            //If dead
            if (!isAlive)
            {
                if (soil != null)
                    soil.occupied = false;
                Destroy(gameObject);
                return;
            }
            //if alive
            else
            {
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
                if (daysBetweenHarvets > 0)
                {
                    UpdatePlantMat(PlantMaterial.Growing);
                    UpdatePlantMesh(PlantState.Growing);
                }
                else
                {
                    UpdatePlantMat(PlantMaterial.Grown);
                    UpdatePlantMesh(PlantState.Grown);
                }
            }
        }
    }

    public void UpdatePlant(int ingameDay)
    {
        //if watered
        if (isWatered)
        {
            dryStreak = 0;

            //if plant is currently NOT grown
            if (!readyToHarvest)
            {
                //if plant is now grown
                if (ingameDay >= dayPlanted + daysToGrow + dryDays)
                {
                    readyToHarvest = true;
                    UpdatePlantMesh(PlantState.Grown);
                    UpdatePlantMat(PlantMaterial.Grown);
                }
                //if plant is still growing
                else
                {
                    isWatered = false;
                    UpdatePlantMesh(PlantState.Growing);
                    UpdatePlantMat(PlantMaterial.Dry);
                }
            }
        }
        //if not watered
        else
        {
            dryStreak++;
            dryDays++;
            
            //check if plant died
            if (dryStreak >= dryDaysToDie)
            {
                readyToHarvest = true;
                isAlive = false;
                UpdatePlantMesh(PlantState.Dead);
                UpdatePlantMat(PlantMaterial.Dead);
            }
        }
    }

    void UpdatePlantMesh(PlantState newState)
    {
        plantState = newState;

        switch (plantState)
        {
            case PlantState.Dead:
                {
                    meshFilter.mesh = deadMesh;
                }
                break;
            case PlantState.Growing:
                {
                    meshFilter.mesh = growingMesh;
                }
                break;
            case PlantState.Grown:
                {
                    meshFilter.mesh = grownMesh;
                }
                break;
            case PlantState.Sapling:
                {
                    meshFilter.mesh = saplingMesh;
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

    public void WaterPlant()
    {
        isWatered = true;
        UpdatePlantMat(PlantMaterial.Growing);
    }
}
