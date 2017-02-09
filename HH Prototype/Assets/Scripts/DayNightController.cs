using UnityEngine;
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
        currentTimeOfDay += (Time.deltaTime / 60) * 24;

        worldLight.transform.rotation = Quaternion.identity;
        worldLight.transform.Rotate((currentTimeOfDay * 7.5f), -30, 0);

        if (currentTimeOfDay >= 24)
            DayJump();


        skybox.SetColor("_SkyTint", lightColour.Evaluate(currentTimeOfDay / 24));
        //  worldLight.color = gradient1.Evaluate(currentTimeOfDay / 24);
        float Scale1 = currentTimeOfDay / 12;

        worldLight.intensity = lighting.Evaluate(currentTimeOfDay / 24) * lightMaxIs;

        RenderSettings.fogDensity = fogDensity.Evaluate(currentTimeOfDay / 24) * fogMaxIs;
        RenderSettings.fogColor = fogColour.Evaluate(currentTimeOfDay / 24);
        RenderSettings.fogStartDistance = fogStartDistance.Evaluate(currentTimeOfDay / 24);
        RenderSettings.fogEndDistance = fogEndDistance.Evaluate(currentTimeOfDay / 24);

        textDays.text = "Days: " + ingameDay.ToString();

        int time = (int)(currentTimeOfDay * 60);

        if (currentTimeOfDay > 12)
        {
            textTime.text = "Time: " + (Mathf.Floor(currentTimeOfDay)).ToString() + ":" + (time % 60).ToString() + " PM";

        }
        else
            textTime.text = "Time: " + (Mathf.Floor(currentTimeOfDay)).ToString() + ":" + (time % 60).ToString() + " AM";




        textMoney.text = "$" + PlayerInventory.instance.money.ToString();


    }


    public void BedDayJump()
    {
        if (currentTimeOfDay > 18)
        {
            currentTimeOfDay = 6;
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
