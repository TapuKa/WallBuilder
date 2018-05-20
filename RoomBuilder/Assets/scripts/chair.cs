using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chair : MonoBehaviour {

    bool isColliding;
    Color colTemp;

    void OnTriggerEnter(Collider other)
    {
        isColliding = true;
        Renderer re = GetComponentInChildren <Renderer>();
        colTemp = re.material.color;
        re.material.color = Color.red;
        Debug.Log("collision");

    }
    void OnTriggerExit(Collider other)
    {
        isColliding = false;
        Renderer re = GetComponentInChildren<Renderer>();
        re.material.color = colTemp;

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
