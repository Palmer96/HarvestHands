﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MaterialColour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
