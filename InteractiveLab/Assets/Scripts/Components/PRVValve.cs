using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRVValve : MonoBehaviour
{
    public string id;
    public int turn;
    public bool rotateVertical;
    public bool rotateHorizontal;

    public void TurnValve(string direction) {
        if (direction == "left" && turn > 0) {
            turn--;
        }

        if (direction == "right" && turn < 4) {
            turn++;
        }
    }
}
