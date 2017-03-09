using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DestroyTimer : MonoBehaviour
{
    public float startTimer = 2;
    public float timer;
    // Use this for initialization
    void Start()
    {
        timer = startTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }

        if (GetComponent<Image>() != null)
        {
            Color col = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, timer / startTimer);
            Color tCol = transform.GetChild(0).GetComponent<Text>().color;
            transform.GetChild(0).GetComponent<Text>().color = new Color(tCol.r, tCol.g, tCol.b, timer / startTimer);
        }
    }
}
