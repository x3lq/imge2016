using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    public Material defaultMat;
    public Material activeMat;

    public static float onTime = 2.0f;

    public bool test;

    void Update()
    {
        if (test)
        {
            test = false;
            toggleColorOnwithTimer();
        }
    }


    public void toggleColorOnwithTimer()
    {
        toggleColorOn();
        StartCoroutine(turnOffColor());
    }

    public void toggleColorOff()
    {
       // this.gameObject.GetComponent<Renderer>().material.color = DefaultColor;
        gameObject.GetComponent<Renderer>().material = defaultMat;

    }

    public void toggleColorOn()
    {
        //this.gameObject.GetComponent<Renderer>().material.color = ActiveColor;
        gameObject.GetComponent<Renderer>().material = activeMat;
    }



    IEnumerator turnOffColor()
    {
        yield return new WaitForSeconds(onTime);
        toggleColorOff();
    }
}
