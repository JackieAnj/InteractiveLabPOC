using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string twoWayValveTag = "TwoWayValve";
    [SerializeField] private string threeWayValveTag = "ThreeWayValve";
    [SerializeField] private string circleValveTag = "CircleValve";
    [SerializeField] private string condensationTrapTag = "CondensationTrap";
    [SerializeField] private string PRVTag = "PRV";
    [SerializeField] private string ComputerTag = "Computer";
    [SerializeField] private string InfoGaugeTag = "InfoGauge";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    public float distanceToSee;
    public Text interactCaption;
    public Image captionBackground;
    public GameObject videoPanel;
    public GameObject DeltaV;
    public GameObject stateManager;
    private Transform _selection;

    private void Start() {
        interactCaption.text = "";
    }

    private void Update() {
        if (Input.GetKeyDown("t")) {
            videoPanel.SetActive(!videoPanel.activeSelf);
        }

        if (_selection != null) {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
            interactCaption.text = "";
            captionBackground.enabled = false;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceToSee) && !PauseMenu.paused) {
            var selection = hit.transform;

            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null) {
                selectionRenderer.material = highlightMaterial;
            }
            _selection = selection;

            if (selection.CompareTag(InfoGaugeTag)) {
                interactCaption.text = hit.collider.gameObject.GetComponent<InfoGauge>().description + hit.collider.gameObject.GetComponent<InfoGauge>().value;
                captionBackground.enabled = true;
            }

            if (selection.CompareTag(twoWayValveTag)) {
                var action = "";
                if (hit.collider.gameObject.GetComponent<TwoWayValve>().open) {
                    action = "Close ";
                } else {
                    action = "Open ";
                }
                interactCaption.text = action + hit.collider.gameObject.GetComponent<TwoWayValve>().id + " [Left Click]";
                captionBackground.enabled = true;

                if (Input.GetMouseButtonDown(0)) {
                    var target = hit.collider.gameObject.GetComponent<TwoWayValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    stateManager.GetComponent<StateManager>().OnChange();
                }
            }

            if (selection.CompareTag(threeWayValveTag)) {
                interactCaption.text = "Turn " + hit.collider.gameObject.GetComponent<ThreeWayValve>().id + "(" + hit.collider.gameObject.GetComponent<ThreeWayValve>().position + ") [Left Click]";
                captionBackground.enabled = true;

                if (Input.GetMouseButtonDown(0)) {
                    var target = hit.collider.gameObject.GetComponent<ThreeWayValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    stateManager.GetComponent<StateManager>().OnChange();
                }
            }

            if (selection.CompareTag(circleValveTag)) {
                var action = "";
                if (hit.collider.gameObject.GetComponent<CircleValve>().open) {
                    action = "Close ";
                } else {
                    action = "Open ";
                }
                interactCaption.text = action + hit.collider.gameObject.GetComponent<CircleValve>().id + " [Left Click]";
                captionBackground.enabled = true;

                if (Input.GetMouseButtonDown(0)) {
                    var target = hit.collider.gameObject.GetComponent<CircleValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    stateManager.GetComponent<StateManager>().OnChange();
                }
            }

            if (selection.CompareTag(condensationTrapTag)) {
                interactCaption.text = "Condensation Trap Liquid Level: " + hit.collider.gameObject.GetComponent<CondensationTrap>().liquidLevel + "%";
                captionBackground.enabled = true;
            }

            if (selection.CompareTag(PRVTag)) {
                interactCaption.text = "Turn " + hit.collider.gameObject.GetComponent<PRVValve>().id + "(current: " + hit.collider.gameObject.GetComponent<PRVValve>().turn + ")\n Left [Right Click], Right [Left Click]";
                captionBackground.enabled = true;

                if (Input.GetMouseButtonDown(0)) {
                    var target = hit.collider.gameObject.GetComponent<PRVValve>();
                    target.TurnValve("right");
                    stateManager.GetComponent<StateManager>().OnChange();
                }

                if (Input.GetMouseButtonDown(1)) {
                    var target = hit.collider.gameObject.GetComponent<PRVValve>();
                    target.TurnValve("left");
                    stateManager.GetComponent<StateManager>().OnChange();
                }
            }

            if (selection.CompareTag(ComputerTag)) {
                interactCaption.text = "Access DeltaV [Left Click]";
                captionBackground.enabled = true;

                if (Input.GetMouseButtonDown(0)) {
                    DeltaV.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }
        }
    }
}
