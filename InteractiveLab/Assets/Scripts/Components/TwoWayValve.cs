using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayValve : MonoBehaviour
{
    public string id;
    public bool closed;
    public GameObject target;
    public bool rotateVertical;
    public bool rotateHorizontal;

    private void Start() {
        if (closed) {
            if (rotateVertical) {
                transform.RotateAround(target.transform.position, Vector3.left, 90f);
            } else if (rotateHorizontal) {
                transform.RotateAround(target.transform.position, Vector3.forward, 90f);
            } else {
                transform.RotateAround(target.transform.position, Vector3.up, 90f);
            }
        }
    }

    public void TurnValve() {
        closed = !closed;

        if (closed) {
            if (rotateVertical) {
                transform.RotateAround(target.transform.position, Vector3.left, 90f);
            } else if (rotateHorizontal) {
                transform.RotateAround(target.transform.position, Vector3.forward, 90f);
            } else {
                transform.RotateAround(target.transform.position, Vector3.up, 90f);
            }
        } else {
            if (rotateVertical) {
                transform.RotateAround(target.transform.position, Vector3.right, 90f);
            } else if (rotateHorizontal) {
                transform.RotateAround(target.transform.position, Vector3.back, 90f);
            } else {
                transform.RotateAround(target.transform.position, Vector3.down, 90f);
            }
        }
    }
}
