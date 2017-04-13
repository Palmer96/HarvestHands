using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMessage : MonoBehaviour
{
    public static ScreenMessage instance;
    public GameObject MessagePrefab;
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

    public void CreateMessage(string message)
    {
        GameObject go = Instantiate(MessagePrefab, transform.position, transform.rotation);
        go.transform.SetParent(transform);
        go.transform.GetChild(0).GetComponent<Text>().text = message;
    }
}
