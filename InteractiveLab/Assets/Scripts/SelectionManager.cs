using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string interactTag = "interactable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    public float distanceToSee;

    public Text interactCaption;

    private Transform _selection;

    private void Start() {
        interactCaption.text = "";
    }
    private void Update() {
        if (_selection != null) {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
            interactCaption.text = "";
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceToSee)) {
            var selection = hit.transform;
            if (selection.CompareTag(interactTag)) {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null) {
                    selectionRenderer.material = highlightMaterial;
                }
                _selection = selection;

                var action = "";
                if (hit.collider.gameObject.GetComponent<Interactable>().closed) {
                    action = "Open ";
                } else {
                    action = "Close ";
                }
                interactCaption.text = action + hit.collider.gameObject.GetComponent<Interactable>().id + " [E]";

                if (Input.GetKeyDown(KeyCode.E)) {
                    var target = hit.collider.gameObject.GetComponent<Interactable>();
                    Debug.Log("Hit " + target.id);
                    target.closed = !target.closed;
                }
            }
        }
    }
}
