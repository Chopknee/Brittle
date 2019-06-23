using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Samuels : MonoBehaviour
{

    public List<IInteractable> interactablesInRange;
    public float interactableScanRange = 100;
    public string searchButton = "Search For Interactables";
    public string switchButton = "Switch Interactable";

    public int selectedInteractable = 0;
    private Vector3 lastPos = new Vector3();
    public bool hasScanned = false;
    public IInteractable interactedObject;

    // Start is called before the first frame update
    void Start()
    {
        interactablesInRange = new List<IInteractable>();
    }

    //Press a control to have samuels look for interactable objects
        //Objects are highlighted
        //Player uses mouse/joystick to select one
        //

    // Update is called once per frame
    void Update()
    {
        //Looking for objects to highlight
        //Get a list of interactable objects
        //
        if (( transform.position - lastPos ).sqrMagnitude > 0.1) {
            lastPos = transform.position;
            UnScan();
        }

        if (Input.GetButtonDown(searchButton)) {
            if (!hasScanned) {
                foreach (IInteractable i in interactablesInRange) {
                    i.DeSelect();
                    i.Dim();
                }
                if (interactedObject != null) {
                    interactedObject.DeSelect();
                    interactedObject.Dim();
                }
                //Clear old list
                interactablesInRange.Clear();
                //Grab all interacables from scene
                var ps = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>();
                //Put them into the list if they are in range
                foreach (IInteractable p in ps) {
                    //Range check (distance from kiesel.)
                    if (Vector3.Distance(p.GetGameObject().transform.position, transform.position) < interactableScanRange) {
                        interactablesInRange.Add(p);
                        p.Highlight();
                    }
                }
                selectedInteractable = 0;
                interactablesInRange[selectedInteractable].Select();
                hasScanned = true;
            } else {
                hasScanned = false;
                UnScan();
            }
        }

        if (Input.GetButtonDown(switchButton)) {
            if (interactablesInRange.Count() > 0) { 
                if (selectedInteractable < interactablesInRange.Count()-1) {
                    interactablesInRange[selectedInteractable].DeSelect();
                    selectedInteractable++;
                    interactablesInRange[selectedInteractable].Select();
                } else {
                    interactablesInRange[selectedInteractable].DeSelect();
                    selectedInteractable = 0;
                    interactablesInRange[selectedInteractable].Select();
                }
            }
        }

        if (Input.GetButtonDown("Interact") && hasScanned) {
            interactablesInRange[selectedInteractable].Interact();
            interactedObject = interactablesInRange[selectedInteractable];
            UnScan();
            interactedObject.Select();
            interactedObject.Highlight();
        }
    }

    public void UnScan() {
        if (interactablesInRange.Count() > 0) {
            foreach (IInteractable i in interactablesInRange) {
                i.DeSelect();
                i.Dim();
            }
            //Clear old list
            interactablesInRange.Clear();
            hasScanned = false;
        }
    }

    void OnDrawGizmosSelected () {
        Gizmos.DrawWireSphere(transform.position, interactableScanRange);
    }
}
