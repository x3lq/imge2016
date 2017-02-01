using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHover : MonoBehaviour {

    public GameObject disable;

    public void onHover()
    {
        disable.SetActive(true);
        StartCoroutine(hoverDisable());
    }

    IEnumerator hoverDisable()
    {
        yield return new WaitForSeconds(1);
        disable.SetActive(false);
    }
}
