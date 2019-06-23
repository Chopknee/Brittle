using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {

    Button myButton;
    public Text myText;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<Button>();
        
        myText = GetComponentInChildren<Text>();
	}
	
	void OnHoverEnter() {

    }

    void OnHoverExit() {

    }
}
