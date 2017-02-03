﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public static DayNightController instance = null;

    public int ingameDay = 0;

    public GameObject Rain;

    public Text textMoney;
    public Text textDays;
    public Text textTime;


    public Light worldLight;
    public Material skybox;
    public Gradient gradient1;


    [Range(0, 24f)]
    public float currentTimeOfDay = 0;
    // Use this for initialization
    void Start()
    {
        Rain.SetActive(false);
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeOfDay += (Time.deltaTime / 60)*24;

        if (currentTimeOfDay >= 24)
            DayJump();


        skybox.SetColor("_SkyTint", gradient1.Evaluate(currentTimeOfDay / 24));
        //  worldLight.color = gradient1.Evaluate(currentTimeOfDay / 24);
        float Scale1 = currentTimeOfDay / 12;

        if (Scale1 < 1)
            worldLight.intensity = Scale1;
        else
            worldLight.intensity = 1 - (Scale1 - 1);


        textDays.text = "Days: " + ingameDay.ToString();
        textTime.text = "Time: " + ((int)(currentTimeOfDay * 100)).ToString();

        textMoney.text = "$" + PlayerInventory.instance.money.ToString();
      

    }

    public void DayJump()
    {
        currentTimeOfDay = 0;
        ingameDay++;
        PlantManager.instance.UpdatePlants(ingameDay);
        SellChest.SellAllChests();

        if (Random.Range(1, 5) == 1)
        {
            Rain.SetActive(true);
            PlantManager.instance.WaterPlants();
        }
        else
            Rain.SetActive(false);
    }
}
