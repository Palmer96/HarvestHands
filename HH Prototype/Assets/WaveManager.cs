using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance = null;


    public GameObject rabbit;

    public int waveNum;
    public int perWave;

    public bool inWave;
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
