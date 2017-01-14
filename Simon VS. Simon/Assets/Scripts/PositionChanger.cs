using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChanger : MonoBehaviour
{

    private GameObject[] pos;
	// Use this for initialization
	void Start () {
        pos = new GameObject[3];
	    int i = 0;
	    foreach (Transform child in transform)
	    {
	        pos[i] = child.gameObject;
	    }
		
	}


    void toPos0()
    {
        Vector3.Lerp(transform.position, pos[0].transform.position, Time.deltaTime);
    }

    void toPos1()
    {
        Vector3.Lerp(transform.position, pos[1].transform.position, Time.deltaTime);

    }

    void toPos2()
    {
        Vector3.Lerp(transform.position, pos[2].transform.position, Time.deltaTime);

    }
}
