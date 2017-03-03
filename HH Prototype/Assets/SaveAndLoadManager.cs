using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager instance = null;
    public SaveData saveData = new SaveData();

    //-----ACTUAL DATA LIST THINGS-----------------------------------------
    //public PlayerSave playerSaveData;
    //public List<AxeSave> axeSaveList = new List<AxeSave>();
    //public List<ShovelSave> shovelSaveList = new List<ShovelSave>();
    //public List<BucketSave> bucketSaveList = new List<BucketSave>();
    //public List<SickleSave> sickleSaveList = new List<SickleSave>();
    //public List<HammerSave> hammerSaveList = new List<HammerSave>();
    //
    //public List<ItemSave> itemSaveList = new List<ItemSave>();
    //public List<LivestockSave> livestockSaveList = new List<LivestockSave>();
    //public List<NPCSave> npcSaveList = new List<NPCSave>();
    //public List<PlotSave> plotSaveList = new List<PlotSave>();
    //public List<BuildingSave> buildingSaveList = new List<BuildingSave>();
    //public List<TreeSave> treeSaveList = new List<TreeSave>();
    //public List<RespawnNodeSave> respawnNodeList = new List<RespawnNodeSave>();
    //public List<RockSave> rockSaveList = new List<RockSave>();
    //public List<QuestSave> questSaveList = new List<QuestSave>();
    //public QuestManagerSave questManagerSave;
    //public DayNightControllerSave dayNightControllerSave;
    

    //-----PREFABS-----------------------------------------------------------
    public List<GameObject> instantiateableTools = new List<GameObject>();
    public List<GameObject> instantiateableItems = new List<GameObject>();
    public List<GameObject> instantiateableLivestock = new List<GameObject>();
    //public List<GameObject> instantiateableNPCs = new List<GameObject>();
    public List<GameObject> instantiateablePlants = new List<GameObject>();
    public List<GameObject> instantiateableBuildingIdentifiers = new List<GameObject>();
    public List<GameObject> instantiateablePlots = new List<GameObject>();
    public GameObject       instantiateableSoil;
    public List<GameObject> instantiateableTrees = new List<GameObject>();
    public List<GameObject> instantiateableRespawnNodes = new List<GameObject>();
    public List<GameObject> instantiateableRocks = new List<GameObject>();
    public List<CraftingRecipe> instantiateableCraftingRecipes = new List<CraftingRecipe>();
    public List<GameObject> instantiateableConstructs = new List<GameObject>();
    public List<GameObject> instantiateableBuildings = new List<GameObject>();
    public GameObject       instantiateableQuestBoard;


    public delegate void SaveAction();
    public static event SaveAction OnSave = delegate { };


    // Use this for initialization
    void Awake ()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
	}

    public static void SaveEvent()
    {
        //Debug.Log("Calling Save Event");
        OnSave();
    }

    public void Save()
    {
        //Create save prefab
        saveData = new SaveData();

        //Collect lists of data to save
        SaveEvent();
                
        //Save Data
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("Data Path = \"" + Application.persistentDataPath + "\"");
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {            
            //Load Data
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();

            Debug.Log("Total Loaded count = " + (saveData.questSaveList.Count).ToString());



            //Clear Pre-Existing Objects
            Item[] items = FindObjectsOfType<Item>();
            foreach(Item item in items)
            {
                item.gameObject.SetActive(false);
                Destroy(item.gameObject);
            }
            Livestock[] livestocks = FindObjectsOfType<Livestock>();
            foreach (Livestock livestock in livestocks)
            {
                livestock.gameObject.SetActive(false);
                Destroy(livestock.gameObject);
            }
            BuildingIdentifier[] buildingsIdentifiers = FindObjectsOfType<BuildingIdentifier>();
            foreach (BuildingIdentifier building in buildingsIdentifiers)
            {
                building.gameObject.SetActive(false);
                Destroy(building.gameObject);
            }
            Tree[] trees = FindObjectsOfType<Tree>();
            foreach (Tree tree in trees)
            {
                tree.gameObject.SetActive(false);
                Destroy(tree.gameObject);
            }
            RespawnNode[] respawnNodes = FindObjectsOfType<RespawnNode>();
            foreach (RespawnNode respawnNode in respawnNodes)
            {
                respawnNode.gameObject.SetActive(false);
                Destroy(respawnNode.gameObject);
            }
            Rock[] rocks = FindObjectsOfType<Rock>();
            foreach(Rock rock in rocks)
            {
                rock.gameObject.SetActive(false);
                Destroy(rock.gameObject);
            }
            Building[] buildings = FindObjectsOfType<Building>();
            foreach (Building building in buildings)
            {
                building.gameObject.SetActive(false);
                Destroy(building.gameObject);
            }
            //Debug.Log("Attemptign to destroy " + trees.Length.ToString() + " trees");


            //Assign Data
            saveData.playerSaveData.LoadObject();
            foreach (AxeSave axeSave in saveData.axeSaveList)
            {
                axeSave.LoadObject();
            }
            foreach (ShovelSave shovelSave in saveData.shovelSaveList)
            {
                shovelSave.LoadObject();
            }
            foreach (BucketSave bucketSave in saveData.bucketSaveList)
            {
                bucketSave.LoadObject();
            }
            foreach (SickleSave sickleSave in saveData.sickleSaveList)
            {
                sickleSave.LoadObject();
            }
            foreach (HammerSave hammerSave in saveData.hammerSaveList)
            {
                hammerSave.LoadObject();
            }
            foreach (ItemSave itemSave in saveData.itemSaveList)
            {
                itemSave.LoadObject();
            }
            foreach (LivestockSave livestockSave in saveData.livestockSaveList)
            {
                livestockSave.LoadObject();
            }            
            foreach (NPCSave npcSave in saveData.npcSaveList)
            {
                npcSave.LoadObject();
            }
            foreach (PlotSave plotSave in saveData.plotSaveList)
            {
                plotSave.LoadObject();
            }
            foreach (BuildingIdentifierSave buildingSave in saveData.buildingIdentifierSaveList)
            {
                buildingSave.LoadObject();
            }
            foreach (TreeSave treeSave in saveData.treeSaveList)
            {
                treeSave.LoadObject();
            }
            foreach (RespawnNodeSave respawnNodeSave in saveData.respawnNodeList)
            {
                respawnNodeSave.LoadObject();
            }
            foreach (RockSave rockSave in saveData.rockSaveList)
            {
                rockSave.LoadObject();
            }            
            foreach (QuestSave questSave in saveData.questSaveList)
            {
                questSave.LoadObject();
            }
            saveData.questManagerSave.LoadObject();
            saveData.dayNightControllerSave.LoadObject();
            foreach (SellChestSave sellChestSave in saveData.sellChestSaveList)
            {
                sellChestSave.LoadObject();
            }
            saveData.craftingRecipeManagerSave.LoadObject();
            saveData.blueprintSave.LoadObject();

            foreach (BuildingSave buildingSave in saveData.buildingSaveList)
            {
                buildingSave.LoadObject();
            }
            //Debug.Log("savedata.buildingsavelist.count = " + saveData.buildingSaveList.Count);
            Invoke("ClearNullLists", 00001);
        }
    }
    public void ClearNullLists()
    {
        PlantManager.instance.RemoveNulls();
    }
}

