using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;
    
    [SerializeField]
    private XRNode xrNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private bool wasPressed = false;

    void getDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
        if (!device.isValid)
        {
            getDevice();
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
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Exit() {
        Application.Quit();
    }
}
