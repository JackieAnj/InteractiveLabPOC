using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string twoWayValveTag = "TwoWayValve";
    [SerializeField] private string threeWayValveTag = "ThreeWayValve";
    [SerializeField] private string circleValveTag = "CircleValve";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    public float distanceToSee;

    public Text interactCaption;
    public Image captionBackground;

    private Transform _selection;
    private GameObject system;

    private void Start() {
        interactCaption.text = "";
        system = GameObject.FindWithTag("System");
    }
    private void Update() {
        if (_selection != null) {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
            interactCaption.text = "";
            captionBackground.enabled = false;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceToSee)) {
            var selection = hit.transform;

            if (selection.CompareTag(twoWayValveTag)) {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null) {
                    selectionRenderer.material = highlightMaterial;
                }
                _selection = selection;

                var action = "";
                if (hit.collider.gameObject.GetComponent<TwoWayValve>().closed) {
                    action = "Open ";
                } else {
                    action = "Close ";
                }
                interactCaption.text = action + hit.collider.gameObject.GetComponent<TwoWayValve>().id + " [E]";
                captionBackground.enabled = true;

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<TwoWayValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    var valves = system.GetComponent<SystemState>().twoWayValves;
                    foreach(var v in valves)
                    {
                        if (v.id == target.id) {
                            v.open = !target.closed;
                        }
                    }
                }
            }

            if (selection.CompareTag(threeWayValveTag)) {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null) {
                    selectionRenderer.material = highlightMaterial;
                }
                _selection = selection;

                interactCaption.text = "Turn " + hit.collider.gameObject.GetComponent<ThreeWayValve>().id + " [E]";
                captionBackground.enabled = true;

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<ThreeWayValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    var valves = system.GetComponent<SystemState>().threeWayValves;
                    foreach(var v in valves)
                    {
                        if (v.id == target.id) {
                            v.position = target.position;
                        }
                    }
                }
            }

            if (selection.CompareTag(circleValveTag)) {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null) {
                    selectionRenderer.material = highlightMaterial;
                }
                _selection = selection;

                interactCaption.text = "Turn " + hit.collider.gameObject.GetComponent<CircleValve>().id + " [E]";
                captionBackground.enabled = true;

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<CircleValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    var valves = system.GetComponent<SystemState>().twoWayValves;
                    foreach(var v in valves)
                    {
                        if (v.id == target.id) {
                            v.open = !target.closed;
                        }
                    }
                }
            }
        }
    }
}
