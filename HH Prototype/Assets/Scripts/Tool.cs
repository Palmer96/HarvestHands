using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tool : MonoBehaviour
{
    public int toolID;
   public float rayMaxDist = 5;

   public RaycastHit hit;
    public  Ray ray;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void UseTool()
    {
        Debug.Log("Use Tool");
      //  ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
      //
      //  useTool();
    }





}
