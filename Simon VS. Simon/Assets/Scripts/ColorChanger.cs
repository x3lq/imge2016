using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    public Color DefaultColor;
    public Color ActiveColor;

    public static float onTime = 2.0f;

    void Update()
    {
       // Debug.Log(DefaultColor.ToString());
       // Debug.Log(ActiveColor.ToString());
    }

    public void toggleColorOn()
    {
        this.gameObject.GetComponent<Renderer>().material.color = ActiveColor;
        StartCoroutine(turnOffColor());
    }

    public void toggleColorOff()
    {
        this.gameObject.GetComponent<Renderer>().material.color = DefaultColor;
    }



    IEnumerator turnOffColor()
    {
        yield return new WaitForSeconds(onTime);
        toggleColorOff();
    }
}
