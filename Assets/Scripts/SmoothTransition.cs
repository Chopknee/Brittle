﻿using System.Collections;
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
    public bool running = false;

    public float outNumber = 0;

    public SmoothTransition() {
        curve = new AnimationCurve();
        //Set up a linear curve by default
        float tan45 = Mathf.Tan(Mathf.Deg2Rad * 45);
        curve.AddKey(new Keyframe(0, 0, tan45, tan45));
        curve.AddKey(new Keyframe(1, 1, tan45, tan45));
    }

    public SmoothTransition(float startNumber, float endNumber, AnimationCurve curve, float duration) {
        start = startNumber;
        end = endNumber;
        this.duration = duration;
        this.curve = curve;
        if (curve == null) {
            this.curve = new AnimationCurve();
            //Set up a linear curve by default
            float tan45 = Mathf.Tan(Mathf.Deg2Rad * 45);
            this.curve.AddKey(new Keyframe(0, 0, tan45, tan45));
            this.curve.AddKey(new Keyframe(1, 1, tan45, tan45));
        }
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
        if (curve != null) {
            this.curve = curve;
        }
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
                if (OnFinish != null) {
                    OnFinish();
                }
                running = false;
            }
        }
        return outNumber;
    }
}
