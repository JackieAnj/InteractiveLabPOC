using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemState : MonoBehaviour
{
    private string[] TwoWayValveNames = {"V110", "V111", "V112", "V113", "V115", "V116", "V118",  "V121",  "V125", "V126", "V128", "V130", "V132", "V133", "V134"};
    private string[] ThreeWayValveNames = {"V119", "V122", "V123", "V124", "V131"};
    //private string[] circleValveNames = {};
    private string[] PRVNames = {"PRV10","PRV11","PRV12","PRV13"};

    // make this private (public for testing)
    public int state = -1;

    public GameObject CondensationTrapOne;

    public GameObject CondensationTrapTwo;

    public Text startupCheckStatus;

    [System.Serializable]
    public class TwoWayValve {
        public string id;
        public bool open;
        public TwoWayValve(string id, bool open = false) {
            this.id = id;
            this.open = open;
        }
    }

    [System.Serializable]
    public class ThreeWayValve {
        public string id;
        public Position position;
        public ThreeWayValve(string id, Position position) {
            this.id = id;
            this.position = position;
        }
    }

    [System.Serializable]
    public class PRV {
        public string id;
        public int turn;
        public PRV(string id, int turn) {
            this.id = id;
            this.turn = turn;
        }
    }

    public TwoWayValve[] twoWayValves;
    public ThreeWayValve[] threeWayValves;
    public PRV[] PRVs;

    private void Start() {
        int index = 0;
        foreach(string v in TwoWayValveNames) {
            twoWayValves[index] = new TwoWayValve(v, false);
            index++;
        }

        index = 0;
        foreach(string v in ThreeWayValveNames) {
            threeWayValves[index] = new ThreeWayValve(v, Position.top);
            index++;
        }

        index = 0;
        foreach (string v in PRVNames) {
            PRVs[index] = new PRV(v, 0);
            index++;
        }

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
                startupCheckStatus.text = "Start up checklist unmet";
                return false;
            }
        }
        
        // check liquid level in condensation traps
        if (CondensationTrapOne.GetComponent<CondensationTrap>().liquidLevel + CondensationTrapTwo.GetComponent<CondensationTrap>().liquidLevel > 0) {
            state = -1;
            startupCheckStatus.text = "Start up checklist unmet";
            return false;
        }

        if (state == -1) {
            state = 0;
        }

        startupCheckStatus.text = "Start up checklist met";
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
            if (checkOpen("V121")) {
                state = 2;
                if (checkPosition("V131", Position.left)) {
                    state = 3;
                        if (checkPosition("V119", Position.left) && checkPosition("V123", Position.left)) {
                            state = 4;
                            if (checkPosition("V124", Position.left) && checkOpen("V125")) {
                                state = 5;
                                if (checkOpen("V132")) {
                                    state = 6;
                                    if (checkTurn("PRV10", 1)) {
                                        state = 7;
                                        if (checkOpen("V111")) {
                                            state = 8;
                                            if (checkTurn("PRV10", 3)) {
                                                state = 9;
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

    // check if a PRV has at least x number of turns
    private bool checkTurn(string id, int turn) {
        return Array.Find(PRVs, v => v.id == id).turn >= turn;
    }
}
