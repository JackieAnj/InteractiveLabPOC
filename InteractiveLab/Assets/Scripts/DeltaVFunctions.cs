using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaVFunctions : MonoBehaviour
{
    public string steamInput;

    public void Exit() {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void getSteamFlow(string s) {
        steamInput = s;
        Debug.Log(steamInput);
    }
}
