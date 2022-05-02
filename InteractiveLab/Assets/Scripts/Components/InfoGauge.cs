using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoGauge : MonoBehaviour
{
    public string id;
    public string description;
    public int value;
    [SerializeField] TextMeshPro textValue;

    public void updateValue(int newValue) {
        value = newValue;
        textValue.text = newValue.ToString() + "C°";
    }
}
