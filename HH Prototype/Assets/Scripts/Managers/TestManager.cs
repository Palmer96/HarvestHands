using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public GameObject Object1;
    public GameObject Object2;

   // public Text Display;
  //  public string Name;
   // public string dialog;

    // Use this for initialization
    void Start()
    {
      //  Display.text = dialog.Replace("%Name", Name);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
            Instantiate(Object2, new Vector3(0, 0, 0), transform.rotation);

        if (Input.GetKeyDown(KeyCode.P))
            Instantiate(Object1, new Vector3(0, 0.25f, -12), transform.rotation);
    }
}
