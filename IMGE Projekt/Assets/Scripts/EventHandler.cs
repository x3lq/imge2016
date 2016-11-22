using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventHandler : MonoBehaviour {

    public static EventHandler current;

    // Control
    public bool save, load, create, play;
    private bool playing;

    // Prefab
    public GameObject Event;

    // Song References
    public Song Song;
    private float SongTime;

    public static int Score;

    // Arten 
    public enum State { Perfect, Good, Bad, ZERO };

    // Liste der Buttons
    public List<State> Buttons;
    public List<Event> Events;

	// Use this for initialization
	void Start () {

        current = this;
        Song = new Song();

        Score = 0;

        Buttons = new List<State>();

        // 4 Buttons
        Buttons.Add(State.ZERO);
        Buttons.Add(State.ZERO);
        Buttons.Add(State.ZERO);
        Buttons.Add(State.ZERO);

        //StartCoroutine(Spawn());
    }
	
	// Update is called once per frame
	void Update () {

        if (load)
        {
            load = false;
            SaveLoad.Load();
        }
        if (save)
        {
            save = false;
            SaveLoad.Save();
        }
        if (create)
        {
            create = false;
            Song.current.createSong();
        }
        if (play)
        {
            play = false;
            playing = true;
            StartCoroutine(playSong());
        }
        if (playing)
        {
            SongTime += Time.deltaTime;
        }

        CheckButtons();

        if(SongTime > 0)
        {
            Debug.Log(SongTime);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            PressButton(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PressButton(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PressButton(2);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            PressButton(3);
        }
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(180, 10, 50, 20), Buttons[0].ToString());
        GUI.Label(new Rect(270, 10, 50, 20), Buttons[1].ToString());
        GUI.Label(new Rect(360, 10, 50, 20), Buttons[2].ToString());
        GUI.Label(new Rect(450, 10, 50, 20), Buttons[3].ToString());
        GUI.Label(new Rect(10, 10, 100, 20), "Score: " + Score);
        GUI.Label(new Rect(600, 10, 100, 20), "Time: " + SongTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int pos = 0; pos < 4; pos++)
        {
            Vector3 Pos = new Vector3(transform.position.x + 2 * pos, transform.position.y, transform.position.z);
            Gizmos.DrawRay(new Ray(Pos, Vector3.forward));
        }
    }
    
    private IEnumerator playSong()
    {
        Debug.Log("Play Song...");
        SongTime = 0;
        float Time = 0;
        while (Song.Events.Count != 0)
        {
            EH next = Song.Events[0];
            yield return new WaitForSeconds(next.Time - Time);
            Time = next.Time;

            while (next.Time ==Time)
            {
                Instantiate(Event).GetComponent<Event>().Position(next.Art);

                Song.Events.RemoveAt(0);
                if (Song.Events.Count != 0)
                {
                    next = Song.Events[0];
                }
                else { break; }
            }
        }
        playing = false;
        Debug.Log("Song finished...");
    }

    // Drückt einen Button/Event
    private void PressButton(int Art)
    {
        Vector3 Pos = new Vector3(transform.position.x + 2 * Art, transform.position.y, transform.position.z);

        Ray Ray = new Ray(Pos, Vector3.forward);
        RaycastHit Hit;

        if (Physics.Raycast(Ray, out Hit, 0.9f))
        {
            Hit.collider.GetComponent<Event>().getState(transform.position, true);
        }
        else
        {
            Score -= 10;
        }
    }

    // Überprüft alle EventSlots
    private void CheckButtons()
    {
        for(int pos = 0; pos < 4; pos++)
        {
            
            Vector3 Pos = new Vector3(transform.position.x + 2*pos, transform.position.y, transform.position.z);

            Ray Ray = new Ray(Pos, Vector3.forward);
            RaycastHit Hit;

            if (Physics.Raycast(Ray, out Hit, 0.9f))
            {
                Buttons[pos] = Hit.collider.GetComponent<Event>().getState(transform.position, false);
            }
            else
            {
                Buttons[pos] = State.ZERO;
            }
        }
    }

    // Spawnt ein neues Event Object
    public IEnumerator Spawn()
    {
        Instantiate(Event);

        yield return new WaitForSeconds(1);

        StartCoroutine(Spawn());
    }
}
