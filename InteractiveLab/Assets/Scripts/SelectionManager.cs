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

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceToSee)) {
            var selection = hit.transform;

            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null) {
                selectionRenderer.material = highlightMaterial;
            }
            _selection = selection;


            if (selection.CompareTag(twoWayValveTag)) {
                var action = "";
                if (hit.collider.gameObject.GetComponent<TwoWayValve>().open) {
                    action = "Close ";
                } else {
                    action = "Open ";
                }
                interactCaption.text = action + hit.collider.gameObject.GetComponent<TwoWayValve>().id + " [E]";
                captionBackground.enabled = true;

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<TwoWayValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    system.GetComponent<SystemState>().onChange();
                }
            }

            if (selection.CompareTag(threeWayValveTag)) {
                interactCaption.text = "Turn " + hit.collider.gameObject.GetComponent<ThreeWayValve>().id + "(" + hit.collider.gameObject.GetComponent<ThreeWayValve>().position + ") [E]";
                captionBackground.enabled = true;

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<ThreeWayValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    system.GetComponent<SystemState>().onChange();
                }
            }

            if (selection.CompareTag(circleValveTag)) {
                var action = "";
                if (hit.collider.gameObject.GetComponent<CircleValve>().open) {
                    action = "Close ";
                } else {
                    action = "Open ";
                }
                interactCaption.text = action + hit.collider.gameObject.GetComponent<CircleValve>().id + " [E]";
                captionBackground.enabled = true;

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<CircleValve>();
                    Debug.Log("Hit " + target.id);
                    target.TurnValve();
                    system.GetComponent<SystemState>().onChange();
                }
            }

            if (selection.CompareTag(condensationTrapTag)) {
                interactCaption.text = "Condensation Trap Liquid Level: " + hit.collider.gameObject.GetComponent<CondensationTrap>().liquidLevel + "%";
                captionBackground.enabled = true;
            }

            if (selection.CompareTag(PRVTag)) {
                interactCaption.text = "Turn " + hit.collider.gameObject.GetComponent<PRVValve>().id + "(current: " + hit.collider.gameObject.GetComponent<PRVValve>().turn + ") Left [Q], Right [E]";
                captionBackground.enabled = true;

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<PRVValve>();
                    target.TurnValve("right");
                    system.GetComponent<SystemState>().onChange();
                }

                if (Input.GetKeyDown(KeyCode.Q)) {
                    var target = hit.collider.gameObject.GetComponent<PRVValve>();
                    target.TurnValve("left");
                    system.GetComponent<SystemState>().onChange();
                }

            }
        }
    }
}
