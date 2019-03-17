using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeiselCenimaticAnimations : MonoBehaviour {

    private SkeletonAnimation skel;
    public float AnimationSpeed = 0;

    public enum KAnimState { IDLE, RUNNING, FALLING, JUMPING };
    public KAnimState currentAnimation;
    public bool looping = false;

    private KAnimState lastAnim;
    private float lastSpd;
    // Use this for initialization
    void Start () {
        if (skel == null) {
            skel = GetComponent<SkeletonAnimation>();
        }
        lastAnim = currentAnimation;
        lastSpd = AnimationSpeed;
    }
	
	// Update is called once per frame
	void Update () {
		if (lastAnim != currentAnimation) {
            skel.AnimationState.SetAnimation(0, getAnimationName(currentAnimation), looping);
            lastAnim = currentAnimation;
        }
        if (lastSpd != AnimationSpeed) {
            skel.timeScale = AnimationSpeed;
            lastSpd = AnimationSpeed;
        }
	}

    public string getAnimationName(KAnimState k) {
        string state = "idle";
        switch (k) {
            case KAnimState.IDLE:
                state = "idle";
                break;
            case KAnimState.RUNNING:
                state = "run";
                break;
            case KAnimState.FALLING:
                state = "jumpstart";
                break;
            case KAnimState.JUMPING:
                state = "jumpair";
                break;
        }
        return state;
    }
}
