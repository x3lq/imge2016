using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Song {

    public static Song current;

    public List<EH> Events;

    public Song()
    {
        current = this;
        Events = new List<EH>();
    }

    public void createSong()
    {
        Events.Add(new EH(1f, 0));
        Events.Add(new EH(1f, 1));
        Events.Add(new EH(1f, 2));
        Events.Add(new EH(1f, 3));
        Events.Add(new EH(2f, 0));
        Events.Add(new EH(2f, 1));
        Events.Add(new EH(2f, 2));
        Events.Add(new EH(2f, 3));

        Debug.Log("Song created...");
    }
}