[System.Serializable]
public class SaveData
{
    public PlayerSave playerSaveData = null;
    public List<AxeSave> axeSaveList = new List<AxeSave>();
    public List<ShovelSave> shovelSaveList = new List<ShovelSave>();
    public List<BucketSave> bucketSaveList = new List<BucketSave>();
    public List<SickleSave> sickleSaveList = new List<SickleSave>();
    public List<HammerSave> hammerSaveList = new List<HammerSave>();

    public List<ItemSave> itemSaveList = new List<ItemSave>();
    public List<LivestockSave> livestockSaveList = new List<LivestockSave>();
    public List<NPCSave> npcSaveList = new List<NPCSave>();
    public List<PlotSave> plotSaveList = new List<PlotSave>();
    public List<BuildingIdentifierSave> buildingIdentifierSaveList = new List<BuildingIdentifierSave>();
    public List<TreeSave> treeSaveList = new List<TreeSave>();
    public List<RespawnNodeSave> respawnNodeList = new List<RespawnNodeSave>();
    public List<RockSave> rockSaveList = new List<RockSave>();
    public List<QuestSave> questSaveList = new List<QuestSave>();
    public QuestManagerSave questManagerSave = null;
    public DayNightControllerSave dayNightControllerSave = null;
    public List<SellChestSave> sellChestSaveList = new List<SellChestSave>();
    public CraftingRecipeManagerSave craftingRecipeManagerSave = null;
    public BlueprintSave blueprintSave = null;
    public List<BuildingSave> buildingSaveList = new List<BuildingSave>();
    public List<NoticeBoardSave> noticeBoardSaveList = new List<NoticeBoardSave>();
}


