﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChanger : MonoBehaviour
{

    public static bool moving;

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
    public void reset()
    {
        StartCoroutine(toPos(0, false));

    }

    public void toPos0()
    {
        StartCoroutine(toPos(0, true));
    }

    public void toPos1()
    {
        StartCoroutine(toPos(1, true));
    }

    public void toPos2()
    {
        StartCoroutine(toPos(2, true));
    }

    IEnumerator toPos(int i, bool color)
    {
        yield return new WaitUntil(() => !moving);
        moving = true;

        float Lock = 2;
        
        if(color) gameObject.GetComponent<ColorChanger>().toggleColorOn();
        while ( Lock > 0 && Vector3.Distance(transform.position, pos[i].transform.position) > 0.05f)
        //while(objectToSlide.transform.position.y != pos[i].transform.position.y)
        {
            Debug.Log("Changing Position to " + i);
            transform.position = Vector3.Lerp(transform.position, pos[i].transform.position, Time.deltaTime*speed);
            //objectToSlide.transform.position = transform.position + Vector3.forward;
            Lock -= Time.deltaTime;
            yield return null;
        }
        //Vector3.Lerp(transform.position, pos[i].transform.position, Time.deltaTime*speed);

        transform.position = pos[i].transform.position;
        //yield return null;
        gameObject.GetComponent<ColorChanger>().toggleColorOff();

        moving = false;
    }
}
