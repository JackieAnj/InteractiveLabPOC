using System.Collections;
using System.Collections.Generic;
using Recording;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum SystemType
    {
        HeatExchange,  // Heat Exchange
        PackedGreen,  // Packed Green
        LeachingSystem,  // Leaching System
    }
    
    public GameObject HeatExchange;
    public GameObject HXProcedure;
    public GameObject HXProcedureVR;
    
    public GameObject PackedGreen;
    public GameObject PDProcedure;
    public GameObject PDProcedureVR;
    
    public GameObject LeachingSystem;
    public GameObject LSProcedure;
    public SystemType activeSystem;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Current active system: {activeSystem}");
        ActivateSystem(activeSystem);
        
        OutputManagerEvents.SetSystemType(activeSystem.ToString());
    }

    void ActivateSystem(SystemType system)
    {
        switch (system)
        {
            case SystemType.HeatExchange:
                HeatExchange.GetComponent<HXSystemState>().enabled = true;
                HXProcedure.SetActive(true);
                PackedGreen.GetComponent<PDSystemState>().enabled = false;
                PDProcedure.SetActive(false);
                break;
            case SystemType.PackedGreen:
                PackedGreen.GetComponent<PDSystemState>().enabled = true;
                PDProcedure.SetActive(true);
                HeatExchange.GetComponent<HXSystemState>().enabled = false;
                HXProcedure.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // // Do not allow live update of system. Instead, active system should be determined before simulation starts.
        // if (Input.GetKeyDown("1")) {
        //     PackedGreen.GetComponent<PDSystemState>().enabled = false;
        //     HeatExchange.GetComponent<HXSystemState>().enabled = true;
        //     PDProcedure.SetActive(false);
        //     HXProcedure.SetActive(true);
        //     activeSystem = "HeatExchange";
        // } else if (Input.GetKeyDown("2")) {
        //     HeatExchange.GetComponent<HXSystemState>().enabled = false;
        //     PackedGreen.GetComponent<PDSystemState>().enabled = true;
        //     PDProcedure.SetActive(true);
        //     HXProcedure.SetActive(false);
        //     activeSystem = "PackedGreen";
        // }
    }

    public void OnChange()
    {
        switch (activeSystem)
        {
            case SystemType.HeatExchange:
                HeatExchange.GetComponent<HXSystemState>().OnChange();
                break;
            case SystemType.PackedGreen:
                PackedGreen.GetComponent<PDSystemState>().onChange();
                break;
            case SystemType.LeachingSystem:
                LeachingSystem.GetComponent<LeachingSystem>().onChange();
                break;
        }
    }
}
