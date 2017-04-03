using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageColourShift : MonoBehaviour
{

    public float min = 0;
    public float max = 1;
    // Use this for initialization
    void Start()
    {
       // GetComponent<Renderer>().material.SetFloat("ColourRandomization", Mathf.PerlinNoise(transform.position.x, transform.position.z));
        float rand = Mathf.Abs(Mathf.PerlinNoise(transform.position.x, transform.position.z));
        Debug.Log(name + ": " + rand);
        GetComponent<Renderer>().material.SetFloat("ColourRandomization", rand);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
