using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;


public class Controller : MonoBehaviour
{

    public static Controller c;
    public float time;

    SerialPort stream = new SerialPort("COM3", 115200);
    private string receivedData = "EMPTY";
    //private string receivedData = "00000040";


    // Buttons
    //********************************
    private int Button1 = System.Convert.ToInt32("00000040", 16);
    private int Button2 = System.Convert.ToInt32("00000080", 16);
    private int Button3 = System.Convert.ToInt32("00000100", 16);
    private int Button4 = System.Convert.ToInt32("00000200", 16);
    private int Button5 = System.Convert.ToInt32("00000400", 16);
    private int Button6 = System.Convert.ToInt32("00000800", 16);

    public bool b1pressed = false;
    public bool b2pressed = false;
    public bool b3pressed = false;
    public bool b4pressed = false;
    public bool b5pressed = false;
    public bool b6pressed = false;

    private float b1deadTime = 0f;
    private float b2deadTime = 0f;
    private float b3deadTime = 0f;
    private float b4deadTime = 0f;
    private float b5deadTime = 0f;
    private float b6deadTime = 0f;
    //********************************

    // Sliders
    //********************************
    public float slider1;
    private float olds1;
    private float s1deadtime;
    public float slider2;
    private float s2deadtime;
    public float knob1;
    private float k1deadtime;
    public float knob2;
    private float k2deadtime;

    //put into other script:
    /*
     * Controller C = new Controller();
     * 
     * StartCoroutine(init());
     * 
     * 
     *     IEnumerator init()
    {
        yield return new WaitUntil(() => Controller.c != null);
        controle = Controller.c;
    }
     */

    //t1 functions 
    //open port
    void Start()
    {
        c = this;
       // c.b3pressed = true;
        stream.Open();
        Debug.Log("Serial Stream started");
    }

    // Update is called once per frame
    void Update()
    {

        // Buttons
        //*********************************************
        /*
         * 1: 60    |
         * 2: A0    |
         * 3: 120   |
         * 4: 220   |
         * 5: 420   |
         * 6: 820   |
         * */
        stream.Write("1");
        receivedData = stream.ReadLine();

        //Debug.Log(receivedData);

        int receivedValue = System.Convert.ToInt32(receivedData, 16);

        if (b1deadTime <= 0)
        {
            b1pressed = (receivedValue & Button1) != 0;
            b1deadTime = 0.5f;
        }
        else
        {
            b1pressed = false;
            b1deadTime -= Time.deltaTime;
        }

        if (b2deadTime <= 0)
        {
            b2pressed = (receivedValue & Button2) != 0;
            b2deadTime = 0.5f;
        }
        else
        {
            b2pressed = false;
            b2deadTime -= Time.deltaTime;
        }

        if (b3deadTime <= 0)
        {
            b3pressed = (receivedValue & Button3) != 0;
            b3deadTime = 0.5f;
        }
        else
        {
            b3pressed = false;
            b3deadTime -= Time.deltaTime;
        }

        if (b4deadTime <= 0)
        {
            b4pressed = (receivedValue & Button4) != 0;
            b4deadTime = 0.5f;
        }
        else
        {
            b4pressed = false;
            b4deadTime -= Time.deltaTime;
        }

        if (b5deadTime <= 0)
        {
            b5pressed = (receivedValue & Button5) != 0;
            b5deadTime = 0.5f;
        }
        else
        {
            b5pressed = false;
            b5deadTime -= Time.deltaTime;
        }

        if (b6deadTime <= 0)
        {
            b6pressed = (receivedValue & Button6) != 0;
            b6deadTime = 0.5f;
        }
        else
        {
            b6pressed = false;
            b6deadTime -= Time.deltaTime;
        }


        // b3pressed = (receivedValue & Button3) != 0;

        // b4pressed = (receivedValue & Button4) != 0;

        // b5pressed = (receivedValue & Button5) != 0;

        // b6pressed = (receivedValue & Button6) != 0;

        //*********************************************

        // Sliders
        //*********************************************
        stream.Write("4");
        receivedData = stream.ReadLine();
        string[] Sliders = receivedData.Split(' ');
        if (s1deadtime <= 0)
        {
            slider1 = System.Convert.ToInt32(Sliders[4], 16) / 4096f;
            s1deadtime = time / 100;
        }
        else
        {
            s1deadtime -= Time.deltaTime;
        }
        if (s2deadtime <= 0)
        {
            slider2 = System.Convert.ToInt32(Sliders[3], 16) / 4096f;
            s2deadtime = time / 100;
        }
        else
        {
            s2deadtime -= Time.deltaTime;
        }
        if (k1deadtime <= 0)
        {
            knob1 = System.Convert.ToInt32(Sliders[2], 16) / 4096f;
            k1deadtime = time / 100;
        }
        else
        {
            k1deadtime -= Time.deltaTime;
        }
        if (k2deadtime <= 0)
        {
            knob2 = System.Convert.ToInt32(Sliders[1], 16) / 4096f;
            k2deadtime = time / 100;
        }
        else
        {
            k2deadtime -= Time.deltaTime;
        }
        //*********************************************
    }

    void OnGui()
    {
        // T1 Ausgabe im Spielscreen
        // hex string & Button
        int receivedValue = System.Convert.ToInt32(receivedData, 16);
        //int buttonBitMask= System.Convert.ToInt32("00000040", 16);

        GUI.Label(new Rect(500, 10, 120, 20), receivedData);
        GUI.Label(new Rect(500, 30, 120, 20), "Button1: " + ((receivedValue & Button1) != 0));
    }
}
