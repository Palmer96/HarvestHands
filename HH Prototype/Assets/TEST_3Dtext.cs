﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_3Dtext : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
    }
}