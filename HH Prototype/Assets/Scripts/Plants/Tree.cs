using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{

    public GameObject Wood;
    public int woodAvaliable;
    public GameObject stump;
    // Use this for initialization
    void Start()
    {
        SaveAndLoadManager.OnSave += Save;
    }


    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public void Harvest(Vector3 pos)
    {
        if (woodAvaliable > 0)
        {
            // Instantiate(Wood, transform.GetChild(0).position, transform.GetChild(0).rotation);
           // Instantiate(Wood, new Vector3(pos.x, 6.5f, pos.z), transform.rotation);
            Instantiate(Wood, pos, transform.rotation);

            woodAvaliable--;
            if (woodAvaliable == 0)
            {
                if (stump != null)
                    Instantiate(stump, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.treeSaveList.Add(new TreeSave(this));
    }
}

[System.Serializable]
public class TreeSave
{
    int woodAvailable;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    public TreeSave(Tree tree)
    {
        woodAvailable = tree.woodAvaliable;
        posX = tree.transform.position.x;
        posY = tree.transform.position.y;
        posZ = tree.transform.position.z;
        rotX = tree.transform.rotation.x;
        rotY = tree.transform.rotation.y;
        rotZ = tree.transform.rotation.z;
        rotW = tree.transform.rotation.w;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject treePrefabType in SaveAndLoadManager.instance.instantiateableTrees)
        {
            Tree treePrefab = treePrefabType.GetComponent<Tree>();
            if (treePrefab == null)
                continue;

            //if (treePrefab.ID == ID)
            //{
            //Debug.Log("Loading Bucket");
            GameObject tree = (GameObject)Object.Instantiate(treePrefabType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
            tree.GetComponent<Tree>().woodAvaliable = woodAvailable;
            return tree;
            //}
        }
        Debug.Log("Failed to load Tree, tree = ");// +.ToString());
        return null;
    }
}