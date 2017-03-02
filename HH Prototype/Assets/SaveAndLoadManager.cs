using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager instance = null;

    //-----ACTUAL DATA LIST THINGS-----------------------------------------
    public PlayerSave playerSaveData;
    public List<AxeSave> axeSaveList = new List<AxeSave>();
    public List<ShovelSave> shovelSaveList = new List<ShovelSave>();
    public List<BucketSave> bucketSaveList = new List<BucketSave>();
    public List<SickleSave> sickleSaveList = new List<SickleSave>();
    public List<HammerSave> hammerSaveList = new List<HammerSave>();

    public List<ItemSave> itemSaveList = new List<ItemSave>();
    public List<LivestockSave> livestockSaveList = new List<LivestockSave>();
    public List<NPCSave> npcSaveList = new List<NPCSave>();
    public List<PlotSave> plotSaveList = new List<PlotSave>();
    public List<BuildingSave> buildingSaveList = new List<BuildingSave>();
    public List<TreeSave> treeSaveList = new List<TreeSave>();
    public List<RespawnNodeSave> respawnNodeList = new List<RespawnNodeSave>();
    public List<RockSave> rockSaveList = new List<RockSave>();
    

    //-----PREFABS-----------------------------------------------------------
    public List<GameObject> instantiateableTools = new List<GameObject>();
    public List<GameObject> instantiateableItems = new List<GameObject>();
    public List<GameObject> instantiateableLivestock = new List<GameObject>();
    //public List<GameObject> instantiateableNPCs = new List<GameObject>();
    public List<GameObject> instantiateablePlants = new List<GameObject>();
    public List<GameObject> instantiateableBuildings = new List<GameObject>();
    public List<GameObject> instantiateablePlots = new List<GameObject>();
    public GameObject       instantiateableSoil;
    public List<GameObject> instantiateableTrees = new List<GameObject>();
    public List<GameObject> instantiateableRespawnNodes = new List<GameObject>();
    public List<GameObject> instantiateableRocks = new List<GameObject>();
    

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
        //Empty List from any previous saves
        //playerSaveData = PlayerSave();
        axeSaveList = new List<AxeSave>();
        shovelSaveList = new List<ShovelSave>();
        bucketSaveList = new List<BucketSave>();
        sickleSaveList = new List<SickleSave>();
        hammerSaveList = new List<HammerSave>();

        itemSaveList = new List<ItemSave>();
        livestockSaveList = new List<LivestockSave>();
        npcSaveList = new List<NPCSave>();
        plotSaveList = new List<PlotSave>();
        buildingSaveList = new List<BuildingSave>();
        treeSaveList = new List<TreeSave>();


        //Generate new lists
        SaveEvent();

        //Put in save file
        SaveData saveData = new SaveData();
        saveData.playerSaveData = playerSaveData;
        saveData.axeSaveList = axeSaveList;
        saveData.shovelSaveList = shovelSaveList;
        saveData.bucketSaveList = bucketSaveList;
        saveData.sickleSaveList = sickleSaveList;
        saveData.hammerSaveList = hammerSaveList;
        saveData.itemSaveList = itemSaveList;
        saveData.livestockSaveList = livestockSaveList;
        saveData.npcSaveList = npcSaveList;
        saveData.plotSaveList = plotSaveList;
        saveData.buildingSaveList = buildingSaveList;
        saveData.treeSaveList = treeSaveList;
        saveData.respawnNodeList = respawnNodeList;
        saveData.rockSaveList = rockSaveList;
        
        Debug.Log("Attemptign to save " + treeSaveList.Count.ToString() + " trees");

        //Debug.Log("axeSaveList.Count = " + axeSaveList.Count);
        //Debug.Log("shovelSaveList.Count = " + shovelSaveList.Count);
        //Debug.Log("bucketSaveList.Count = " + bucketSaveList.Count);
        //Debug.Log("sickleSaveList.Count = " + sickleSaveList.Count);
        //Debug.Log("hammerSaveList.Count = " + hammerSaveList.Count);
        //Debug.Log("itemSaveList.Count = " + itemSaveList.Count);
        //Debug.Log("livestockSaveList.Count = " + livestockSaveList.Count);

        Debug.Log("Total Saved count = " + (saveData.axeSaveList.Count + saveData.shovelSaveList.Count + saveData.bucketSaveList.Count + saveData.sickleSaveList.Count
            + saveData.hammerSaveList.Count + saveData.itemSaveList.Count + saveData.livestockSaveList.Count).ToString());

        //Save Data
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("Data Path = \"" + Application.persistentDataPath + "\"");
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"));
        {            
            //Load Data
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();

            Debug.Log("Total Loaded count = " + (saveData.axeSaveList.Count + saveData.shovelSaveList.Count + saveData.bucketSaveList.Count
                + saveData.sickleSaveList.Count + saveData.hammerSaveList.Count + saveData.itemSaveList.Count + saveData.livestockSaveList.Count).ToString());



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
            BuildingIdentifier[] buildings = FindObjectsOfType<BuildingIdentifier>();
            foreach (BuildingIdentifier building in buildings)
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
            foreach (BuildingSave buildingSave in saveData.buildingSaveList)
            {
                buildingSave.LoadObject();
            }
            foreach(PlotSave plotSave in saveData.plotSaveList)
            {
                plotSave.LoadObject();
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
            Invoke("ClearNullLists", 00001);
        }
    }
    public void ClearNullLists()
    {
        PlantManager.instance.RemoveNulls();
    }
}

[System.Serializable]
class SaveData
{
    public PlayerSave playerSaveData;
    public List<AxeSave> axeSaveList = new List<AxeSave>();
    public List<ShovelSave> shovelSaveList = new List<ShovelSave>();
    public List<BucketSave> bucketSaveList = new List<BucketSave>();
    public List<SickleSave> sickleSaveList = new List<SickleSave>();
    public List<HammerSave> hammerSaveList = new List<HammerSave>();

    public List<ItemSave> itemSaveList = new List<ItemSave>();
    public List<LivestockSave> livestockSaveList = new List<LivestockSave>();
    public List<NPCSave> npcSaveList = new List<NPCSave>();
    public List<PlotSave> plotSaveList = new List<PlotSave>();
    public List<BuildingSave> buildingSaveList = new List<BuildingSave>();
    public List<TreeSave> treeSaveList = new List<TreeSave>();
    public List<RespawnNodeSave> respawnNodeList = new List<RespawnNodeSave>();
    public List<RockSave> rockSaveList = new List<RockSave>();
}


