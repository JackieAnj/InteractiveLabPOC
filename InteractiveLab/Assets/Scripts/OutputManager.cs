using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputManager : MonoBehaviour
{

    private RecordingTable _outputTable;

    private void OnEnable()
    {
        OutputManagerEvents.OnRecord += RecordOutput;
    }

    private void OnDisable()
    {
        OutputManagerEvents.OnRecord -= RecordOutput;
    }

    // Start is called before the first frame update
    void Start()
    {
        // EQUIPMENT RECORDING SETUP ========================================
        _outputTable = new RecordingTable();
        _outputTable.AddColumn("ComponentID", Type.GetType("System.String"));
        _outputTable.AddColumn("ActionOutcome", Type.GetType("System.Decimal"));
    }
    
    void RecordOutput(string componentID, float actionOutcome)
    {
        _outputTable.AddRow(new TableCell<object>[]
        {
            new("ComponentID", componentID), 
            new("ActionOutcome", actionOutcome)
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
