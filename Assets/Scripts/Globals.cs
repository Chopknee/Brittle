using UnityEngine;

public class Globals : MonoBehaviour {

    public static Globals Instance {
        get {
            return inst;
        }
    }

    private static Globals inst;

    public bool joystickUsed = false;

    public float volume {
        get {
            return AudioListener.volume;
        }
        set {
            AudioListener.volume = value;
        }
    }

	// Use this for initialization
	void Awake () {
		if (inst == null) {
            inst = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            
            enabled = false;
        }
	}
}