﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public int ID;
    public GameObject rock;
    public int rockAvaliable;
    public GameObject respawnObject;

    public Transform spawnPoint;
    // Use this for initialization
    void Start()
    {
        SaveAndLoadManager.OnSave += Save;
    }

    public virtual void Save()
    {
        SaveAndLoadManager.instance.saveData.rockSaveList.Add(new RockSave(this));
        //Debug.Log("Saved item = " + name);
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public void Harvest(Vector3 pos)
    {
        Instantiate(rock, pos, transform.rotation);

      //  rockAvaliable--;
     //   if (rockAvaliable == 0)
      //  {
      //      Instantiate(respawnObject, transform.position, transform.rotation);
     //       Destroy(gameObject);
      //  }
    }
}

[System.Serializable]
public class RockSave
{
    int ID;
    int rockAvailable;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    public RockSave(Rock rock)
    {
        ID = rock.ID;
        rockAvailable = rock.rockAvaliable;
        posX = rock.transform.position.x;
        posY = rock.transform.position.y;
        posZ = rock.transform.position.z;
        rotX = rock.transform.rotation.x;
        rotY = rock.transform.rotation.y;
        rotZ = rock.transform.rotation.z;
        rotW = rock.transform.rotation.w;
    }

    public GameObject LoadObject()
    {
        foreach (GameObject rockPrefabType in SaveAndLoadManager.instance.instantiateableRocks)
        {
            Rock rockPrefab = rockPrefabType.GetComponent<Rock>();
            if (rockPrefab == null)
                continue;

            if (rockPrefab.ID == ID)
            {
            //Debug.Log("Loading Bucket");
            GameObject rock = (GameObject)Object.Instantiate(rockPrefabType, new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW));
            rock.GetComponent<Rock>().rockAvaliable = rockAvailable;
            return rock;
            }
        }
        Debug.Log("Failed to load Rock, ID = " + ID.ToString());
        return null;
    }
}