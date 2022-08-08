using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public GameObject HeatExchange;
    public GameObject HXProcedure;
    public GameObject PackedGreen;
    public GameObject PDProcedure;
    public static string activeSystem = "LS";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            PackedGreen.GetComponent<PDSystemState>().enabled = false;
            HeatExchange.GetComponent<HXSystemState>().enabled = true;
            PDProcedure.SetActive(false);
            HXProcedure.SetActive(true);
            activeSystem = "HX";
        } else if (Input.GetKeyDown("2")) {
            HeatExchange.GetComponent<HXSystemState>().enabled = false;
            PackedGreen.GetComponent<PDSystemState>().enabled = true;
            PDProcedure.SetActive(true);
            HXProcedure.SetActive(false);
            activeSystem = "PD";
        }
    }

    public void onChange()
    {
        if (activeSystem == "HX")
        {
            GameObject.FindWithTag("HX").GetComponent<HXSystemState>().onChange();
        }
        else if (activeSystem == "PD")
        {
            GameObject.FindWithTag("PD").GetComponent<PDSystemState>().onChange();
        }
        else if (activeSystem == "LS")
        {
            GameObject.FindWithTag("LS").GetComponent<LeachingSystem>().onChange();
        }
    }
}
