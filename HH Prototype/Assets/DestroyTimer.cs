using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DestroyTimer : MonoBehaviour
{
    public float startTimer = 2;
    public float timer;

    public float speed = 0.1f;
    // Use this for initialization
    void Start()
    {
        timer = startTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        transform.position += new Vector3(0, -speed, 0);


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
