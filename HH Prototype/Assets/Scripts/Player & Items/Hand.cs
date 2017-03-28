using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Item
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moveing)
        {
            if (moveBack)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.8f, -3.9f, -1.5f), 0.1f);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.8f, -3.9f, -1), 0.2f);

            if (transform.localPosition.z > -1.05f)//swap
            {
                moveBack = true;
                PrimaryUse();
            }
            if (moveBack)
            {
                if (transform.localPosition.z < -1.49f)
                {
                    transform.localPosition = new Vector3(-0.8f, -3.9f, -1.5f);
                    moveing = false;
                    moveBack = false;
                }
            }
        }
    }

    public override void Move()
    {
        base.Move();
    }
    public override void PrimaryUse()
    {

    }


}
