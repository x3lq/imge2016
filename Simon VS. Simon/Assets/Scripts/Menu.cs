using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Menu : MonoBehaviour {

    public GameObject Play, Options, Quit;  // Main Menu
    public GameObject Standard, TimeTrial, Hatefest;
    public GameObject Replay, MainMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void play()
    {
        Play.SetActive(false);
        Options.SetActive(false);
        Quit.SetActive(false);
        Standard.SetActive(true);
        TimeTrial.SetActive(true);
        Hatefest.SetActive(true);
        MainMenu.SetActive(true);
    }

    public void quit()
    {
        Debug.Log("Quit Game...");
        Application.Quit();
        if (EditorApplication.isPlaying) { EditorApplication.isPlaying = false; }
        Debug.Log("Still there?");
    }

    public void options()
    {
        Play.SetActive(false);
        Options.SetActive(false);
        Quit.SetActive(false);
        Replay.SetActive(true);
        MainMenu.SetActive(true);
    }

    public void standard()
    {
        global::Options.GameMode = 0;
        SceneManager.LoadScene(1);
    }
    public void timetrial()
    {
        global::Options.GameMode = 1;
        SceneManager.LoadScene(1);
    }
    public void hatefest()
    {
        global::Options.GameMode = 2;
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        Replay.SetActive(false);
        Standard.SetActive(false);
        TimeTrial.SetActive(false);
        Hatefest.SetActive(false);
        MainMenu.SetActive(false);
        Play.SetActive(true);
        Options.SetActive(true);
        Quit.SetActive(true);
    }
}
