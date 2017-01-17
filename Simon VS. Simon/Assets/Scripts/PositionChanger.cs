﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChanger : MonoBehaviour
{

    public float speed;
    public GameObject[] pos;
   // public GameObject objectToSlide;
	// Use this for initialization
	/*void Start () {
        pos = new GameObject[3];
	    int i = 0;
	    foreach (Transform child in transform)
	    {
	        pos[i] = child.gameObject;
	    }
		
	}*/


    public void toPos0()
    {
        StartCoroutine(toPos(0));
    }

    public void toPos1()
    {
        StartCoroutine(toPos(1));
    }

    public void toPos2()
    {
        StartCoroutine(toPos(2));
    }

    IEnumerator toPos(int i)
    {
        
       while ( Vector3.Distance(transform.position, pos[i].transform.position) > 0.05f)
        //while(objectToSlide.transform.position.y != pos[i].transform.position.y)
        {
            transform.position = Vector3.Lerp(transform.position, pos[i].transform.position, Time.deltaTime*speed);
           //objectToSlide.transform.position = transform.position + Vector3.forward;
            yield return null;
        }
        //Vector3.Lerp(transform.position, pos[i].transform.position, Time.deltaTime*speed);

        transform.position = pos[i].transform.position;
        //yield return null;

    }
}
