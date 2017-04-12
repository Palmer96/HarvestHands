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
    // int count;
    public GameObject Dot;
    int posSize;

    public Gradient col;

    public Transform Movement;

    public Terrain terrain;
    public Gradient grdnt;
    public int highest;
    public int highestMax;
    //  public int axeUsed;
    //  public int pickaxeUsed;
    //  public int shovelUsed;
    //  public int hammerUsed;
    //  public int sycleUsed;
    //
    //
    //  public int fencePlaced;
    //  public int fenceBuilt;
    //
    //  public int plotPlaced;
    //  public int plotBuilt;
    //
    //  public int scarecrowPlaced;
    //  public int scarecrowBuilt;
    //
    //  public int troughPlaced;
    //  public int troughBuilt;





    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);


        timer = rate;
        // count = 0;
        highest = 0;
        pos = new Vector2[0];
        Movement = transform.GetChild(0);

        //  terrain.terrainData.
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = rate;
            //   pos.Add(new Vector2(PlayerInventory.instance.transform.position.x, PlayerInventory.instance.transform.position.z));
            Vector2[] newpos = new Vector2[pos.Length + 1];
            for (int i = 0; i < pos.Length; i++)
            {
                newpos[i] = pos[i];
            }
            pos = newpos;
            posSize = pos.Length;
            pos[posSize - 1] = new Vector2(PlayerInventory.instance.transform.position.x, PlayerInventory.instance.transform.position.z);
            // Instantiate(Dot, new Vector3(PlayerInventory.instance.transform.position.x, (int)PlayerInventory.instance.transform.position.y, PlayerInventory.instance.transform.position.z), Quaternion.identity);
            // count++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Save(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Save(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Save(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Save(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Save(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Save(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Save(8);
        }


        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    Save();
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Save2();
        //}
        //
        //     if (Input.GetKeyDown(KeyCode.K))
        //     {
        //         Load();
        //     }
        //     if (Input.GetKeyDown(KeyCode.L))
        //     {
        //         Load2();
        //     }

    }

    void OnApplicationClose()
    {
        //   Save();
    }

    public void Save(int num)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/SaveFiles/playerInfo" + num + ".dat");//, FileMode.Open);

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


    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");//, FileMode.Open);

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

    public void Save2()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo2.dat");//, FileMode.Open);

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

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            posSize = data.posSize;

            //  pos.Clear();
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
                // if (i ==0)
                dot.GetComponent<HeatmapDot>().SetColour(grdnt.Evaluate(((float)count[i] / highest)));
            }
        }
    }
    public void Load2()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo2.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo2.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            posSize = data.posSize;

            //  pos.Clear();
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
            for (int i = 1; i < count.Count; i++)
            {
                if (count[i] > highest)
                    highest = count[i];
            }

            for (int i = 0; i < heat.Count; i++)
            {
                GameObject dot = Instantiate(Dot, new Vector3(heat[i].x, 0, heat[i].y), Dot.transform.rotation);
                dot.transform.SetParent(Movement);
                dot.GetComponent<HeatmapDot>().count = count[i];
                // if (i ==0)
                dot.GetComponent<HeatmapDot>().SetColour(grdnt.Evaluate(((float)count[i] / highest)));
            }
        }
    }

    public void LoadAll()
    {
        heat = new List<Vector2>();
        count = new List<int>();

        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/SaveFiles");
        FileInfo[] infoArray = info.GetFiles();

     //  Debug.Log(infoArray[0].Directory);
     //  Debug.Log(infoArray[0].DirectoryName);
     //  Debug.Log(infoArray[0].FullName);
     //  Debug.Log(infoArray[0].Name);

        for (int k = 0; k < infoArray.Length - 1; k++)
        {
            if (File.Exists(infoArray[0].FullName))
            // if (File.Exists(Application.dataPath + "/SaveFiles/playerInfo0.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(infoArray[0].FullName, FileMode.Open);
                //FileStream file = File.Open(Application.dataPath + "/SaveFiles/playerInfo0.dat", FileMode.Open);
                SaveData data = (SaveData)bf.Deserialize(file);
                file.Close();

                posSize = data.posSize;

                //  pos.Clear();
                pos = new Vector2[posSize];


                for (int i = 0; i < posSize; i++)
                {
                    pos[i].x = data.posX[i];
                    pos[i].y = data.posZ[i];
                }



                for (int i = 0; i < pos.Length; i++)
                {
                  //  if (pos[i] == new Vector2(-36, -18))
                  //      continue;
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

        #region
        //  if (File.Exists(Application.dataPath + "/SaveFiles/playerInfo.dat"))
        //  {
        //      BinaryFormatter bf = new BinaryFormatter();
        //      FileStream file = File.Open(Application.dataPath + "/SaveFiles/playerInfo.dat", FileMode.Open);
        //      SaveData data = (SaveData)bf.Deserialize(file);
        //      file.Close();
        //
        //      posSize = data.posSize;
        //
        //      //  pos.Clear();
        //      pos = new Vector2[posSize];
        //
        //
        //      for (int i = 0; i < posSize; i++)
        //      {
        //
        //          pos[i].x = data.posX[i];
        //          pos[i].y = data.posZ[i];
        //      }
        //
        //
        //
        //      for (int i = 0; i < pos.Length; i++)
        //      {
        //          if (pos[i] == new Vector2(-36, -18))
        //              continue;
        //          bool toAdd = true;
        //          for (int j = 0; j < heat.Count; j++)
        //          {
        //              if (heat[j] == pos[i])
        //              {
        //                  count[j]++;
        //                  toAdd = false;
        //                  break;
        //              }
        //          }
        //          if (toAdd)
        //          {
        //              heat.Add(pos[i]);
        //              count.Add(1);
        //          }
        //
        //      }
        //      //  float highest = 0;
        //      //  for (int i = 0; i < count.Count; i++)
        //      //  {
        //      //      if (count[i] > highest)
        //      //          highest = count[i];
        //      //  }
        //      //
        //      //  for (int i = 0; i < heat.Count; i++)
        //      //  {
        //      //      GameObject dot = Instantiate(Dot, new Vector3(heat[i].x, 0, heat[i].y), Dot.transform.rotation);
        //      //      dot.transform.SetParent(Movement);
        //      //      dot.GetComponent<HeatmapDot>().count = count[i];
        //      //      // if (i ==0)
        //      //      dot.GetComponent<HeatmapDot>().SetColour(grdnt.Evaluate(((float)count[i] / highest)));
        //      //  }
        //  }
        //
        //
        //
        //  if (File.Exists(Application.dataPath + "/SaveFiles/playerInfo2.dat"))
        //  {
        //      BinaryFormatter bf = new BinaryFormatter();
        //      FileStream file = File.Open(Application.dataPath + "/SaveFiles/playerInfo2.dat", FileMode.Open);
        //      SaveData data = (SaveData)bf.Deserialize(file);
        //      file.Close();
        //
        //      posSize = data.posSize;
        //
        //      //  pos.Clear();
        //      pos = new Vector2[posSize];
        //
        //
        //      for (int i = 0; i < posSize; i++)
        //      {
        //          pos[i].x = data.posX[i];
        //          pos[i].y = data.posZ[i];
        //      }
        //
        //      for (int i = 0; i < pos.Length; i++)
        //      {
        //          if (pos[i] == new Vector2(-36, -18))
        //              continue;
        //          bool toAdd = true;
        //          for (int j = 0; j < heat.Count; j++)
        //          {
        //              if (heat[j] == pos[i])
        //              {
        //                  count[j]++;
        //                  toAdd = false;
        //                  break;
        //              }
        //          }
        //          if (toAdd)
        //          {
        //              heat.Add(pos[i]);
        //              count.Add(1);
        //          }
        //      }
        //  }

        #endregion

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
