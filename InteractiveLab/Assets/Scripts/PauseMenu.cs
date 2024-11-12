using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;
using UnityEngine.Rendering;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;
    
    [SerializeField]
    private XRNode xrNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private bool wasPressed = false;
    
    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
        // it turns out for VR, Canvas appears twice - one is VR Canvas (Camera) and the other is VR Canvas (World
        // Space). This caused the button press to be registered twice in a row.
        
        if (!device.isValid)
        {
            GetDevice();
        }

        bool isPressed = device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed) && pressed;

        if (!isPressed) {
            wasPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab) || (isPressed && !wasPressed)) {
            wasPressed = true;

            if (paused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Debug.Log($"Pause menu is inactive, Paused is {paused}");
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        // Debug.Log($"Pause menu is active, Paused is {paused}");
    }

    public void Exit() {
        Application.Quit();
    }
}
