using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemState : MonoBehaviour
{
    //public bool V110, V111, V112, V113, V115, V116, V118, V119, V121, V122, V123, V124, V125, V126, V128, V130, V131, V132, V133, V134; 
    //public bool PRV10, PRV11, PRV12, PRV13;

    private string[] TwoWayValveNames = {"V110", "V111", "V112", "V113", "V115", "V116", "V118",  "V121",  "V125", "V126", "V128", "V130",  "V132", "V133", "V134"};
    private string[] ThreeWayValveNames = {"V119", "V122", "V123", "V124", "V131",};
    private string[] circleValveNames = {};
    private string[] PRVNames = {};

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

    public TwoWayValve[] twoWayValves;
    public ThreeWayValve[] threeWayValves;

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

        startupCheckStatus.text = "Start up checklist unmet";
    }

    private void Update() {
        if (startupCheck()) {
            startupCheckStatus.text = "Start up checklist met";
        } else {
            startupCheckStatus.text = "Start up checklist unmet";
        }
    }

    private bool startupCheck() {
        // check all specified valves are closed
        string[] closedValves = {"V111", "V121", "V128", "V114", "V130", "V132", "V134"};
        foreach(TwoWayValve v in twoWayValves) {
            if (Array.Exists(closedValves, x => x == v.id) && v.open) {
                return false;
            }
        }

        return true;
    }
}
