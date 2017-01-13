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
    public static List<string> SequenzAlt;
    public static List<ControllerElement> Sequenz;
    public int SCount;
    // Counter wo man sich gerade in der Sequenz befindet
    private int SequenzCounter;


    // Use this for initialization
    void Start() {
        Mode = Modes.Normal;

        ActivePlayer = "Player 1";

        LastAction = "Begin";

        CoolDownTime = 1;

        CoolDown = 0;

        SequenzAlt = new List<string>();
        Sequenz = new List<ControllerElement>();
        SequenzCounter = 0;

        switch (Mode)
        {
            case Modes.Normal:
                StartCoroutine(Normal());
                break;
            case Modes.HateFest:
                break;
            case Modes.TotalDestruction:
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if (CoolDown > 0) { CoolDown -= Time.deltaTime; }

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

        // Sequenz
        GUI.Label(new Rect(50, 50, 100, 50), "Fortschritt: " + SequenzCounter + "/" + Sequenz.Count, style);

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

    // Normal Game Mode
    private IEnumerator Normal()
    {
        bool correct = true;
        SequenzCounter = 0;

        // Sequenz wiederholen
        while (correct && SequenzCounter < Sequenz.Count)
        {
            yield return new WaitUntil(() => ActionDone);

            ActionDone = false;

            ControllerElement Last = new ControllerElement(LastAction);

            if (Sequenz[SequenzCounter].Equals(Last))
            {
                Debug.Log("Correct!");
                SequenzCounter++;
            }
            else
            {
                correct = false;
                SequenzCounter = 0;
                Sequenz = new List<ControllerElement>();
                LastAction = "Begin";
            }
        }

        // Element hinzufügen
        yield return new WaitUntil(() => ActionDone);

        ActionDone = false;

        Sequenz.Add(new ControllerElement(LastAction));

        // Player Switch
        if (ActivePlayer == "Player 1") { ActivePlayer = "Player 2"; }
        else { ActivePlayer = "Player 1"; }

        StartCoroutine(Normal());
    }
}
