using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class MovementTracker : MonoBehaviour
{

    public static MovementTracker instance = null;


    public List<Vector2> heat;
    public List<int> count;

    [SerializeField]
    public Vector2[] pos;
    public int posCount;
    public Vector2[] axe;
    public float timer;
    public float rate = 2;
    public GameObject Dot;
    int posSize;

    public Gradient col;

    public Transform Movement;

    public Terrain terrain;
    public Gradient grdnt;
    public int highest;
    public int highestMax;

    public FileInfo[] infoArray;

    public string loadFileName;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        timer = rate;
        highest = 0;
        pos = new Vector2[0];
        Movement = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = rate;
            Vector2[] newpos = new Vector2[pos.Length + 1];
            for (int i = 0; i < pos.Length; i++)
            {
                newpos[i] = pos[i];
            }
            pos = newpos;
            posSize = pos.Length;
            pos[posSize - 1] = new Vector2(PlayerInventory.instance.transform.position.x, PlayerInventory.instance.transform.position.z);
        }
    }

    void OnApplicationQuit()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            Save();
    }

    public void Save()
    {
        if (!File.Exists(Application.dataPath + "/SaveFiles"))
        {
            Directory.CreateDirectory(Application.dataPath + "/SaveFiles");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        float num = Time.realtimeSinceStartup;

        if (!File.Exists(Application.dataPath + "/SaveFiles/playerAnalystics_" + num + ".dat"))
            file = File.Create(Application.dataPath + "/SaveFiles/playerAnalystics_" + num + ".dat");//, FileMode.Open);

        SaveData data = new SaveData();

        data.posSize = posSize;
        data.posX = new int[posSize];
        data.posZ = new int[posSize];

        for (int i = 0; i < posSize; i++)
        {
            data.posX[i] = (int)Mathf.Round(pos[i].x);
            data.posZ[i] = (int)Mathf.Round(pos[i].y);
        }

        bf.Serialize(file, data);
        file.Close();
    }


    public void LoadSingle()
    {
        if (File.Exists(Application.dataPath + "/SaveFiles/" + loadFileName + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/SaveFiles/" + loadFileName + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            posSize = data.posSize;

            pos = new Vector2[posSize];

            for (int i = 0; i < posSize; i++)
            {

                pos[i].x = data.posX[i];
                pos[i].y = data.posZ[i];
            }

            heat = new List<Vector2>();
            count = new List<int>();

            for (int i = 0; i < pos.Length; i++)
            {
                bool toAdd = true;
                for (int j = 0; j < heat.Count; j++)
                {
                    if (heat[j] == pos[i])
                    {
                        count[j]++;
                        toAdd = false;
                        break;
                    }
                }
                if (toAdd)
                {
                    heat.Add(pos[i]);
                    count.Add(1);
                }
            }
            highest = 0;
            for (int i = 0; i < count.Count; i++)
            {
                if (count[i] > highest)
                    highest = count[i];
            }

            for (int i = 0; i < heat.Count; i++)
            {
                GameObject dot = Instantiate(Dot, new Vector3(heat[i].x, 0, heat[i].y), Dot.transform.rotation);
                dot.transform.SetParent(Movement);
                dot.GetComponent<HeatmapDot>().count = count[i];
                dot.GetComponent<HeatmapDot>().SetColour(grdnt.Evaluate(((float)count[i] / highest)));
            }
        }
    }

    public void LoadAll()
    {
        heat = new List<Vector2>();
        count = new List<int>();

        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/SaveFiles");
        infoArray = info.GetFiles();

        for (int k = 0; k < infoArray.Length - 1; k++)
        {
            if (File.Exists(infoArray[k].FullName))
            {
                if (infoArray[k].Extension == ".dat")
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(infoArray[k].FullName, FileMode.Open);
                    SaveData data = (SaveData)bf.Deserialize(file);
                    file.Close();

                    posSize = data.posSize;
                    pos = new Vector2[posSize];

                    for (int i = 0; i < posSize; i++)
                    {
                        pos[i].x = data.posX[i];
                        pos[i].y = data.posZ[i];
                    }

                    for (int i = 0; i < pos.Length; i++)
                    {
                        if (pos[i] == new Vector2(-53, -18))
                            continue;
                        bool toAdd = true;
                        for (int j = 0; j < heat.Count; j++)
                        {
                            if (heat[j] == pos[i])
                            {
                                count[j]++;
                                toAdd = false;
                                break;
                            }
                        }
                        if (toAdd)
                        {
                            heat.Add(pos[i]);
                            count.Add(1);
                        }
                    }
                }
            }
        }

        highest = 0;
        for (int i = 0; i < count.Count; i++)
        {
            if (count[i] < highestMax)
            {
                if (count[i] > highest)
                    highest = count[i];
            }
        }

        for (int i = 0; i < heat.Count; i++)
        {
            GameObject dot = Instantiate(Dot, new Vector3(heat[i].x, 0, heat[i].y), Dot.transform.rotation);
            dot.transform.SetParent(Movement);
            dot.GetComponent<HeatmapDot>().count = count[i];
            dot.GetComponent<HeatmapDot>().SetColour(grdnt.Evaluate(((float)count[i] / highest)));
        }
    }


    public void Clear()
    {
        int count = Movement.childCount - 1;
        for (int i = count; i >= 0; i--)
        {
            DestroyImmediate(Movement.GetChild(i).gameObject);
        }
        pos = new Vector2[0];
    }

    [Serializable]
    class SaveData
    {
        public int posSize;
        public int[] posX;
        public int[] posZ;
    }

}
