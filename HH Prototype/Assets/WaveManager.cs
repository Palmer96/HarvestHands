using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance = null;

    public Text rabbitText;
    public Text plantText;
    public GameObject rabbit;

    public int waveNum;
    public int perWave;

    public bool inWave;

    public int rabbitsLeft;
    public int plantsLeft;
    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        rabbitText.text = "Rabbits left: " + rabbitsLeft.ToString();
        plantText.text = "Plants Remaining: " + plantsLeft.ToString();
    }


    public void StartWave()
    {
        //   if (!inWave)
        {
            inWave = true;

            for (int i = 0; i < perWave * waveNum; i++)
            {
                Instantiate(rabbit, RandPos(), transform.rotation);
            }
            waveNum++;
        }

        rabbitsLeft += perWave * waveNum;

    }

    Vector3 RandPos()
    {
        float x;
        float z;
        if (Random.Range(0, 2) == 1)
            x = Random.Range(-100, -40);
        else
            x = Random.Range(40, 100);

        if (Random.Range(0, 2) == 1)
            z = Random.Range(-30, -50);
        else
            z = Random.Range(40, 80);

        return new Vector3(x, 0, z);
    }
}
