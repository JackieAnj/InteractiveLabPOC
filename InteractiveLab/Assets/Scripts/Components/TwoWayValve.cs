using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayValve : MonoBehaviour
{
    public string id;
    public bool open;
    public GameObject target;
    public bool rotateVertical;
    public bool rotateHorizontal;

    private void Start() {
        if (!open) {
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
        open = !open;

        FindObjectOfType<SoundManager>().Play("TurnValve");

        if (open) {
            if (rotateVertical) {
                transform.RotateAround(target.transform.position, Vector3.right, 90f);
            } else if (rotateHorizontal) {
                transform.RotateAround(target.transform.position, Vector3.back, 90f);
            } else {
                transform.RotateAround(target.transform.position, Vector3.down, 90f);
            }
        } else {
            if (rotateVertical) {
                transform.RotateAround(target.transform.position, Vector3.left, 90f);
            } else if (rotateHorizontal) {
                transform.RotateAround(target.transform.position, Vector3.forward, 90f);
            } else {
                transform.RotateAround(target.transform.position, Vector3.up, 90f);
            }
        }
    }

    public void Reset() {
        if (open) {
            if (rotateVertical) {
                transform.RotateAround(target.transform.position, Vector3.right, 90f);
            } else if (rotateHorizontal) {
                transform.RotateAround(target.transform.position, Vector3.back, 90f);
            } else {
                transform.RotateAround(target.transform.position, Vector3.down, 90f);
            }

            open = false;
        }
    }
}
