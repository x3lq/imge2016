using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

    Coroutine GameCo;
    Coroutine ResetCo;

    Controller C;
    public bool k1, k2, s1, s2;
    public bool resettingController;

    public string AnzeigeMid;
    public bool Mid;

    public GameObject Controller_Plate;
    public Replay Replay;

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
    public string LastAction;
    private int LastID;
    private int LastPos;

    // CoolDown für die nächste Aktion
    private int CoolDownTime;
    private float CoolDown;

    // Gespeicherte Sequenz die beim nächsten Zug zu absolvieren ist
    public static List<string> SequenzAlt;
    public static List<ControllerElement> Sequenz;
    public int SCount;
    // Counter wo man sich gerade in der Sequenz befindet
    private int SequenzCounter;

    public UnityEvent ReplayEvent;
    public UnityEvent ResetEvent;
    public UnityEvent LEDEvent;


    // Use this for initialization
    void Start() {
        Debug.Log(Options.ShowReplay);

        Mode = (Modes)Options.GameMode;

        Replay = Controller_Plate.GetComponent<Replay>();

        k1 = true;
        k2 = true;

        StartCoroutine(ControllerInit());

        ExtraTime = 2;

        HateTime = 3;
        Hating = false;

        ActivePlayer = "Player 1";
        P1Health = P2Health = 100;

        CoolDownTime = 1;

        CoolDown = 0;

        SequenzAlt = new List<string>();
        Sequenz = new List<ControllerElement>();
        SequenzCounter = 0;

        ResetEvent.Invoke();

        ResetCo = StartCoroutine(WaitForControllerReset());
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

        if (Mid)
        {
            GUI.Label(new Rect(Screen.width / 2 - (AnzeigeMid.Length * 15) / 2, Screen.height / 2, 100, 50), AnzeigeMid, style);
        }

        // Active Player Visualization
        //GUI.Label(new Rect(Screen.width / 2 - (ActivePlayer.Length * 15) / 2, Screen.height - 50, 100, 50), ActivePlayer, style);
        if (!TekkenStyle)
        {
            // P1 Points
            GUI.Label(new Rect(50, Screen.height/2, 100, 50), "P1: " + P1Score, style);
            // P2 Points
            GUI.Label(new Rect(Screen.width - 130, Screen.height/2, 100, 50), "P2: " + P2Score, style);
        }
        else
        {
            // P1 Points
            GUI.Label(new Rect(50, Screen.height/2, 100, 50), "P1: " + P1Health, style);
            // P2 Points
            GUI.Label(new Rect(Screen.width - 130, Screen.height/2, 100, 50), "P2: " + P2Health, style);
        }

        // Sequenz
        //GUI.Label(new Rect(50, 50, 100, 50), "Fortschritt: " + SequenzCounter + "/" + Sequenz.Count, style);

        // Mode
        //GUI.Label(new Rect(Screen.width - Mode.ToString().Length * 15 - 50, 50, 100, 50), Mode.ToString(), style);

        // Mode Extras
        if (Mode == Modes.TimeTrial || Mode == Modes.TotalDestruction)
        {
            GUI.Label(new Rect(Screen.width - TimeCurrent.ToString().Length * 15 - 50, 100, 100, 50), TimeCurrent.ToString(), style);
            GUI.Label(new Rect(Screen.width - TimeToBeat.ToString().Length * 15 - 50, 150, 100, 50), TimeToBeat.ToString(), style);
        }
        if (Mode == Modes.HateFest)
        {
            GUI.Label(new Rect(Screen.width - 80, Screen.height / 3, 100, 50), (((float)((int)(Hate*100)))/100).ToString(), style);
        }
    }
    
    // Input muss noch an Controller angepasst werden!
    private void InputListener()
    {
        // Exit
        if (Input.GetKeyDown(KeyCode.Escape)) { C.LEDOFF(); C.CloseStream(); SceneManager.LoadScene(0); }

        // Game Input
        if (!resettingController && !Replay.replaying)
        {
            if (C == null)
            {
                if (Input.GetKeyDown(KeyCode.A)) { LastAction = "Button"; LastPos = -1; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; }
                if (Input.GetKeyDown(KeyCode.S)) { LastAction = "Button"; LastPos = -1; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; }

                if (Input.GetKeyDown(KeyCode.D)) { LastAction = "Slider"; LastPos = 0; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; }
                if (Input.GetKeyDown(KeyCode.F)) { LastAction = "Slider"; LastPos = 1; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; }

                if (Input.GetKeyDown(KeyCode.J)) { LastAction = "Knob"; LastPos = 0; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; }
                if (Input.GetKeyDown(KeyCode.K)) { LastAction = "Knob"; LastPos = 1; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; }
            }
            else
            {
                // Buttons
                if (C.b1pressed) { LastAction = "Button"; LastPos = -1; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; Replay.buttons[0].Invoke(); }
                if (C.b2pressed) { LastAction = "Button"; LastPos = -1; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; Replay.buttons[1].Invoke(); }
                if (C.b3pressed) { LastAction = "Button"; LastPos = -1; LastID = 2; CoolDown = CoolDownTime; ActionDone = true; Replay.buttons[2].Invoke(); }
                if (C.b4pressed) { LastAction = "Button"; LastPos = -1; LastID = 3; CoolDown = CoolDownTime; ActionDone = true; Replay.buttons[3].Invoke(); }
                if (C.b5pressed) { LastAction = "Button"; LastPos = -1; LastID = 4; CoolDown = CoolDownTime; ActionDone = true; Replay.buttons[4].Invoke(); }
                if (C.b6pressed) { LastAction = "Button"; LastPos = -1; LastID = 5; CoolDown = CoolDownTime; ActionDone = true; Replay.buttons[5].Invoke(); }
                
                // Knobs
                if (!k1 && C.knob1 >= 0.75f) { k1 = true; LastPos = 0; LastAction = "Knob"; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; Replay.rotation01[0].Invoke(); }
                else
                {
                    if (k1 && C.knob1 <= 0.25f) { k1 = false; LastPos = 2; LastAction = "Knob"; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; Replay.rotation01[2].Invoke(); }
                }

                if (!k2 && C.knob2 >= 0.75f) { k2 = true; LastPos = 0; LastAction = "Knob"; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; Replay.rotation02[0].Invoke(); }
                else
                {
                    if (k2 && C.knob2 <= 0.25f) { k2 = false; LastPos = 2; LastAction = "Knob"; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; Replay.rotation02[2].Invoke(); }
                }

                // Sliders
                if (!s1 && C.slider1 >= 0.75f) { s1 = true; LastPos = 2; LastAction = "Slider"; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; Replay.slider01[2].Invoke(); }
                else
                {
                    if (s1 && C.slider1 <= 0.25f) { s1 = false; LastPos = 0; LastAction = "Slider"; LastID = 0; CoolDown = CoolDownTime; ActionDone = true; Replay.slider01[0].Invoke(); }
                }

                if (!s2 && C.slider2 >= 0.75f) { s2 = true; LastPos = 2; LastAction = "Slider"; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; Replay.slider02[2].Invoke(); }
                else
                {
                    if (s2 && C.slider2 <= 0.25f) { s2 = false; LastPos = 0; LastAction = "Slider"; LastID = 1; CoolDown = CoolDownTime; ActionDone = true; Replay.slider02[0].Invoke(); }
                }
            }
        }
    }

    // Normal Game Mode
    private IEnumerator Game()
    {
        Mid = true;
        AnzeigeMid = "Begin";
        bool correct = true;
        SequenzCounter = 0;

        
        Debug.Log("Sequenz Check...");
               
        // Sequenz wiederholen
        while (correct && SequenzCounter < Sequenz.Count)
        {
            yield return new WaitUntil(() => ActionDone || Hate <= 0 || TimeCurrent > TimeToBeat);

            Mid = false;

            Debug.Log("Action has been done!");

            if (Mode == Modes.HateFest) { Hating = true; }
            if (Mode == Modes.TimeTrial || Mode == Modes.TotalDestruction) { measureTime = true; }

            ActionDone = false;
            
            ControllerElement Last = new ControllerElement(LastAction, LastPos, LastID);

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
                StartCoroutine(WrongInput());
                correct = false;
            }
        }

        Debug.Log("Sequenz Check Finished!");
        StartCoroutine(C.Vibrate(1, 0.1f, 0, 500));



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

        // Total Destruction
        if(Mode == Modes.TotalDestruction)
        {
            TimeToBeat = TDTime;
        }



        // Korrekte Eingabe wird Überprüft
        if (correct)
        {
            Mid = true;
            AnzeigeMid = "Add Action";
            // Element hinzufügen
            yield return new WaitUntil(() => ActionDone || TimeCurrent > TimeToBeat);
            if (TimeCurrent <= TimeToBeat)
            {
                ActionDone = false;

                Sequenz.Add(new ControllerElement(LastAction, LastPos, LastID));

                SequenzCounter++;

                Debug.Log("Added: " + LastAction);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                LEDEvent.Invoke();
            }
            else
            {
                Debug.Log("Time is Over!");
                StartCoroutine(WrongInput());
            }
            Mid = false;
        }


        // Total Destruction
        if (Mode == Modes.TotalDestruction)
        {
            Debug.Log("Measured Time...");
            TimeToBeat = TDTime;
            TimeCurrent = 0;
            measureTime = false;
        }



        // Replay Der Kompletten Sequenz
        if (Options.ShowReplay)
        {
            Debug.Log("Replay...");

            yield return new WaitForSeconds(1);

            Mid = true;
            AnzeigeMid = "Replay";

            Replay.replaying = true;

            yield return new WaitForSeconds(Replay.waitTime + 0.1f);

            Replay.Reset();
            yield return new WaitForSeconds(Replay.waitTime);

            ReplayEvent.Invoke();

            yield return new WaitUntil(() => !Replay.replaying);

            ResetEvent.Invoke();

            Mid = false;
        }
        else
        {
            yield return new WaitForSeconds(1);
            ResetEvent.Invoke();
        }


        // Ende des Replays
        yield return new WaitForSeconds(1);

        ActionDone = false;

        // Player Switch
        if (ActivePlayer == "Player 1") { ActivePlayer = "Player 2"; }
        else { ActivePlayer = "Player 1"; }

        ResetCo = StartCoroutine(WaitForControllerReset());
    }
    
    private IEnumerator WrongInput()
    {
        // Falsche Eingabe
        Debug.Log("Wrong!");
        StartCoroutine(C.Vibrate(1, 0.5f, 0, 900));

        if (ActivePlayer == "Player 1") { P2Score++; P1Health -= Sequenz.Count - SequenzCounter; }
        else { P1Score++; P2Health -= Sequenz.Count - SequenzCounter; }
        
        SequenzCounter = 0;
        Sequenz = new List<ControllerElement>();

        Mid = true;
        AnzeigeMid = ActivePlayer + " f*cked up!";

        StopCoroutine(GameCo);
        StopCoroutine(ResetCo);
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(0);
    }

    public static void log(string logMessage)
    {
        Debug.Log(logMessage);
    }
    
    IEnumerator ControllerInit()
    {
        yield return new WaitUntil(() => Controller.c != null);
        C = Controller.c;
        C.LEDOFF();
    }

    IEnumerator WaitForControllerReset()
    {
        yield return new WaitUntil(() => C != null);

        resettingController = true;

        Mid = true;
        AnzeigeMid = "Reset Controller";

        Debug.Log("Resetting Controller");

        yield return new WaitUntil(() => C.slider1 <= 0.25f && C.slider2 <= 0.25f && C.knob1 >= 0.75f && C.knob2 >= 0.75f);
        //yield return new WaitUntil(() => C.slider2 <= 0.25f);
        //yield return new WaitUntil(() => C.knob1 >= 0.75f);
        //yield return new WaitUntil(() => C.knob2 >= 0.75f);

        k1 = true;
        k2 = true;
        s1 = false;
        s2 = false;

        yield return new WaitForSeconds(0.5f);

        Mid = false;

        Debug.Log("Controller is ready!");
        resettingController = false;
        ActionDone = false;

        GameCo = StartCoroutine(Game());
    }
}
