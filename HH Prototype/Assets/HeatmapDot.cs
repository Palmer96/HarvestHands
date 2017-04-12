using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapDot : MonoBehaviour
{

    public enum HeatColour
    {
        Red,
        Purple,
        Blue

    }

    public HeatColour heatColour = HeatColour.Red;
    public int count;
  public  Color col;
    // Use this for initialization
    void Start()
    {
        count = 0;
       // col = GetComponent<Renderer>().material.GetColor("_MainColor");

    }

    // Update is called once per frame
    void Update()
    {
        //    GetComponent<Renderer>().material.SetColor("_MainColor", MovementTracker.instance.GetColour(count));
        switch (heatColour)
        {
            case HeatColour.Red:
                GetComponent<Renderer>().material.SetColor("_MainColor", new Color(col.r, col.g - (count / 255f), col.b, col.a));

                break;
            case HeatColour.Purple:
                GetComponent<Renderer>().material.SetColor("_MainColor", new Color(col.r + (count / 255f), col.g, col.b, col.a));
                break;
            case HeatColour.Blue:

                break;
        }

        //    transform.localScale = new Vector3(1,1,1) +  (new Vector3(0.001f, 0.001f, 0.001f) * count);
    }

    public void SetColour(Color colour)
    {
        GetComponent<SpriteRenderer>().color = colour;
      //  GetComponent<SpriteRenderer>().color = new Color(col.r, col.g - ((count * 10) / 255f), col.b, col.a);
     //  GetComponent<Renderer>().material.SetColor("_MainColor", new Color(col.r, col.g - (count / 255f), col.b, 1));
    //    GetComponent<Renderer>().material.SetColor("_MainColor", new Color(col.r, col.g - (count / 255f), col.b, col.a));
    }
    void OnTriggerEnter(Collider other)
    {

      //  if (other.transform.CompareTag("HeatDot"))
      //  {
      //      count += 10;
      //  }
    }
}
