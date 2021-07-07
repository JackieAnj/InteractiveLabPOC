using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemState : MonoBehaviour
{
    // make this private (public for testing)
    private int state = -1;

    public GameObject CondensationTrapOne;
    public GameObject CondensationTrapTwo;
    public Text statusUI;
    public GameObject partOneText;
    public GameObject partTwoText;
    public GameObject partThreeText;
    private Transform textContent;
    private TextMeshProUGUI content;
    private int section = 1;
    private TwoWayValve[] twoWayValves;
    private ThreeWayValve[] threeWayValves;
    private CircleValve[] circleValves;
    private PRVValve[] PRVs;

    private void Start() {
        twoWayValves = UnityEngine.Object.FindObjectsOfType<TwoWayValve>();
        threeWayValves = UnityEngine.Object.FindObjectsOfType<ThreeWayValve>();
        circleValves = UnityEngine.Object.FindObjectsOfType<CircleValve>();
        PRVs = UnityEngine.Object.FindObjectsOfType<PRVValve>();
        textContent = partOneText.transform;

        startupCheck();
    }

    private void Update() {
        if (Input.GetKeyDown("c")) {
            if (section == 1) {
                section = 2;
                textContent = partOneText.transform;
                partOneText.SetActive(false);
                partTwoText.SetActive(true);
                partThreeText.SetActive(false);
                partTwo();
            } else if (section == 2) {
                section = 3;
                textContent = partTwoText.transform;
                partOneText.SetActive(false);
                partTwoText.SetActive(false);
                partThreeText.SetActive(true);
                partThree();
            } else {
                section = 1;
                textContent = partTwoText.transform;
                partOneText.SetActive(true);
                partTwoText.SetActive(false);
                partThreeText.SetActive(false);
                partOne();
            }
        }
    }

    public void onChange() {
        clearCondensationCheck();

        if (state == -1) {
            startupCheck();
        }

        if (state > -1) {
            Debug.Log(section);
            if (section == 1) {
                partOne();
            } else if (section == 2) {
                partTwo();
            } else {
                partThree();
            }
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

    private void clearCondensationCheck() {

        if (checkOpen("V125")) {
            CondensationTrapOne.GetComponent<CondensationTrap>().ClearLiquidLevel();
        }

        if (checkOpen("V126")) {
            CondensationTrapTwo.GetComponent<CondensationTrap>().ClearLiquidLevel();
        }
    }

    private void partOne() {
        if (checkPosition("V122", Position.left)) {
            state = 1;
            statusUI.text = "Part 1: step " + state;
            if (checkCircle("V121")) {
                state = 2;
                statusUI.text = "Part 1: step " + state;
                if (checkPosition("V131", Position.left)) {
                    state = 3;
                    statusUI.text = "Part 1: step " + state;
                    if (checkPosition("V119", Position.left) && checkPosition("V123", Position.left)) {
                        state = 4;
                        statusUI.text = "Part 1: step " + state;
                        if (checkPosition("V124", Position.left) && checkOpen("V125")) {
                            state = 5;
                            statusUI.text = "Part 1: step " + state;
                            if (checkCircle("V132")) {
                                state = 6;
                                statusUI.text = "Part 1: step " + state;
                                if (checkTurn("PRV10", 1)) {
                                    state = 7;
                                    statusUI.text = "Part 1: step " + state;
                                    if (checkCircle("V111")) {
                                        state = 8;
                                        statusUI.text = "Part 1: step " + state;
                                        if (checkTurn("PRV10", 3)) {
                                            state = 9;
                                            statusUI.text = "Part 1 Complete!" + state;
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
            startupCheck();
        }

        updateStatus();
    }

    private void partTwo() {
        if (checkPosition("V122", Position.left)) {
            state = 1;
            statusUI.text = "Part 2: step " + state;
            if (checkCircle("V121")) {
                state = 2;
                statusUI.text = "Part 2: step " + state;
                if (checkPosition("V119", Position.left)) {
                    state = 3;
                    statusUI.text = "Part 2: step " + state;
                    if (checkPosition("V123", Position.left)) {
                        state = 4;
                        statusUI.text = "Part 2: step " + state;
                        if (checkCircle("V132")) {
                            state = 5;
                            statusUI.text = "Part 2: step " + state;
                            if (checkTurn("PRV10", 1)) {
                                state = 6;
                                statusUI.text = "Part 2: step " + state;
                                if (checkCircle("V111")) {
                                    state = 7;
                                    statusUI.text = "Part 2: step " + state;
                                    if (checkTurn("PRV10", 3)) {
                                        state = 8;
                                        statusUI.text = "Part 2 Complete!" + state;
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
        if (checkPosition("V112", Position.left) && (checkPosition("V113", Position.left))) {
            state = 1;
            statusUI.text = "Part 3: step " + state;
            if (!checkOpen("V115") && !checkOpen("V116")) {
                state = 2;
                statusUI.text = "Part 3: step " + state;
                if (checkCircle("V128") && checkTurn("PRV12", 1)) {
                    state = 3;
                    statusUI.text = "Part 3: step " + state;
                    if (checkPosition("V131", Position.left)) {
                        state = 4;
                        statusUI.text = "Part 3: step " + state;
                        if (checkOpen("V126")) {
                            state = 5;
                            statusUI.text = "Part 3: step " + state;
                            if (checkPosition("V118", Position.left)) {
                                state = 6;
                                statusUI.text = "Part 3: step " + state;
                                if (checkOpen("V130")) {
                                    state = 7;
                                    statusUI.text = "Part 3: step " + state;
                                    if (checkTurn("PRV10", 3)) {
                                        state = 8;
                                        statusUI.text = "Part 3: step " + state;
                                        if (checkTurn("PRV10", 1)) {
                                            state = 9;
                                            statusUI.text = "Part 3: step " + state;
                                            if (checkCircle("V111")) {
                                                state = 10;
                                                statusUI.text = "Part 3: step " + state;
                                                if (checkTurn("PRV10", 3)) {
                                                    state = 11;
                                                    statusUI.text = "Part 3 Complete!";
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
            startupCheck();
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

    private void updateStatus() {
        int index = 0;

        foreach(Transform instruction in textContent) {
            index++;
            content = instruction.GetComponent<TextMeshProUGUI>();

            if (index <= state) {
                content.fontStyle = FontStyles.Strikethrough;
            } else {
                content.fontStyle = FontStyles.Normal;
            }
        }
    }
}
