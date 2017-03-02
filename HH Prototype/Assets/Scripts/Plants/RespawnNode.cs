using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNode : MonoBehaviour
{
    public int ID;
    public GameObject toSpawn;
    public int daysTill;

    // Use this for initialization
    void Start()
    {
        daysTill = Random.Range(3, 5);
        SaveAndLoadManager.OnSave += Save;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.respawnNodeList.Add(new RespawnNodeSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public void UpdateTree()
    {
        daysTill--;
            if (daysTill <= 0)
        {
            Instantiate(toSpawn, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class RespawnNodeSave
{
    int ID;
    int daysTill;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;

    public RespawnNodeSave(RespawnNode respawnNode)
    {
        ID = respawnNode.ID;
        daysTill = respawnNode.daysTill;
        posX = respawnNode.transform.position.x;
        posY = respawnNode.transform.position.y;
        posZ = respawnNode.transform.position.z;
        rotX = respawnNode.transform.rotation.x;
        rotY = respawnNode.transform.rotation.y;
        rotZ = respawnNode.transform.rotation.z;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject respawnNodeType in SaveAndLoadManager.instance.instantiateableRespawnNodes)
        {
            RespawnNode respawnNodePrefab = respawnNodeType.GetComponent<RespawnNode>();
            if (respawnNodePrefab == null)
                continue;

            if (respawnNodePrefab.ID == ID)
            {
            //Debug.Log("Loading Bucket");
            GameObject respawnNode = (GameObject)Object.Instantiate(respawnNodeType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, 0));
            respawnNode.GetComponent<RespawnNode>().daysTill = daysTill;
            return respawnNode;
            }
        }
        Debug.Log("Failed to load respawnNode, respawnNode.ID = " + ID.ToString());
        return null;
    }
}