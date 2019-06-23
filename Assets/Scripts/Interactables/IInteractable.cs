using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    void Interact();
    GameObject GetGameObject ();
    void Highlight ();
    void Select ();
    void Dim ();
    void DeSelect ();
}
