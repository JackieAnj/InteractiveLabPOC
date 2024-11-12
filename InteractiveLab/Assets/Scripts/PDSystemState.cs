using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PDSystemState : MonoBehaviour
{
    private int state = -1;
    private int oldState = -1;
    public Text statusUI;
    public Text timer; // time to be displayed in game
    public Text score; // score to be displayed in game
    public GameObject partOneText;
    public GameObject partTwoText;
    public GameObject partThreeText;
    public GameObject endScreen;
    public Text finalScore; // final score to be displayed in end screen
    public Text timePlayed; // final time to be displayed in end screen
    private float timeStart; // internal time variable
    private float currentScore; // internal score variable
    private Transform textContent;
    private TextMeshProUGUI content;
    private int section = 1;
    private TwoWayValve[] twoWayValves;
    private ThreeWayValve[] threeWayValves;
    private CircleValve[] circleValves;
    private PRVValve[] PRVs;
    private InfoGauge[] infoGauges;
    private Boolean partOneComplete = false;
    private Boolean partTwoComplete = false;
    private Boolean partThreeComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        timer.text = "Time: 00:00.00";
        score.text = "Score: 0";
        twoWayValves = UnityEngine.Object.FindObjectsOfType<TwoWayValve>();
        threeWayValves = UnityEngine.Object.FindObjectsOfType<ThreeWayValve>();
        circleValves = UnityEngine.Object.FindObjectsOfType<CircleValve>();
        PRVs = UnityEngine.Object.FindObjectsOfType<PRVValve>();
        textContent = partOneText.transform;
        infoGauges = UnityEngine.Object.FindObjectsOfType<InfoGauge>();
    }

    // Update is called once per frame
    void Update()
    {
        if (partOneComplete && partTwoComplete && partThreeComplete) {
            TimeSpan timePlaying = TimeSpan.FromSeconds(timeStart);
            finalScore.text = "Score: " + currentScore;
            timePlayed.text = "Time Played: " + timePlaying.ToString("mm':'ss'.'ff");
            endScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        } else {
            timeStart += Time.deltaTime;
            TimeSpan timePlaying = TimeSpan.FromSeconds(timeStart);
            timer.text = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
        }

        updateScore();
    }

    void OnDisable() {
        restart();
    }

    public void updateScore() {
        float newScore = state * 100;
        if (section == 2) {
            newScore += 900;
        } else if (section == 3) {
            newScore += 1700;
        }

        if (newScore > currentScore) {
            currentScore = newScore;
            score.text = "Score: " + currentScore;
        }
    }

    public void restart() {
        partOneComplete = false;
        partTwoComplete = false;
        partThreeComplete = false;
        timeStart = 0;
        currentScore = 0;
        endScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        updateScore();
    }

    public void onChange() {
        oldState = state;

        if (section == 1) {
            partOne();
        } else if (section == 2) {
            partTwo();
        } else if (section == 3) {
            partThree();
        }

        if (oldState > state) {
            FindObjectOfType<SoundManager>().Play("Error");
        } else if (oldState < state) {
            FindObjectOfType<SoundManager>().Play("Correct");
        }
    }
    
    // Helper method to set state and update UI text
    private void SetState(int newState, string gauge1Value = null, string gauge2Value = null)
    {
        state = newState;
        statusUI.text = $"Step {newState}";
        if (gauge1Value != null) updateGaugeValue("Temp", int.Parse(gauge1Value));
        if (gauge2Value != null) updateGaugeValue("PI802", int.Parse(gauge2Value));
    }
    
    private void ExecuteSteps(List<(int currState, Func<bool> condition, Action action)> steps)
    {
        foreach (var (currState, condition, action) in steps)
        {
            Debug.Log($"state {currState} condition {condition} which is {condition()} ");
            if (!condition()) break;
            action();
        }
        updateStatus();
    }

    private void partOne() {
        Debug.Log("In Part One");

        var steps = new List<(int currState, Func<bool> condition, Action action)>
        {
            (1, () => checkPosition("HV700", Position.left), () => { SetState(1); updateGaugeValue("Temp", 10); }),
            (2, () => checkPosition("HV701", Position.left), () => SetState(2)),
            (3, () => checkTurn("FIC703", 1), () => SetState(3)),
            (4, () => checkTurn("HV806", 1), () => SetState(4)),
            (5, () => checkTurn("PRV807", 1), () => { SetState(5); updateGaugeValue("PI802", 10); }),
            (6, () => checkCircle("HV704") && checkCircle("HV705"), () => SetState(6)),
            (7, () => checkOpen("HV901"), CompletePartOne)
        };

        ExecuteSteps(steps);
    }

    private void partTwo() {
        Debug.Log("In Part Two");

        var steps = new List<(int currState, Func<bool> condition, Action action)>
        {
            (1, () => checkOpen("HV203"), () => SetState(1)),
            (2, () => checkTurn("FIC204", 1), () => SetState(2)),
            (3, () => !checkOpen("HV403") && checkOpen("HV402") && checkOpen("HV404"), () => SetState(3)),
            (4, () => checkTurn("FIC401", 1), () => SetState(4)),
            (5, () => checkOpen("HV802"), () => SetState(5)),
            (6, () => checkTurn("PRV803", 1), () => { SetState(6); updateGaugeValue("PI801", 3); CompletePartTwo(); })
        };

        ExecuteSteps(steps);
    }

    private void partThree() {
        Debug.Log("In Shutdown Procedure");
        
        statusUI.text = "Shutdown Procedure";
        
        var steps = new List<(int currState, Func<bool> condition, Action action)>
        {
            (1, () => !checkOpen("FIC401"), () => SetState(1)),
            (2, () => checkTurn("FIC204", 0), () => SetState(2)),
            (3, () => !checkOpen("HV203"), () => SetState(3)),
            (4, () => !checkOpen("HS201"), () => SetState(4)),
            (5, () => checkOpen("HV303"), () => SetState(5)),
            (6, () => !checkOpen("HV402") && checkOpen("HV403") && checkTurn("HS301", 1), () => SetState(6)),
            (7, () => !checkOpen("FIC703") && !checkOpen("HV701"), CompletePartThree)
        };

        ExecuteSteps(steps);
    }
    
    private void CompletePartOne()
    {
        SetState(7);
        partOneComplete = true;
        statusUI.text = "Part 1 Complete! Press [C] to perform part 2";
    }

    private void CompletePartTwo()
    {
        SetState(6);
        partTwoComplete = true;
        statusUI.text = "Part 2 Complete! Press [C] to perform shutdown procedure";
    }

    private void CompletePartThree()
    {
        SetState(7);
        partThreeComplete = true;
        statusUI.text = "Shutdown Procedure Complete!";
    }

    // check if a two way valve is open given valve id
    private bool checkOpen(string id) {
        return Array.Find(twoWayValves, v => v.id == id).open;
    }

    // check if a three way valve is in the right position given valve id and target position
    private bool checkPosition(string id, Position p) {
        return Array.Find(threeWayValves, v => v.id == id).position == p;
    }

    // check if a circle valve is open given valve id
    private bool checkCircle(string id) {
        return Array.Find(circleValves, v => v.id == id).open;
    }

    // check if a PRV has at least x number of turns
    private bool checkTurn(string id, int turn) {
        return Array.Find(PRVs, v => v.id == id).turn >= turn;
    }

    // update the value of an info gauge
    private void updateGaugeValue(string id, int value) {
        InfoGauge target = Array.Find(infoGauges, g => g.id == id);
        target.updateValue(value);
    }

    private void updateStatus() {
        Debug.Log($"Update status, current state {state}");
        int index = 0;
        
        for (int i = 1; i < textContent.transform.childCount; ++i) {
            index++;
            Transform instruction = textContent.transform.GetChild(i);
            content = instruction.GetComponent<TextMeshProUGUI>();

            if (index <= state) {
                content.fontStyle = FontStyles.Strikethrough;
                Debug.Log("Strikethrough");
            } else {
                content.fontStyle = FontStyles.Normal;
            }
        }
    }
}
