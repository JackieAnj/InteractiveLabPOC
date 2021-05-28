using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemState : MonoBehaviour
{
    // make this private (public for testing)
    public int state = -1;

    public GameObject CondensationTrapOne;
    public GameObject CondensationTrapTwo;
    public Text statusUI;

    public TwoWayValve[] twoWayValves;
    public ThreeWayValve[] threeWayValves;
    public CircleValve[] circleValves;
    public PRVValve[] PRVs;

    private void Start() {
        twoWayValves = UnityEngine.Object.FindObjectsOfType<TwoWayValve>();
        threeWayValves = UnityEngine.Object.FindObjectsOfType<ThreeWayValve>();
        circleValves = UnityEngine.Object.FindObjectsOfType<CircleValve>();
        PRVs = UnityEngine.Object.FindObjectsOfType<PRVValve>();

        startupCheck();
    }

    public void onChange() {
        clearCondensationCheck();

        if (state == -1) {
            startupCheck();
        }

        partOne();
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
            statusUI.text = "step: " + state;
            if (checkCircle("V121")) {
                state = 2;
                statusUI.text = "step: " + state;
                if (checkPosition("V131", Position.left)) {
                    state = 3;
                    statusUI.text = "step: " + state;
                        if (checkPosition("V119", Position.left) && checkPosition("V123", Position.left)) {
                            state = 4;
                            statusUI.text = "step: " + state;
                            if (checkPosition("V124", Position.left) && checkOpen("V125")) {
                                state = 5;
                                statusUI.text = "step: " + state;
                                if (checkCircle("V132")) {
                                    state = 6;
                                    statusUI.text = "step: " + state;
                                    if (checkTurn("PRV10", 1)) {
                                        state = 7;
                                        statusUI.text = "step: " + state;
                                        if (checkCircle("V111")) {
                                            state = 8;
                                            statusUI.text = "step: " + state;
                                            if (checkTurn("PRV10", 3)) {
                                                state = 9;
                                                statusUI.text = "step: " + state;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }
            }
        } else {
            startupCheck();
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
}
