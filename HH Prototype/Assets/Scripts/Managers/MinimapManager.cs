using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour {

    public static MinimapManager instance = null;
    public GameObject img;
	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateImage(Transform trans, Color col)
    {
        GameObject mapPoint = Instantiate(img);//, trans.position, Quaternion.identity);
        mapPoint.GetComponent<Image>().color = col;
        mapPoint.transform.SetParent(transform);
        mapPoint.transform.localRotation = Quaternion.identity;
        mapPoint.transform.localScale = new Vector3(1, 1, 1);
        mapPoint.GetComponent<TEST_minimapUI>().follow = trans;
    }
}
