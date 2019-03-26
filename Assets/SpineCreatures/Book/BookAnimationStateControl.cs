using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimationStateControl : MonoBehaviour {

    private SkeletonGraphic skel;
    public float AnimationSpeed = 0;
    private float AnimationDirection = 0;
    public enum KAnimState { IDLE, OPEN, CLOSE };
    public KAnimState currentAnimation;
    public bool looping = false;

    private KAnimState lastAnim;
    private float lastSpd;
    // Use this for initialization
    void Start () {
        if (skel == null) {
            skel = GetComponent<SkeletonGraphic>();
        }
        lastAnim = currentAnimation;
        lastSpd = AnimationSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (skel != null) {
            if (lastAnim != currentAnimation) {
                skel.AnimationState.SetAnimation(0, getAnimationName(currentAnimation), looping);
                lastAnim = currentAnimation;
            }
            if (lastSpd != AnimationSpeed * AnimationDirection) {
                skel.timeScale = AnimationSpeed * AnimationDirection;
                lastSpd = AnimationSpeed * AnimationDirection;
            }
        }
	}

    public string getAnimationName(KAnimState k) {
        string state = "";
        switch (k) {
            case KAnimState.IDLE:
                state = "animation";
                AnimationDirection = 1;
                break;
            case KAnimState.OPEN:
                state = "animation";
                AnimationDirection = 1;
                break;
            case KAnimState.CLOSE:
                state = "animation";
                AnimationDirection = -1;
                break;
        }
        return state;
    }
}
