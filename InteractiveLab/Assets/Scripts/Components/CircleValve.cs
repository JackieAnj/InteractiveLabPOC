using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleValve : MonoBehaviour
{
    public string id;
    public bool open;
    public bool rotateVertical;
    public bool rotateHorizontal;

    public void TurnValve() {
        FindObjectOfType<SoundManager>().Play("TurnCircleValve");
        open = !open;
    }
}
