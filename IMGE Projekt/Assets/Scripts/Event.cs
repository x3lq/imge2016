using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour {

    public int Art;
    public int Length;
    public int Speed;
    public bool pressed;

    // Use this for initialization
    void Start () {
        Length = 1;
        pressed = false;
        //RNG();
	}
	
	// Update is called once per frame
	void Update () {
	    if(transform.localScale.y != Length)
        {
            transform.localScale = new Vector3(transform.localScale.x, Length, transform.localScale.z);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime / 2f * Speed, transform.position.z);
	}

    // Random Position bei Spawn
    private void RNG()
    {
        Art = Random.Range(0, 4);
        transform.position = new Vector3(-4 + 2 * Art, 10, 0);
    }

    public void Position(int Art)
    {
        transform.position = new Vector3(-4 + 2 * Art, 5, 0);
    }


    // Gibt den State zurück und ändert den Score falls es ein echter Press war
    public EventHandler.State getState(Vector3 Position, bool Real)
    {
        if (Real)
        {
            pressed = true;
        }

        /* Bereiche des Events:
         * Perfect: 0 - 0.25 -> 10 Punkte
         * Good:    0.25 - 0.5 -> 5 Punkte
         * Bad:     0.5 - 1 -> 1 Punkte
         */
        if(Mathf.Abs(Position.y - gameObject.transform.position.y) <= 0.25f/2f*Length)
        {
            // Perfect
            if (Real)
            {
                EventHandler.Score += 10;
            }
            return EventHandler.State.Perfect;
        }
        else
        {
            if (Mathf.Abs(Position.y - gameObject.transform.position.y) <= 0.5f/2f*Length)
            {
                // Good
                if (Real)
                {
                    EventHandler.Score += 5;
                }
                return EventHandler.State.Good;
            }
            else
            {
                // Bad
                if (Real)
                {
                    EventHandler.Score += 1;
                }
                return EventHandler.State.Bad;
            }
        }
    }
}
