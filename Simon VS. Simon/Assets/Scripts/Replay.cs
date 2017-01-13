using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Replay : MonoBehaviour
{
    public UnityEvent Button_1_down;

    public bool on;

    void Update()
    {
        if (on)
        {
            on = false;
            Button_1_down.Invoke();
        }  
    }

    void OnReplay()
    {

        foreach (string var in GameHandler.Sequenz)
        {
            switch (var)
            {
                case "A":
                    Button_1_down.Invoke();
                    break;

            }
        }
    }
}
