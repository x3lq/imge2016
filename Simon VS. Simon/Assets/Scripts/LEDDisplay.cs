using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDDisplay : MonoBehaviour
{
    public bool on, off;
    public Controller c;
    public GameHandler gameHandler;
    public GameObject[] led;

    





    
    //blau rot gelb grün
    void Update()
    {
        if (on)
        {
            setLED();
            on = false;
        }

        if (off)
        {
            off = false;
            offLED();
        }
    }

    public int[] calculateNumber()
    {
        int[] res = {0, 0, 0, 0};

        int num = gameHandler.SCount;

        int i = 3;
        while (i >= 0)
        {
            res[i] = num%2;
            num = num/2;
            i--;
        }

        return res;
    }

    public void setLED()
    {
        Debug.Log("LED");
        int[] on = calculateNumber();

        int i = 0;
        foreach (GameObject tmp in led)
        {
            if (on[i] == 1)
            {
                tmp.GetComponent<ColorChanger>().toggleColorOn();
            }
            else
            {
                tmp.GetComponent<ColorChanger>().toggleColorOff();
            }
            c.LED(i, on[i]);
            i++;
            
        }
    }

    public void offLED()
    {
        c.LEDOFF();
    }
}
