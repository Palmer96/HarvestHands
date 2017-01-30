using UnityEngine;
using System.Collections;

public class mesh : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameObject Display = (GameObject)Instantiate(transform.parent.GetComponent<StoreItem>().objectToBuy, transform.position, transform.rotation);
        GetComponent<MeshFilter>().mesh = Display.GetComponent<MeshFilter>().mesh;
        GetComponent<MeshRenderer>().material = Display.GetComponent<MeshRenderer>().material;
        Destroy(Display);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
