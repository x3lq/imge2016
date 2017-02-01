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

    public static float waitTime = 1.0f;

    public bool on;

    public static bool replaying;


    
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

    public void Reset()
    {
        reset.Invoke();
    }

    public void OnReplay()
    {
        Debug.Log("OnReplay");
        StartCoroutine(replay());
    }

    IEnumerator replay()
    {
        Debug.Log("Replay Of Sequenz: " + GameHandler.Sequenz.Count);

        foreach (ControllerElement var in GameHandler.Sequenz)
        {
            if (var.Type.Equals("Button"))
            {
                Debug.Log("ID: " + var.ID);
                buttons[var.ID].Invoke();
            }
            else if (var.Type.Equals("Slider"))
            {
                if (var.ID == 0)
                {
                    slider01[var.Position].Invoke();
                }
                else
                {
                    slider02[var.Position].Invoke();
                }
            }
            else if (var.Type.Equals("Knob"))
            {
                Debug.Log(var.Position);
                if (var.ID == 0)
                {
                    rotation01[var.Position].Invoke();
                }
                else
                {
                    rotation02[var.Position].Invoke();
                }
            }
            else if (var.Type.Equals("LED"))
            {
                led[var.ID].Invoke();
            }
            yield return new WaitForSeconds(waitTime + 0.1f);
        }

        Debug.Log("Finished Replaying!");

        replaying = false;
    }
}
