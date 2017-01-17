using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Replay : MonoBehaviour
{
    public UnityEvent[] buttons;

    public UnityEvent[] slider01;
    public UnityEvent[] slider02;

    public UnityEvent[] rotation01;
    public UnityEvent[] rotation02;

    public UnityEvent[] led;

    public UnityEvent reset;

    public bool on;

    public Color active, def;

   /* void Start()
    {
        ColorChanger.DefaultColor = def;
        ColorChanger.ActiveColor = active;
        Debug.Log(def.ToString());
        Debug.Log(active.ToString());
    }*/

    void Update()
    {
        if (on)
        {
            on = false;
         /*   foreach (UnityEvent button in buttons)
            {
                button.Invoke();
            }*/
            reset.Invoke();
        }  
    }

    void Reset()
    {
        reset.Invoke();
    }

    void OnReplay()
    {

        foreach (string var in GameHandler.SequenzAlt)
        {
            switch (var)
            {
                case "Buttton1":
                    buttons[0].Invoke();
                    break;
                case "Buttton2":
                    buttons[1].Invoke();
                    break;
                case "Buttton3":
                    buttons[2].Invoke();
                    break;
                case "Buttton4":
                    buttons[3].Invoke();
                    break;
                case "Buttton5":
                    buttons[4].Invoke();
                    break;
                case "Buttton6":
                    buttons[5].Invoke();
                    break;

            }
        }
    }
}
