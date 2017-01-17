using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationChanger : MonoBehaviour
{

    public GameObject[] pos;
    public float rotTime;
    private float timer;
    public float speed;

    void Start()
    {
        timer = rotTime;
    }


    public void pos0()
    {
        StartCoroutine(rotateLerp(0));
    }

    public void pos1()
    {
        StartCoroutine(rotateLerp(1));
    }

    public void pos2()
    {
        StartCoroutine(rotateLerp(2));
    }

    IEnumerator rotateLerp(int i)
    {
        while (timer > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, pos[i].transform.rotation, speed*Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = rotTime;
        transform.rotation = pos[i].transform.rotation;
    }
}
