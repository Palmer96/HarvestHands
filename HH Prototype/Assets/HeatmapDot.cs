using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapDot : MonoBehaviour
{

  public  float count;
    Color col;
    // Use this for initialization
    void Start()
    {
        count = 0;
        col = GetComponent<Renderer>().material.GetColor("_MainColor");

    }

    // Update is called once per frame
    void Update()
    {
            GetComponent<Renderer>().material.SetColor("_MainColor", new Color(col.r, col.g - (count/255f), col.b, col.a));
       //    transform.localScale = new Vector3(1,1,1) +  (new Vector3(0.001f, 0.001f, 0.001f) * count);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("HeatDot"))
        {
           // transform.position += other.transform.position;
           // transform.position /= 2;
           // Destroy(other.gameObject);
         //   transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            //GetComponent<Renderer>().material.color = new Color(col.r, col.g - (count), col.b, col.a);
            count+= 10;
        }
    }
}
