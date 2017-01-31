using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationChanger : MonoBehaviour
{

    public GameObject[] pos;
    private float timer;
    public float speed;

    void Start()
    {
        timer = Replay.waitTime;
    }

    public void reset()
    {
        StartCoroutine(rotateLerp(0, false));

    }
    public void pos0()
    {
        StartCoroutine(rotateLerp(0, true));
    }

    public void pos1()
    {
        StartCoroutine(rotateLerp(1, true));
    }

    public void pos2()
    {
        StartCoroutine(rotateLerp(2, true));
    }

    IEnumerator rotateLerp(int i, bool color)
    {
        if(color) gameObject.GetComponent<ColorChanger>().toggleColorOn();
        while (timer > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, pos[i].transform.rotation, speed*Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = Replay.waitTime;
        transform.rotation = pos[i].transform.rotation;
        gameObject.GetComponent<ColorChanger>().toggleColorOff();
    }


}
