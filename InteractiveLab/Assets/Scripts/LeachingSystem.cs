using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeachingSystem : MonoBehaviour
{
    private int state = -1;
    private int oldState = -1;

    public Text statusUI;
    public Text timer; // time to be displayed in game
    public Text timerVR;
    public Text score; // score to be displayed in game
    public GameObject BStartup;
    public GameObject BProcedure;
    public GameObject BShutDown;
    public GameObject CStartup;
    public GameObject CProcedure;
    public GameObject CShutDown;
    public GameObject DStartup;
    public GameObject DProcedure;
    public GameObject DShutDown;
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
    private Boolean partFourComplete = false;
    private Boolean partFiveComplete = false;
    private Boolean partSixComplete = false;
    private Boolean partSevenComplete = false;
    private Boolean partEightComplete = false;
    private Boolean partNineComplete = false;

    private void Start() {
        timer.text = "Time: 00:00.00";
        timerVR.text = timer.text;
        score.text = "Score: 0";
        twoWayValves = UnityEngine.Object.FindObjectsOfType<TwoWayValve>();
        threeWayValves = UnityEngine.Object.FindObjectsOfType<ThreeWayValve>();
        circleValves = UnityEngine.Object.FindObjectsOfType<CircleValve>();
        PRVs = UnityEngine.Object.FindObjectsOfType<PRVValve>();
        infoGauges = UnityEngine.Object.FindObjectsOfType<InfoGauge>();
        textContent = BStartup.transform;

        startupCheck();
    }

    public void PerformBStartUp() {
        BStartup.SetActive(true);
        BProcedure.SetActive(false);
        BShutDown.SetActive(false);
        CStartup.SetActive(false);
        CProcedure.SetActive(false);
        CShutDown.SetActive(false);
        DStartup.SetActive(false);
        DProcedure.SetActive(false);
        DShutDown.SetActive(false);
        section = 1;
        state = 1;
        textContent = BStartup.transform;
    }

    public void PerformBProcedure() {
        BStartup.SetActive(false);
        BProcedure.SetActive(true);
        BShutDown.SetActive(false);
        CStartup.SetActive(false);
        CProcedure.SetActive(false);
        CShutDown.SetActive(false);
        DStartup.SetActive(false);
        DProcedure.SetActive(false);
        DShutDown.SetActive(false);
        section = 2;
        state = 1;
        textContent = BProcedure.transform;
    }

    public void PerformBShutDown() {
        BStartup.SetActive(false);
        BProcedure.SetActive(false);
        BShutDown.SetActive(true);
        CStartup.SetActive(false);
        CProcedure.SetActive(false);
        CShutDown.SetActive(false);
        DStartup.SetActive(false);
        DProcedure.SetActive(false);
        DShutDown.SetActive(false);
        section = 3;
        state = 1;
        textContent = BShutDown.transform;
    }

    public void PerformCStartUp()
    {
        BStartup.SetActive(false);
        BProcedure.SetActive(false);
        BShutDown.SetActive(false);
        CStartup.SetActive(true);
        CProcedure.SetActive(false);
        CShutDown.SetActive(false);
        DStartup.SetActive(false);
        DProcedure.SetActive(false);
        DShutDown.SetActive(false);
        section = 4;
        state = 1;
        textContent = BStartup.transform;
    }

    public void PerformCProcedure()
    {
        BStartup.SetActive(false);
        BProcedure.SetActive(false);
        BShutDown.SetActive(false);
        CStartup.SetActive(false);
        CProcedure.SetActive(true);
        CShutDown.SetActive(false);
        DStartup.SetActive(false);
        DProcedure.SetActive(false);
        DShutDown.SetActive(false);
        section = 5;
        state = 1;
        textContent = BProcedure.transform;
    }

    public void PerformCShutDown()
    {
        BStartup.SetActive(false);
        BProcedure.SetActive(false);
        BShutDown.SetActive(false);
        CStartup.SetActive(false);
        CProcedure.SetActive(false);
        CShutDown.SetActive(true);
        DStartup.SetActive(false);
        DProcedure.SetActive(false);
        DShutDown.SetActive(false);
        section = 6;
        state = 1;
        textContent = BShutDown.transform;
    }

    public void PerformDStartUp()
    {
        BStartup.SetActive(false);
        BProcedure.SetActive(false);
        BShutDown.SetActive(false);
        CStartup.SetActive(false);
        CProcedure.SetActive(false);
        CShutDown.SetActive(false);
        DStartup.SetActive(true);
        DProcedure.SetActive(false);
        DShutDown.SetActive(false);
        section = 7;
        state = 1;
        textContent = BStartup.transform;
    }

    public void PerformDProcedure()
    {
        BStartup.SetActive(false);
        BProcedure.SetActive(false);
        BShutDown.SetActive(false);
        CStartup.SetActive(false);
        CProcedure.SetActive(false);
        CShutDown.SetActive(false);
        DStartup.SetActive(false);
        DProcedure.SetActive(true);
        DShutDown.SetActive(false);
        section = 8;
        state = 1;
        textContent = BProcedure.transform;
    }

    public void PerformDShutDown()
    {   
        BStartup.SetActive(false);
        BProcedure.SetActive(false);
        BShutDown.SetActive(false);
        CStartup.SetActive(false);
        CProcedure.SetActive(false);
        CShutDown.SetActive(false);
        DStartup.SetActive(false);
        DProcedure.SetActive(false);
        DShutDown.SetActive(true);
        section = 9;
        state = 1;
        textContent = BShutDown.transform;
    }

    private void Update() {
        if (partOneComplete && partTwoComplete && partThreeComplete &&
            partFourComplete && partFiveComplete && partSixComplete &&
            partSevenComplete && partEightComplete && partNineComplete) {
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

        updateScore();

        // for testing only (completes all procedures)
        if (Input.GetKeyDown("v")) {
            partOneComplete = true;
            partTwoComplete = true;
            partThreeComplete = true;
        }

        // completes individual procedures
        if (Input.GetKeyDown("j")) {
            BStartup.SetActive(true);
            BProcedure.SetActive(false);
            BShutDown.SetActive(false);
            resetValves();
            section = 1;
            state = 1;
            textContent = BStartup.transform;
        } else if (Input.GetKeyDown("k")) {
            partOneComplete = true;
            BStartup.SetActive(false);
            BProcedure.SetActive(true);
            BShutDown.SetActive(false);
            resetValves();
            section = 2;
            state = 1;
            textContent = BProcedure.transform;
        } else if (Input.GetKeyDown("l")) {
            partOneComplete = true;
            partTwoComplete = true;
            BStartup.SetActive(false);
            BProcedure.SetActive(false);
            BShutDown.SetActive(true);
            resetValves();
            section = 3;
            state = 1;
            textContent = BShutDown.transform;
        }

        if (Input.GetKeyDown("c")) {
             if (partEightComplete) {
                PerformDShutDown();
                partThree();
            } else if (partSevenComplete)
            {
                PerformDProcedure();
                partTwo();
            } else if (partSixComplete)
            {
                PerformDStartUp();
                partOne();
            } else if (partFiveComplete)
            {
                PerformCShutDown();
                partThree();
            } else if (partFourComplete)
            {
                PerformCProcedure();
                partTwo();
            } else if (partThreeComplete)
            {
                PerformCStartUp();
                partOne();
            } else if (partTwoComplete)
            {
                PerformBShutDown();
                partThree();
            } else if (partOneComplete)
            {
                PerformBProcedure();
                partTwo();
            }

        }
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
        partFourComplete = false;
        partFiveComplete = false;
        partSixComplete = false;
        partSevenComplete = false;
        partEightComplete = false;
        partNineComplete = false;
        timeStart = 0;
        currentScore = 0;
        endScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        updateScore();
    }

    public void onChange() {
        oldState = state;

        if (state == -1) {
            startupCheck();
        }

        if (state > -1) {
            if (section == 1) {
                partOne();
            } else if (section == 2) {
                partTwo();
            } else if (section == 3) {
                partThree();
            } else if (section == 4)
            {
                partOne();
            }
            else if (section == 5)
            {
                partTwo();
            }
            else if (section == 6)
            {
                partThree();
            }
            else if (section == 7)
            {
                partOne();
            }
            else if (section == 8)
            {
                partTwo();
            }
            else if (section == 9)
            {
                partThree();
            }
        }

        if (oldState > state) {
            FindObjectOfType<SoundManager>().Play("Error");
        } else if (oldState < state) {
            FindObjectOfType<SoundManager>().Play("Correct");
        }
    }

    private bool startupCheck() {
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
        if (state == -1) {
            state = 0;
        }

        statusUI.text = "Start up checklist met";
        return true;
    }

    private void partOne() {
        if (!checkCircle("V102")) { //temp placeholder
            state = 1;
            statusUI.text = "B Start Up: step " + state;
            if (!checkCircle("V102")) {
                state = 2;
                statusUI.text = "B Start Up: step " + state;
                if (!checkCircle("V103")) {
                    state = 3;
                    statusUI.text = "B Start Up: step " + state;
                    if (!checkOpen("V104")) { //verify direction and valve type
                        state = 4;
                        statusUI.text = "B Start Up: step " + state;
                        if (checkCircle("V105")) {
                            state = 5;
                            statusUI.text = "B Start Up: step " + state;
                            if (!checkCircle("V106")) {
                                state = 6;
                                statusUI.text = "B Start Up: step " + state;
                                if (!checkCircle("V102")) { //temp placeholder
                                    state = 7;
                                    partOneComplete = true;
                                    statusUI.text = "B Start Up Complete! Press [C] to perform B procedure";
                                }
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
            startupCheck();
        }

        updateStatus();
    }

    private void partTwo() {
        if (checkCircle("V101")) { 
            state = 1;
            statusUI.text = "B Procedure: step " + state;
            if (checkCircle("V101"))
            { // placeholder for now, no switch found
                state = 2;
                statusUI.text = "B Procedure: step " + state;
                if (!checkCircle("V103") && checkCircle("V101")) { //pump part unimplemented, no switch
                    if (!checkCircle("V103") && checkCircle("V101")) { //liquid drained condition unimplemented, pump no switch
                        state = 3;
                        statusUI.text = "B Procedure: step " + state;
                        if (checkCircle("V101")) { //placeholder, no adjustment yet
                            state = 4;
                            statusUI.text = "B Procedure: step " + state;
                            if (checkCircle("V105")) { //placeholder, no adjustment on circle valve yet
                                state = 5;
                                statusUI.text = "B Procedure: step " + state;
                                if (checkCircle("V106")) { //check level to be implemented
                                    state = 6;
                                    statusUI.text = "B Procedure: step " + state;
                                    if (checkCircle("V106")) { //placeholder
                                        state = 7;
                                        partTwoComplete = true;
                                        statusUI.text = "B Procedure Complete! Press [C] to perform B Shutdown procedure";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
            startupCheck();
        }

        updateStatus();
    }

    private void partThree() {
        if (!checkCircle("V101")) { //placeholder for pump
            state = 1;
            statusUI.text = "B Shutdown: step " + state;
            if (!checkCircle("V101")) {
                state = 2;
                statusUI.text = "B Shutdown: step " + state;
                if (!checkCircle("V103")) { //placeholder for liquidline drainage condition
                    state = 3;
                    statusUI.text = "B Shutdown: step " + state;
                    if (!checkCircle("V106")) { //placeholder for drainage of leaching vessel condition
                        state = 4;
                        partThreeComplete = true;
                        statusUI.text = "B Shutdown Complete! Press [C] to go to next section";
                    }
                }
            }
        } else {
            state = 0;
            startupCheck();
        }

        updateStatus();
    }


    // reset valves
    public void resetValves() {
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
