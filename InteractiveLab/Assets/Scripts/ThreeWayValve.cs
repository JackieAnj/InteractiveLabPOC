using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Position {top, left, bottom, right};
public class ThreeWayValve : MonoBehaviour
{
    public string id;
    public Position position;
    public GameObject target;
    public bool rotateVertical;
    public bool rotateHorizontal;

    public void TurnValve() {
        
        if (position == Position.top) {
            position = Position.right;
        } else if (position == Position.right) {
            position = Position.bottom;
        } else if (position == Position.bottom) {
            position = Position.left;
        } else {
            position = Position.top;
        }

        if (rotateVertical) {
            transform.RotateAround(target.transform.position, Vector3.left, 90f);
        } else if (rotateHorizontal) {
            transform.RotateAround(target.transform.position, Vector3.forward, 90f);
        } else {
            transform.RotateAround(target.transform.position, Vector3.up, 90f);
        }
    }
}
