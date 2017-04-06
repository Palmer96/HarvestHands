using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class MovementTracker : MonoBehaviour
{
    [SerializeField]
    public Vector3[] pos;
    public float timer;
    public float rate = 2;
    int count;
    public GameObject Dot;
    int posSize;

    // Use this for initialization
    void Start()
    {
        timer = rate;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = rate;
            //   pos.Add(new Vector2(PlayerInventory.instance.transform.position.x, PlayerInventory.instance.transform.position.z));
            Vector3[] newpos = new Vector3[pos.Length + 1];
            for (int i = 0; i < pos.Length; i++)
            {
                newpos[i] = pos[i];
            }
            pos = newpos;
            posSize = pos.Length;
            pos[posSize - 1] = (PlayerInventory.instance.transform.position);
            // Instantiate(Dot, new Vector3(PlayerInventory.instance.transform.position.x, (int)PlayerInventory.instance.transform.position.y, PlayerInventory.instance.transform.position.z), Quaternion.identity);
            count++;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Load();
        }

    }

    void OnApplicationClose()
    {
        Save();
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        SaveData data = new SaveData();

        data.posSize = posSize;

        data.posX = new float[posSize];
        data.posY = new float[posSize];
        data.posZ = new float[posSize];

        for (int i = 0; i < posSize; i++)
        {
            data.posX[i] = pos[i].x;
            data.posY[i] = pos[i].y;
            data.posZ[i] = pos[i].z;
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            posSize = data.posSize;
            
            pos = new Vector3[posSize];


            for (int i = 0; i < posSize; i++)
            {

                pos[i].x = data.posX[i];
                pos[i].y = data.posY[i];
                pos[i].z = data.posZ[i];
            }
            //pos.Add(new Vector3(data.posX, data.posY, data.posZ));
            //  pos.Clear();
            // pos.Add(new Vector3(data.posX[i], data.posY[i], data.posZ[i]));


            for (int i = 0; i < pos.Length; i++)
            {
                Instantiate(Dot, pos[i], transform.rotation);
            }
        }
    }

    [Serializable]
    class SaveData
    {
        public int posSize;
        public float[] posX;
        public float[] posY;
        public float[] posZ;
    }

}
