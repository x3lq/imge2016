using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour {

    public enum Modes { Normal,                 // Normaler Modus: Jeder hat so viel Zeit wie er will
                        HateFest,               // Hate Fest: Man hat für jede Eingabe nur eine bestimmte Zeit. Wenn man es bis dahin nicht geschafft hat sich zu erinnern hat man verloren!
                        TotalDestruction        // Total Destruction: Man hat für die komplette Sequenz eine gegebene Zeit. Auch wenn sie länger wird!
                      }

    public Modes Mode;

    private string ActivePlayer;

    // Kontrolle ob schon etwas gemacht wurde
    private bool ActionDone;
    // Letzte Ausgeführte Aktion
    private string LastAction;

    // CoolDown für die nächste Aktion
    private int CoolDownTime;
    private float CoolDown;

    // Gespeicherte Sequenz die beim nächsten Zug zu absolvieren ist
    private List<string> Sequenz;
    // Counter wo man sich gerade in der Sequenz befindet
    private int SequenzCounter;


    // Use this for initialization
    void Start () {
        Mode = Modes.Normal;

        ActivePlayer = "Player 1";

        LastAction = "Begin";

        CoolDownTime = 1;

        CoolDown = 0;

        Sequenz = new List<string>();
        SequenzCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(CoolDown > 0) { CoolDown -= Time.deltaTime; }

        InputListener();

        GameLogic();
	}

    // Display of Information
    void OnGUI()
    {
        // Style
        GUIStyle style = new GUIStyle();
        GUI.color = Color.black;
        style.fontSize = 30;

        // Action Output
        if (SequenzCounter == 0 || CoolDown > 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - (LastAction.Length * 15) / 2, Screen.height / 2, 100, 50), LastAction, style);
        }

        // Active Player Visualization
        GUI.Label(new Rect(Screen.width / 2 - (ActivePlayer.Length * 15) / 2, Screen.height - 50, 100, 50), ActivePlayer, style);

        // Sequenz
        GUI.Label(new Rect(50, 50, 100, 50), "Sequenzlänge: " + Sequenz.Count, style);
        // Sequenz Counter
        GUI.Label(new Rect(50, 80, 100, 50), "Sequenzcounter: " + SequenzCounter, style);

        // Mode
        GUI.Label(new Rect(Screen.width - Mode.ToString().Length * 15 - 50, 50, 100, 50), Mode.ToString(), style);
    }


    // Input muss noch an Controller angepasst werden!
    private void InputListener()
    {
        if (Input.GetKeyDown(KeyCode.A)) { LastAction = "A"; CoolDown = CoolDownTime; ActionDone = true; }
        if (Input.GetKeyDown(KeyCode.S)) { LastAction = "S"; CoolDown = CoolDownTime; ActionDone = true; }
        if (Input.GetKeyDown(KeyCode.D)) { LastAction = "D"; CoolDown = CoolDownTime; ActionDone = true; }
        if (Input.GetKeyDown(KeyCode.F)) { LastAction = "F"; CoolDown = CoolDownTime; ActionDone = true; }
    }

    private void GameLogic()
    {
        // Erste Eingabe muss gemacht werden
        if(Sequenz.Count == 0 && ActionDone)
        {
            // Aktion zu Sequenz hinzufügen
            Sequenz.Add(LastAction);

            ActionDone = false;
            
            SequenzCounter++;

            // Player Switch
            if(ActivePlayer == "Player 1") { ActivePlayer = "Player 2"; }
            else { ActivePlayer = "Player 1"; }
        }
        else
        {
            // Sequenz noch nicht richtig wiedergegeben
            if (ActionDone && SequenzCounter - 1 < Sequenz.Count)
            {
                // Aktion war richtig
                if (LastAction == Sequenz[SequenzCounter - 1])
                {
                    ActionDone = false;

                    SequenzCounter++;
                }

                // Aktion war falsch
                if (ActionDone && LastAction != Sequenz[SequenzCounter - 1])
                {
                    ActionDone = false;

                    LastAction = "Begin";

                    SequenzCounter = 0;
                    Sequenz = new List<string>();
                }
            }
            else
            {
                // Sequenz vollständig durchlaufen und neue Aktion hinzugefügt
                if (ActionDone)
                {
                    ActionDone = false;

                    Sequenz.Add(LastAction);
                    SequenzCounter = 1;

                    // Player Switch
                    if (ActivePlayer == "Player 1") { ActivePlayer = "Player 2"; }
                    else { ActivePlayer = "Player 1"; }
                }
            }
        }
    }
}
