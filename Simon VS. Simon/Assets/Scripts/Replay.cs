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

    public float waitTime;

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
        Debug.Log("Replay Of Sequenz...");

        foreach (ControllerElement var in GameHandler.Sequenz)
        {
            if (var.Type.Equals("Button"))
            {
                buttons[var.id].Invoke();
            }
            else if (var.Type.Equals("Slider"))
            {
                if (var.id == 0)
                {
                    slider01[var.Position].Invoke();
                }
                else
                {
                    slider02[var.Position].Invoke();
                }
            }
            else if (var.Type.Equals("Drehknopf"))
            {
                if (var.id == 0)
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
                led[var.id].Invoke();
            }
        }
        yield return new WaitForSeconds(waitTime);

        Debug.Log("Finished Replaying!");

        replaying = false;
    }


}
