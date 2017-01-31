using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    public Material defaultMat;
    public Material activeMat;

    public static float onTime = 1.0f;
    public bool on;

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
        Debug.Log("Color Switch ");
        if (on) { toggleColorOff(); StartCoroutine(turnOnColor(0.1f)); }
        else
        {
            toggleColorOn();
        }
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
        on = true;
        gameObject.GetComponent<Renderer>().material = activeMat;
    }

    IEnumerator turnOnColor(float timer)
    {
        on = true;
        yield return new WaitForSeconds(timer);
        toggleColorOn();
    }

    IEnumerator turnOffColor()
    {
        yield return new WaitForSeconds(Replay.waitTime);
        on = false;
        toggleColorOff();
    }
}
