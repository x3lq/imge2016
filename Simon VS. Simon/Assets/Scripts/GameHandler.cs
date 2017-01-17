using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour {

    public enum Modes { Normal,                 // Normaler Modus: Jeder hat so viel Zeit wie er will
        TimeTrial,
        HateFest,               // Hate Fest: Man hat für jede Eingabe nur eine bestimmte Zeit. Wenn man es bis dahin nicht geschafft hat sich zu erinnern hat man verloren!
        TotalDestruction        // Total Destruction: Man hat für die komplette Sequenz eine gegebene Zeit. Auch wenn sie länger wird!
    }

    public Modes Mode;

    // TimeTrial
    private float ExtraTime;
    private float TimeCurrent;
    private float TimeToBeat;
    private bool measureTime;

    // HateFest
    private int HateTime;
    private float Hate;
    private bool Hating;

    // Total Destruction
    public int TDTime;

    // Health Bar
    public bool TekkenStyle;

    // Players
    private string ActivePlayer;
    private int P1Score, P2Score;
    private int P1Health, P2Health;


    // Kontrolle ob schon etwas gemacht wurde
    private bool ActionDone;
    // Letzte Ausgeführte Aktion
    private string LastAction;

    // CoolDown für die nächste Aktion
    private int CoolDownTime;
    private float CoolDown;

    // Gespeicherte Sequenz die beim nächsten Zug zu absolvieren ist
    public static List<string> SequenzAlt;
    public static List<ControllerElement> Sequenz;
    public int SCount;
    // Counter wo man sich gerade in der Sequenz befindet
    private int SequenzCounter;


    // Use this for initialization
    void Start() {
        //Mode = Modes.Normal;

        ExtraTime = 2;

        HateTime = 3;
        Hating = false;

        ActivePlayer = "Player 1";
        P1Health = P2Health = 100;

        LastAction = "Begin";

        CoolDownTime = 1;

        CoolDown = 0;

        SequenzAlt = new List<string>();
        Sequenz = new List<ControllerElement>();
        SequenzCounter = 0;

        StartCoroutine(Game());
    }

    // Update is called once per frame
    void Update() {
        if (CoolDown > 0) { CoolDown -= Time.deltaTime; }
        if (Hating) { Hate -= Time.deltaTime; }
        if (measureTime) { TimeCurrent += Time.deltaTime; }

        SCount = Sequenz.Count;

        InputListener();

        //GameLogic();
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
        if (!TekkenStyle)
        {
            // P1 Points
            GUI.Label(new Rect(50, Screen.height - 50, 100, 50), "P1: " + P1Score, style);
            // P2 Points
            GUI.Label(new Rect(Screen.width - 130, Screen.height - 50, 100, 50), "P2: " + P2Score, style);
        }
        else
        {
            // P1 Points
            GUI.Label(new Rect(50, Screen.height - 50, 100, 50), "P1: " + P1Health, style);
            // P2 Points
            GUI.Label(new Rect(Screen.width - 130, Screen.height - 50, 100, 50), "P2: " + P2Health, style);
        }

        // Sequenz
        GUI.Label(new Rect(50, 50, 100, 50), "Fortschritt: " + SequenzCounter + "/" + Sequenz.Count, style);

        // Mode
        GUI.Label(new Rect(Screen.width - Mode.ToString().Length * 15 - 50, 50, 100, 50), Mode.ToString(), style);

        // Mode Extras
        if (Mode == Modes.TimeTrial || Mode == Modes.TotalDestruction)
        {
            GUI.Label(new Rect(Screen.width - TimeCurrent.ToString().Length * 15 - 50, 100, 100, 50), TimeCurrent.ToString(), style);
            GUI.Label(new Rect(Screen.width - TimeToBeat.ToString().Length * 15 - 50, 150, 100, 50), TimeToBeat.ToString(), style);
        }
        if (Mode == Modes.HateFest)
        {
            GUI.Label(new Rect(Screen.width - 80, Screen.height / 2, 100, 50), Hate.ToString(), style);
        }
    }
    
    // Input muss noch an Controller angepasst werden!
    private void InputListener()
    {
        if (Input.GetKeyDown(KeyCode.A)) { LastAction = "A"; CoolDown = CoolDownTime; ActionDone = true; }
        if (Input.GetKeyDown(KeyCode.S)) { LastAction = "S"; CoolDown = CoolDownTime; ActionDone = true; }
        if (Input.GetKeyDown(KeyCode.D)) { LastAction = "D"; CoolDown = CoolDownTime; ActionDone = true; }
        if (Input.GetKeyDown(KeyCode.F)) { LastAction = "F"; CoolDown = CoolDownTime; ActionDone = true; }

        if (Input.GetKeyDown(KeyCode.J)) { LastAction = "J"; CoolDown = CoolDownTime; ActionDone = true; }
        if (Input.GetKeyDown(KeyCode.K)) { LastAction = "K"; CoolDown = CoolDownTime; ActionDone = true; }
    }

    // Normal Game Mode
    private IEnumerator Game()
    {
        bool correct = true;
        SequenzCounter = 0;

        // Sequenz wiederholen
        while (correct && SequenzCounter < Sequenz.Count)
        {
            yield return new WaitUntil(() => ActionDone || Hate <= 0 || TimeCurrent > TimeToBeat);

            Debug.Log("Continue");

            if (Mode == Modes.HateFest) { Hating = true; }
            if (Mode == Modes.TimeTrial || Mode == Modes.TotalDestruction) { measureTime = true; }

            ActionDone = false;

            ControllerElement Last = new ControllerElement(LastAction);

            if(Hate <= 0) { Last = new ControllerElement("HATEFEST"); }
            if(TimeCurrent > TimeToBeat) { Debug.Log("OVERTIME"); Last = new ControllerElement("TIMETRIAL"); }

            if (Sequenz[SequenzCounter].Equals(Last))
            {
                // Korrekte Eingabe
                Hate = HateTime;

                Debug.Log("Correct!");
                SequenzCounter++;
            }
            else
            {
                correct = WrongInput();
            }
        }

        // Hatefest Mode
        Hating = false;
        Hate = HateTime;

        // TimeTrial Mode
        if (Mode == Modes.TimeTrial)
        {
            Debug.Log("Measured Time...");
            if (correct)
            {
                TimeToBeat = TimeCurrent + ExtraTime;
            }
            else
            {
                TimeToBeat = 0;
            }
            TimeCurrent = 0;
            measureTime = false;
        }
        if(Mode == Modes.TotalDestruction)
        {
            TimeToBeat = TDTime;
        }

        if (correct)
        {
            // Element hinzufügen
            yield return new WaitUntil(() => ActionDone || TimeCurrent > TimeToBeat);
            if (TimeCurrent <= TimeToBeat)
            {
                ActionDone = false;

                Sequenz.Add(new ControllerElement(LastAction));

                SequenzCounter++;

                Debug.Log("Added: " + LastAction);
            }
            else
            {
                Debug.Log("Time is Over!");
                WrongInput();
            }
        }

        if (Mode == Modes.TotalDestruction)
        {
            Debug.Log("Measured Time...");
            TimeToBeat = TDTime;
            TimeCurrent = 0;
            measureTime = false;
        }

        // Ende der Sequenz

        yield return new WaitForSeconds(1);

        Debug.Log("RESET");

        LastAction = "Begin";
        ActionDone = false;

        // Player Switch
        if (ActivePlayer == "Player 1") { ActivePlayer = "Player 2"; }
        else { ActivePlayer = "Player 1"; }

        StartCoroutine(Game());
    }

    private bool WrongInput()
    {
        // Falsche Eingabe
        Debug.Log("Wrong!");
        if (ActivePlayer == "Player 1") { P2Score++; P1Health -= Sequenz.Count - SequenzCounter; }
        else { P1Score++; P2Health -= Sequenz.Count - SequenzCounter; }
        
        SequenzCounter = 0;
        Sequenz = new List<ControllerElement>();

        return false;
    }
}
