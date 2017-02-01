using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;


public class Controller : MonoBehaviour
{

    public bool vibrate;
    public bool vibrating;

    public static Controller c;
    public float time;

    SerialPort stream = new SerialPort("COM3", 115200);
    private string receivedData = "EMPTY";
    //private string receivedData = "00000040";


    public int MotorSpeed;

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

    // Axes
    public float AccX, AccY, AccZ;

    // Microphone
    public float Volume;

    //Vector3 for gesture erkennung     Für Gesture müssen die Scripts aus T6 implementiert werden
    /*
    private StreamProcessor sp;
    private Vector3 gestureVector3;
    private Gesture current_Gesture;
    private GestureLibrary gestureLibrary;
    public String detected_Gesture;
    */

    //put into other script:
    /*
     * Controller C;
     * 
     * StartCoroutine(init());
     * 
     * 
     *     IEnumerator init()
    {
        yield return new WaitUntil(() => Controller.c != null);
        C = Controller.c;
    }
     */

    //t1 functions 
    //open port
    void Start()
    {
            c = this;
            Debug.Log(stream.IsOpen);
            if (!stream.IsOpen)
            {
                stream.Open();
                Debug.Log("Serial Stream started");
            }
            Debug.Log("Stream is Open: " + stream.IsOpen);

            // Gestures
            /*
            sp = new StreamProcessor();
            gestureLibrary = new GestureLibrary();
            load_Gestures();
            */
    }

    // Update is called once per frame
    void Update()
    {
        Buttons();
        Sliders();
        Accellerometer();
        Mic();

        if (vibrate) { StartCoroutine(Vibrate(3, 0.1f, 0.1f, 700));}

        //setMotorSpeed(MotorSpeed);
        //gestureDetection();

    }

    public void Buttons()
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

        int receivedValue = System.Convert.ToInt32(receivedData, 16);

        if (b1deadTime <= 0)
        {
            b1pressed = (receivedValue & Button1) != 0;
            b1deadTime = 0.2f;
        }
        else
        {
            b1pressed = false;
            b1deadTime -= Time.deltaTime;
        }

        if (b2deadTime <= 0)
        {
            b2pressed = (receivedValue & Button2) != 0;
            b2deadTime = 0.2f;
        }
        else
        {
            b2pressed = false;
            b2deadTime -= Time.deltaTime;
        }

        if (b3deadTime <= 0)
        {
            b3pressed = (receivedValue & Button3) != 0;
            b3deadTime = 0.2f;
        }
        else
        {
            b3pressed = false;
            b3deadTime -= Time.deltaTime;
        }

        if (b4deadTime <= 0)
        {
            b4pressed = (receivedValue & Button4) != 0;
            b4deadTime = 0.2f;
        }
        else
        {
            b4pressed = false;
            b4deadTime -= Time.deltaTime;
        }

        if (b5deadTime <= 0)
        {
            b5pressed = (receivedValue & Button5) != 0;
            b5deadTime = 0.2f;
        }
        else
        {
            b5pressed = false;
            b5deadTime -= Time.deltaTime;
        }

        if (b6deadTime <= 0)
        {
            b6pressed = (receivedValue & Button6) != 0;
            b6deadTime = 0.2f;
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
    }

    public void Sliders()
    {
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
    }

    public void LED(int ID, int Mode)
    {
        string s = "l " + ID + " " + Mode + "\r\n";
        stream.Write(s);
        stream.ReadLine();
    }

    public void LEDOFF()
    {
        string s = "l " + 0 + " " + 0 + "\r\n";
        stream.Write(s);
        stream.ReadLine();
        s = "l " + 1 + " " + 0 + "\r\n";
        stream.Write(s);
        stream.ReadLine();
        s = "l " + 2 + " " + 0 + "\r\n";
        stream.Write(s);
        stream.ReadLine();
        s = "l " + 3 + " " + 0 + "\r\n";
        stream.Write(s);
        stream.ReadLine();
    }

    public void setMotorSpeed(float Speed)
    {
        stream.Write("m " + Speed + "\r\n");
        stream.ReadLine();
        MotorSpeed = (int)Speed;
    }

    public IEnumerator Vibrate(int Times, float OnTime, float Gap, int Strength)
    {
        vibrate = false;

        if (!vibrating)
        {
            vibrating = true;

            Debug.Log("Motor OnTime: " + OnTime);

            for (int i = 0; i < Times; i++)
            {
                stream.Write("m " + 800 + "\r\n");
                stream.ReadLine();
                MotorSpeed = (int)800;

                yield return new WaitForSeconds(OnTime);

                stream.Write("m " + 0 + "\r\n");
                stream.ReadLine();
                MotorSpeed = (int)0;

                yield return new WaitForSeconds(Gap);
            }

            Debug.Log("Motor Off!");

        }
        vibrating = false;
    }

    public void Accellerometer()
    {
        stream.Write("a");
        receivedData = stream.ReadLine();

        string[] Axes = receivedData.Split(' ');
        AccX = convertSigned((System.Convert.ToInt32(Axes[1], 16)))/128f;
        AccY = convertSigned((System.Convert.ToInt32(Axes[2], 16)))/128f;
        AccZ = convertSigned((System.Convert.ToInt32(Axes[3], 16)))/128f;

    }

    public void Mic()
    {
        stream.Write("s");
        receivedData = stream.ReadLine();
        Volume = (float)(System.Convert.ToDouble(receivedData.Split(' ')[1]))/32768f;
    }
    
    private int convertSigned(int input)
    {
        if(input > 127) { input -= 256; }
        return input;
    }


    // Gestures
    /*
    private void gestureDetection()
    {
        gestureVector3 = new Vector3(AccX,AccY,AccZ);
        sp.ProcessRawMeasurement(gestureVector3);

        if (sp.GestureIsValid)
        {
           current_Gesture =  sp.Gesture;
           detected_Gesture = gestureLibrary.Classify(current_Gesture);
           Debug.Log(sp.Gesture.ToString());
        }
    }

    private void load_Gestures()
    {
        //String path = Application.dataPath + "/Gestures/";
        String path = Application.dataPath+ "\\Gestures\\gesture_";
        
        //Debug.Log(path);
        gestureLibrary["up"]= new GestureModel();
        gestureLibrary["up"].load(path + "High.xml");
        gestureLibrary["right"] = new GestureModel();
        gestureLibrary["right"].load(path + "Mid.xml");
        gestureLibrary["left"] = new GestureModel();
        gestureLibrary["left"].load(path + "Low.xml");

        gestureLibrary["left"] = new GestureModel();
        gestureLibrary["left"].load(path + "left.xml");

        gestureLibrary["right"] = new GestureModel();
        gestureLibrary["right"].load(path + "right.xml");

        gestureLibrary["up"] = new GestureModel();
        gestureLibrary["up"].load(path + "up.xml");

    }
    */

    void OnGui()
    {
        /*
        // T1 Ausgabe im Spielscreen
        // hex string & Button
        int receivedValue = System.Convert.ToInt32(receivedData, 16);
        //int buttonBitMask= System.Convert.ToInt32("00000040", 16);

        GUI.Label(new Rect(500, 10, 120, 20), receivedData);
        GUI.Label(new Rect(500, 30, 120, 20), "Button1: " + ((receivedValue & Button1) != 0));
        */
    }
}

