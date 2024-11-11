using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputManagerEvents : MonoBehaviour
{
    public delegate void Record(string componentID, float actionOutcome);
    public static event Record OnRecord;

    public static void RecordToOutput(string componentID, float actionOutcome)
    {
        OnRecord?.Invoke(componentID, actionOutcome);
    }
}
