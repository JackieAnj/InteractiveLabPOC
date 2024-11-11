using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HXSystemState : MonoBehaviour
{
    private int state = -1;
    private int oldState = -1;

    public GameObject CondensationTrapOne;
    public GameObject CondensationTrapTwo;
    public Text statusUI;
    public Text timer; // time to be displayed in game
    public Text timerVR;
    public Text score; // score to be displayed in game
    public GameObject partOneText;
    public GameObject partTwoText;
    public GameObject partThreeText;
    public GameObject shutdownProcedureText;
    public GameObject endScreen;
    public MeterValues meters;
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
    private Boolean shutDownComplete = false;

    private void Start() {
        timer.text = "Time: 00:00.00";
        timerVR.text = timer.text;
        score.text = "Score: 0";
        twoWayValves = UnityEngine.Object.FindObjectsOfType<TwoWayValve>();
        threeWayValves = UnityEngine.Object.FindObjectsOfType<ThreeWayValve>();
        circleValves = UnityEngine.Object.FindObjectsOfType<CircleValve>();
        PRVs = UnityEngine.Object.FindObjectsOfType<PRVValve>();
        infoGauges = UnityEngine.Object.FindObjectsOfType<InfoGauge>();
        textContent = partOneText.transform;

        StartupCheck();
    }

    public void ChangeTube() {
        partOneText.SetActive(true);
        partTwoText.SetActive(false);
        partThreeText.SetActive(false);
        shutdownProcedureText.SetActive(false);
        ResetValves();
        section = 1;
        state = 1;
        textContent = partOneText.transform;
    }

    public void ChangePlate() {
        partOneComplete = true;
        partOneText.SetActive(false);
        partTwoText.SetActive(true);
        partThreeText.SetActive(false);
        shutdownProcedureText.SetActive(false);
        ResetValves();
        section = 2;
        state = 1;
        textContent = partTwoText.transform;
    }

    public void ChangeColumn() {
        partOneComplete = true;
        partTwoComplete = true;
        partOneText.SetActive(false);
        partTwoText.SetActive(false);
        partThreeText.SetActive(true);
        shutdownProcedureText.SetActive(false);
        ResetValves();
        section = 3;
        state = 1;
        textContent = partThreeText.transform;
    }

    private void Update() {
        if (partOneComplete && partTwoComplete && partThreeComplete && shutDownComplete) {
            TimeSpan timePlaying = TimeSpan.FromSeconds(timeStart);
            finalScore.text = "Score: " + currentScore;
            timePlayed.text = "Time Played: " + timePlaying.ToString("mm':'ss'.'ff");
            endScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        } else {
            timeStart += Time.deltaTime;
            TimeSpan timePlaying = TimeSpan.FromSeconds(timeStart);
            timer.text = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timerVR.text = timer.text;
        }

        UpdateScore();

        // for testing only (completes all procedures)
        if (Input.GetKeyDown("v")) {
            partOneComplete = true;
            partTwoComplete = true;
            partThreeComplete = true;
            shutDownComplete = true;
        }

        // completes individual procedures
        if (Input.GetKeyDown("j")) {
            partOneText.SetActive(true);
            partTwoText.SetActive(false);
            partThreeText.SetActive(false);
            shutdownProcedureText.SetActive(false);
            ResetValves();
            section = 1;
            state = 1;
            textContent = partOneText.transform;
        } else if (Input.GetKeyDown("k")) {
            partOneComplete = true;
            partOneText.SetActive(false);
            partTwoText.SetActive(true);
            partThreeText.SetActive(false);
            shutdownProcedureText.SetActive(false);
            ResetValves();
            section = 2;
            state = 1;
            textContent = partTwoText.transform;
        } else if (Input.GetKeyDown("l")) {
            partOneComplete = true;
            partTwoComplete = true;
            partOneText.SetActive(false);
            partTwoText.SetActive(false);
            partThreeText.SetActive(true);
            shutdownProcedureText.SetActive(false);
            ResetValves();
            section = 3;
            state = 1;
            textContent = partThreeText.transform;
        }

        if (Input.GetKeyDown("c")) {
            if (
                (section == 1 && state == 9) || 
                (section == 2 && state == 8) ||
                (section == 3 && state == 11)
            ){
                section = 4;
                textContent = shutdownProcedureText.transform;
                partOneText.SetActive(false);
                partTwoText.SetActive(false);
                partThreeText.SetActive(false);
                shutdownProcedureText.SetActive(true);
                Shutdown();
            } else if (section == 4 && state == 6){
                if (partOneComplete && partTwoComplete && partThreeComplete) {
                    shutDownComplete = true;
                }
                if (partOneComplete && partTwoComplete) {
                    section = 3;
                    textContent = partThreeText.transform;
                    partOneText.SetActive(false);
                    partTwoText.SetActive(false);
                    partThreeText.SetActive(true);
                    shutdownProcedureText.SetActive(false);
                    PartThree();
                } else if (partOneComplete) {
                    section = 2;
                    textContent = partTwoText.transform;
                    partOneText.SetActive(false);
                    partTwoText.SetActive(true);
                    partThreeText.SetActive(false);
                    shutdownProcedureText.SetActive(false);
                    PartTwo();
                }
            }
        }
    }

    void OnDisable() {
        Restart();
    }

    public void UpdateScore() {
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

    public void Restart() {
        partOneComplete = false;
        partTwoComplete = false;
        partThreeComplete = false;
        timeStart = 0;
        currentScore = 0;
        endScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        UpdateScore();
    }

    public void OnChange() {
        ClearCondensationCheck();
        oldState = state;

        if (state == -1) {
            StartupCheck();
        }

        if (state > -1) {
            if (section == 1) {
                partOne();
            } else if (section == 2) {
                PartTwo();
            } else if (section == 3) {
                PartThree();
            } else {
                Shutdown();
            }
        }

        if (oldState > state) {
            FindObjectOfType<SoundManager>().Play("Error");
        } else if (oldState < state) {
            FindObjectOfType<SoundManager>().Play("Correct");
        }
    }

    private bool StartupCheck() {
        // check all specified valves are closed
        string[] closedValves = {"V111", "V121", "V128", "V114", "V130", "V132", "V134"};
        foreach(TwoWayValve v in twoWayValves) {
            if (Array.Exists(closedValves, x => x == v.id) && v.open) {
                state = -1;
                statusUI.text = "Start up checklist unmet";
                return false;
            }
        }

        foreach(CircleValve v in circleValves) {
            if (Array.Exists(closedValves, x => x == v.id) && v.open) {
                state = -1;
                statusUI.text = "Start up checklist unmet";
                return false;
            }
        }
        
        // check liquid level in condensation traps
        if (CondensationTrapOne.GetComponent<CondensationTrap>().liquidLevel + CondensationTrapTwo.GetComponent<CondensationTrap>().liquidLevel > 0) {
            state = -1;
            statusUI.text = "Start up checklist unmet";
            return false;
        }

        if (state == -1) {
            state = 0;
        }

        statusUI.text = "Start up checklist met";
        return true;
    }

    private void ClearCondensationCheck() {

        if (CheckOpen("V125")) {
            CondensationTrapOne.GetComponent<CondensationTrap>().ClearLiquidLevel();
        }

        if (CheckOpen("V126")) {
            CondensationTrapTwo.GetComponent<CondensationTrap>().ClearLiquidLevel();
        }
    }

    private void partOne() {
        if (CheckPosition("V122", Position.left)) {
            state = 1;
            statusUI.text = "Part 1: step " + state;
            meters.changeValue(0, "70.0");
            meters.changeValue(1, "6.0");
            if (CheckCircle("V121")) {
                state = 2;
                statusUI.text = "Part 1: step " + state;
                meters.changeValue(0, "78.0");
                meters.changeValue(1, "8.0");
                // updateGaugeValue("TI13", 30);
                if (CheckPosition("V131", Position.left)) {
                    state = 3;
                    statusUI.text = "Part 1: step " + state;
                    if (CheckPosition("V119", Position.left) && CheckPosition("V123", Position.left)) {
                        state = 4;
                        statusUI.text = "Part 1: step " + state;
                        if (CheckPosition("V124", Position.left) && CheckOpen("V125")) {
                            state = 5;
                            statusUI.text = "Part 1: step " + state;
                            if (CheckCircle("V132")) {
                                state = 6;
                                statusUI.text = "Part 1: step " + state;
                                if (CheckTurn("PRV10", 1)) {
                                    state = 7;
                                    statusUI.text = "Part 1: step " + state;
                                    if (CheckCircle("V111")) {
                                        state = 8;
                                        statusUI.text = "Part 1: step " + state;
                                        if (CheckTurn("PRV10", 3)) {
                                            state = 9;
                                            partOneComplete = true;
                                            statusUI.text = "Part 1 Complete! Press [C] to perform shutdown procedure";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
            StartupCheck();
        }

        UpdateStatus();
    }

    private void PartTwo() {
        if (CheckPosition("V122", Position.left)) {
            state = 1;
            statusUI.text = "Part 2: step " + state;
            if (CheckCircle("V121")) {
                state = 2;
                statusUI.text = "Part 2: step " + state;
                if (CheckPosition("V119", Position.left)) {
                    state = 3;
                    statusUI.text = "Part 2: step " + state;
                    if (CheckPosition("V123", Position.left)) {
                        state = 4;
                        statusUI.text = "Part 2: step " + state;
                        if (CheckCircle("V132")) {
                            state = 5;
                            statusUI.text = "Part 2: step " + state;
                            if (CheckTurn("PRV10", 1)) {
                                state = 6;
                                statusUI.text = "Part 2: step " + state;
                                if (CheckCircle("V111")) {
                                    state = 7;
                                    statusUI.text = "Part 2: step " + state;
                                    if (CheckTurn("PRV10", 3)) {
                                        state = 8;
                                        partTwoComplete = true;
                                        statusUI.text = "Part 2 Complete! Press [C] to perform shutdown procedure";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
            StartupCheck();
        }

        UpdateStatus();
    }

    private void PartThree() {
        if (CheckPosition("V112", Position.left) && (CheckPosition("V113", Position.left))) {
            state = 1;
            statusUI.text = "Part 3: step " + state;
            if (!CheckOpen("V115") && !CheckOpen("V116")) {
                state = 2;
                statusUI.text = "Part 3: step " + state;
                if (CheckCircle("V128") && CheckTurn("PRV12", 1)) {
                    state = 3;
                    statusUI.text = "Part 3: step " + state;
                    if (CheckPosition("V131", Position.left)) {
                        state = 4;
                        statusUI.text = "Part 3: step " + state;
                        if (CheckOpen("V126")) {
                            state = 5;
                            statusUI.text = "Part 3: step " + state;
                            if (CheckPosition("V118", Position.left)) {
                                state = 6;
                                statusUI.text = "Part 3: step " + state;
                                if (CheckCircle("V130")) {
                                    state = 7;
                                    statusUI.text = "Part 3: step " + state;
                                    if (CheckTurn("PRV10", 3)) {
                                        state = 8;
                                        statusUI.text = "Part 3: step " + state;
                                        if (CheckTurn("PRV10", 1)) {
                                            state = 9;
                                            statusUI.text = "Part 3: step " + state;
                                            if (CheckCircle("V111")) {
                                                state = 10;
                                                statusUI.text = "Part 3: step " + state;
                                                if (CheckTurn("PRV10", 3)) {
                                                    state = 11;
                                                    partThreeComplete = true;
                                                    statusUI.text = "Part 3 Complete! Press [C] to perform shutdown procedure";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
            StartupCheck();
        }

        UpdateStatus();
    }

    private void Shutdown() {
        statusUI.text = "Shutdown Procedure";
        if (!CheckCircle("V111") && CheckPosition("V112", Position.top) && CheckPosition("V113", Position.top)) {
            state = 1;
            statusUI.text = "Shutdown: step " + state;
            if (!CheckOpen("V115") && !CheckOpen("V116") && CheckPosition("V118", Position.top)) {
                state = 2;
                statusUI.text = "Shutdown: step " + state;
                if (CheckPosition("V122", Position.top) && CheckPosition("V123", Position.top) && CheckPosition("V124", Position.top)) {
                    state = 3;
                    statusUI.text = "Shutdown: step " + state;
                    if (!CheckOpen("V125") && !CheckOpen("V126")) {
                        state = 4;
                        statusUI.text = "Shutdown: step " + state;
                        if (!CheckCircle("V128") && !CheckCircle("V130")) {
                            state = 5;
                            statusUI.text = "Shutdown: step " + state;
                            if (CheckPosition("V131", Position.top) && !CheckCircle("V133") && !CheckCircle("V134")) {
                                state = 6;
                                statusUI.text = "Shutdown Complete! Press [C] to go to next section";
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
        }

        UpdateStatus();
    }

    // reset valves
    private void ResetValves() {
        foreach (var valve in circleValves)
        {
            valve.Reset();
        }

        foreach (var valve in PRVs)
        {
            valve.Reset();
        }

        foreach (var valve in twoWayValves)
        {
            valve.Reset();
        }

        foreach (var valve in threeWayValves)
        {
            valve.Reset();
        }
    }

    // check if a two way valve is open given valve id
    private bool CheckOpen(string id) {
        return Array.Find(twoWayValves, v => v.id == id).open;
    }

    // check if a three way valve is in the right position given valve id and target position
    private bool CheckPosition(string id, Position p) {
        return Array.Find(threeWayValves, v => v.id == id).position == p;
    }

    // check if a circle valve is open given valve id
    private bool CheckCircle(string id) {
        return Array.Find(circleValves, v => v.id == id).open;
    }

    // check if a PRV has at least x number of turns
    private bool CheckTurn(string id, int turn) {
        return Array.Find(PRVs, v => v.id == id).turn >= turn;
    }

    // update the value of an info gauge
    private void UpdateGaugeValue(string id, int value) {
        InfoGauge target = Array.Find(infoGauges, g => g.id == id);
        target.updateValue(value);
    }

    private void UpdateStatus() {
        int index = 0;
        
        for (int i = 1; i < textContent.transform.childCount; ++i) {
            index++;
            Transform instruction = textContent.transform.GetChild(i);
            content = instruction.GetComponent<TextMeshProUGUI>();

            if (index <= state) {
                content.fontStyle = FontStyles.Strikethrough;
            } else {
                content.fontStyle = FontStyles.Normal;
            }
        }
    }
}
