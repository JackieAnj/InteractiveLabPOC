using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string id;
    public bool closed;
    public GameObject target;
    public bool rotateVertical;
    public bool rotateHorizontal;
    private bool turned = false;

    void Update()
    {
        if (closed && !turned) {
            turned = true;
            if (rotateVertical) {
                transform.RotateAround(target.transform.position, Vector3.left, 90f);
            } else if (rotateHorizontal) {
                transform.RotateAround(target.transform.position, Vector3.forward, 90f);
            } else {
                transform.RotateAround(target.transform.position, Vector3.up, 90f);
            }
            
        }

        if (!closed && turned) {
            turned = false;
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
