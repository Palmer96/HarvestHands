using UnityEngine;
using System.Collections;

public class Construct : MonoBehaviour
{
    public string constructName = "";
    public string constructDescription = "";
    public GameObject selfObject;
    public bool canBuild;
    public Material[] mat;

   public bool colliding;
   public bool upRight;
    public bool onGround;
    // Use this for initialization
    void Start()
    {
        if (transform.GetComponent<Renderer>() != null)
        {
        mat = new Material[transform.childCount + 1];
            mat[transform.childCount] = transform.GetComponent<Renderer>().material;
        }
        else
            mat = new Material[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            mat[i] = transform.GetChild(i).GetComponent<Renderer>().material;
        }
        if (transform.GetComponent<Renderer>() != null)
        mat[transform.childCount] = transform.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.eulerAngles.x > 10 &&
              transform.eulerAngles.x < 350 ||
              transform.eulerAngles.z > 10 &&
              transform.eulerAngles.z < 350)
        {
            upRight = false;
        }
        else
        {
            upRight = true;
        }


        
        if (onGround && upRight && !colliding)
        {
            canBuild = true;
            for (int i = 0; i < mat.Length; i++)
                mat[i].color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            canBuild = false;
            for (int i = 0; i < mat.Length; i++)
                mat[i].color = new Color(1, 0, 0, 0.5f);
        }

      //  colliding = false;
    }

    public void Place()
    {
        Instantiate(selfObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void OnTriggerStay(Collider col)
    {
        if (!col.transform.CompareTag("Ground"))
            colliding = true;
    }
    void OnTriggerExit(Collider col)
    {
        colliding = false;
    }

}
