using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChanger : MonoBehaviour
{

    public float speed;
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
        
        transform.position = pos[i].transform.position;
        //Vector3.Lerp(transform.position, pos[i].transform.position, Time.deltaTime*speed);
        yield return null;
    }
}
