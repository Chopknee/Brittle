using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour {

    public int superSampleMultiplier = 1;
    public bool autoIncrementFileNames = false;
    public string fileName = "Capture";
    private int capNum = 0;
	// Use this for initialization
	void Start () {
        //Application.CaptureScreenshot("Afile.png", 8);
        ScreenCapture.CaptureScreenshot("Afile.png", superSampleMultiplier);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
            string filename = fileName;
            if (autoIncrementFileNames) {
                fileName += "_" + capNum;
                capNum++;
            }
            ScreenCapture.CaptureScreenshot(fileName + ".png", superSampleMultiplier);
        }
	}
}
