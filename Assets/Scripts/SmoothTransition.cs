using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransition {

    public delegate void Finish();
    public event Finish OnFinish;

    public float start = 0;
    public float end = 0;
    public float duration = 0;
    public AnimationCurve curve;

    private float currentTime = 0;
    public bool running;

    public float outNumber = 0;

    public SmoothTransition(float startNumber, float endNumber, AnimationCurve curve, float duration) {
        start = startNumber;
        end = endNumber;
        this.duration = duration;
        this.curve = curve;
    }

    public void Begin() {
        running = true;
        currentTime = 0;
        outNumber = start;
    }

    public void Begin(float startNumber, float endNumber, AnimationCurve curve, float duration) {
        start = startNumber;
        end = endNumber;
        this.duration = duration;
        this.curve = curve;
        Begin();
    }

    public float DriveForward() {
        if (running) {
            currentTime += Time.deltaTime;
            if (start > end) {
                outNumber = Mathf.Lerp(start, end, curve.Evaluate(currentTime / duration));
            } else {
                outNumber = Mathf.Lerp(end, start, curve.Evaluate((duration - currentTime) / duration));
            }
            if (currentTime >= duration) {
                OnFinish();
                running = false;
            }
        }
        return outNumber;
    }
}
