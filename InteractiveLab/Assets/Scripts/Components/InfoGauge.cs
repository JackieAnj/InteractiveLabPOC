using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoGauge : MonoBehaviour
{
    public string id;
    public string description;
    public int value;

    public void updateValue(int newValue) {
        value = newValue;
    }
}
