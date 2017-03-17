using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public static DayNightController instance = null;

    public float timeSpeed = 12;
    public int ingameDay = 0;

    public GameObject Rain;

    public Text textMoney;
    public Text textDays;
    public Text textTime;



    public Material skybox;

    [Header("Lighting")]
    public Light worldLight;
    public float lightMaxIs = 1;
    public Gradient lightColour;
    public AnimationCurve lighting;

    [Header("Fog")]
    public float fogMaxIs = 1;
    public Gradient fogColour;
    public AnimationCurve fogDensity;

    public AnimationCurve fogStartDistance;
    public AnimationCurve fogEndDistance;

    [Range(0, 24f)]
    public float currentTimeOfDay = 0;

    public float timePast;
    // Use this for initialization
    void Start()
    {
        Rain.SetActive(false);
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        SaveAndLoadManager.OnSave += Save;
    }

    // Update is called once per frame
    void Update()
    {
        timePast = (Time.deltaTime / 60) * timeSpeed;
        currentTimeOfDay += timePast;

        worldLight.transform.rotation = Quaternion.identity;
        worldLight.transform.Rotate((currentTimeOfDay * 7.5f), -30, 0);

        if (currentTimeOfDay >= 24)
            DayJump();


        skybox.SetColor("_Tint", lightColour.Evaluate(currentTimeOfDay / 24));
        //  worldLight.color = gradient1.Evaluate(currentTimeOfDay / 24);
        float Scale1 = currentTimeOfDay / 12;

        worldLight.intensity = (lighting.Evaluate(currentTimeOfDay / 24) * lightMaxIs) * 0.8f;

        RenderSettings.fogDensity = fogDensity.Evaluate(currentTimeOfDay / 24) * fogMaxIs;
        RenderSettings.fogColor = fogColour.Evaluate(currentTimeOfDay / 24);
        RenderSettings.fogStartDistance = fogStartDistance.Evaluate(currentTimeOfDay / 24);
        RenderSettings.fogEndDistance = fogEndDistance.Evaluate(currentTimeOfDay / 24);

        textDays.text = "Day: " + ingameDay.ToString();

        int time = (int)(currentTimeOfDay * 60);

        string ampm;
        if (currentTimeOfDay > 12)
        {
            ampm = " PM";

        }
        else
            ampm = " AM";

        if (currentTimeOfDay > 13)
            textTime.text = "Time: " + (Mathf.Floor(currentTimeOfDay - 12)).ToString() + ":" + (time % 60).ToString() + ampm;
        else
            textTime.text = "Time: " + (Mathf.Floor(currentTimeOfDay)).ToString() + ":" + (time % 60).ToString() + ampm;




        textMoney.text = "$" + PlayerInventory.instance.money.ToString();


    }

    void UpdateTree()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Stump");
        for (int i = 0; i < trees.Length; i++)
        {
            trees[i].GetComponent<RespawnNode>().UpdateTree();
        }
    }

    public void BedDayJump()
    {
        //  if (currentTimeOfDay > 18)
        {
            float time = 24 - currentTimeOfDay + 6;

            currentTimeOfDay = 6;
            ingameDay++;

            Debug.Log("Time Past: " + time);

            PlantManager.instance.UpdatePlants(time);
            UpdateTree();
            SellChest.SellAllChests();
            if (WaveManager.instance != null)
                WaveManager.instance.StartWave();


            if (Random.Range(1, 5) == 1)
            {
                Rain.SetActive(true);
                PlantManager.instance.Raining(true);
            }
            else
            {
                Rain.SetActive(false);
                PlantManager.instance.Raining(false);
            }
            if (Random.Range(1, 5) == 1)
            {
                PlantManager.instance.GlobalWeedSpread();
            }
        }
    }

    public void DayJump()
    {
        currentTimeOfDay = 0;
        ingameDay++;
        PlantManager.instance.UpdatePlants(ingameDay);
        UpdateTree();
        SellChest.SellAllChests();
        if (WaveManager.instance != null)
            WaveManager.instance.StartWave();

        if (Random.Range(1, 5) == 1)
        {
            Rain.SetActive(true);
            PlantManager.instance.Raining(true);
        }

        else
        {
            Rain.SetActive(false);
            PlantManager.instance.Raining(false);
        }
        if (Random.Range(1, 5) == 1)
        {
            PlantManager.instance.GlobalWeedSpread();
        }
    }

    void OnDestroy()
    {
        SaveAndLoadManager.OnSave -= Save;
    }

    public void Save()
    {
        SaveAndLoadManager.instance.saveData.dayNightControllerSave = new DayNightControllerSave(this);
        //Debug.Log("Saved item = " + name);
    }

    void OnApplicationQuit()
    {
        skybox.SetColor("_Tint", new Color(128.0f / 255, 128.0f / 255, 128.0f / 255));
    }
}

[System.Serializable]
public class DayNightControllerSave
{
    int ingameDay;
    float currentTimeOfDay;
    bool raining;

    public DayNightControllerSave(DayNightController DNCont)
    {
        ingameDay = DNCont.ingameDay;
        currentTimeOfDay = DNCont.currentTimeOfDay;
        raining = DNCont.Rain.activeSelf;
    }

    public GameObject LoadObject()
    {
        if (DayNightController.instance != null)
        {
            DayNightController.instance.ingameDay = ingameDay;
            DayNightController.instance.currentTimeOfDay = currentTimeOfDay;
            DayNightController.instance.Rain.SetActive(raining);
            PlantManager.instance.Raining(raining);

            return DayNightController.instance.gameObject;
        }
        Debug.Log("Failed to load DayNightController, DayNightController.instance == null");
        return null;
    }
}