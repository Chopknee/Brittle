using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    public Slider sldMasterVolume;
    public Toggle togController;

	// Use this for initialization
	void Start () {
		if (sldMasterVolume == null || togController == null) {
            Debug.Log("Options menu script is missing components. Name is " + gameObject.name + ".");
            return;
        }

        sldMasterVolume.maxValue = 1.0f;
        sldMasterVolume.minValue = 0.0f;
        sldMasterVolume.value = Globals.Instance.volume;
        sldMasterVolume.onValueChanged.AddListener(OnVolume);
        togController.isOn = Globals.Instance.joystickUsed;
        togController.onValueChanged.AddListener(SetController);
	}
	
    void OnVolume(float value) {
        Globals.Instance.volume = value;
    }

    void SetController(bool val) {
        Globals.Instance.joystickUsed = val;
    }
}
