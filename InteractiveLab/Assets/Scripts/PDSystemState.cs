﻿using System;
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

    private void partOne() {
        if (checkPosition("HV700", Position.left)) {
            state = 1;
            statusUI.text = "Part 1: step " + state;
            updateGaugeValue("Temp", 10);
            if (checkPosition("HV701", Position.left)) {
                state = 2;
                statusUI.text = "Part 1: step " + state;
                if (checkTurn("FIC703", 1)) {
                    state = 3;
                    statusUI.text = "Part 1: step " + state;
                    if (checkTurn("HV806", 1)) {
                        state = 4;
                        statusUI.text = "Part 1: step " + state;
                        if (checkTurn("PRV807", 1)) {
                            state = 5;
                            statusUI.text = "Part 1: step " + state;
                            updateGaugeValue("PI802", 10);
                            if (checkCircle("HV704") && checkCircle("HV705")) {
                                state = 6;
                                statusUI.text = "Part 1: step " + state;
                                if (checkOpen("HV901")) {
                                    state = 7;
                                    partOneComplete = true;
                                    statusUI.text = "Part 1 Complete! Press [C] to perform part 2";
                                }
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
        }

        updateStatus();
    }

    private void partTwo() {
        if (checkOpen("HV203")) {
            state = 1;
            statusUI.text = "Part 2: step " + state;
            if (checkTurn("FIC204", 1)) {
                state = 2;
                statusUI.text = "Part 2: step " + state;
                if (!checkOpen("HV403") && checkOpen("HV402") && checkOpen("HV404")) {
                    state = 3;
                    statusUI.text = "Part 2: step " + state;
                    if (checkTurn("FIC401", 1)) {
                        state = 4;
                        statusUI.text = "Part 2: step " + state;
                        if (checkOpen("HV802")) {
                            state = 5;
                            statusUI.text = "Part 2: step " + state;
                            if (checkTurn("PRV803", 1)) {
                                state = 6;
                                statusUI.text = "Part 2: step " + state;
                                updateGaugeValue("PI801", 3);
                                partTwoComplete = true;
                                statusUI.text = "Part 2 Complete! Press [C] to perform shutdown procedure";
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
        }

        updateStatus();
    }

    private void partThree() {
        statusUI.text = "Shutdown Procedure";
        if (!checkOpen("FIC401")) {
            state = 1;
            statusUI.text = "Shutdown Procedure: step " + state;
            if (checkTurn("FIC204", 0)) {
                state = 2;
                statusUI.text = "Shutdown Procedure: step " + state;
                if (!checkOpen("HV203")) {
                    state = 3;
                    statusUI.text = "Shutdown Procedure: step " + state;
                    if (!checkOpen("HS201")) {
                        state = 4;
                        statusUI.text = "Shutdown Procedure: step " + state;
                        if (checkOpen("HV303")) {
                            state = 5;
                            statusUI.text = "Shutdown Procedure: step " + state;
                            if (!checkOpen("HV402") && checkOpen("HV403") && checkTurn("HS301", 1)) {
                                state = 6;
                                statusUI.text = "Shutdown Procedure: Step " + state;
                                if (!checkOpen("FIC703") && !checkOpen("HV701")) {
                                    state = 7;
                                    statusUI.text = "Shutdown Procedure: step " + state;
                                    partThreeComplete = true;
                                    statusUI.text = "Shutdown Procedure Complete!";
                                }
                            }
                        }
                    }
                }
            }
        } else {
            state = 0;
        }

        updateStatus();
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
