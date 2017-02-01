using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

    public static int GameMode;
    public static bool ShowReplay;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showReplay(bool wert) { ShowReplay = wert; Debug.Log(ShowReplay); }
}
