using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Keisel_AnimationController : MonoBehaviour {

    private SkeletonAnimation skeletonAnimation;
    public float horizontalSpeed = 0;
    public float verticalSpeed = 0;
    public float runSpeedMultiplier = 1;
    public float jumpAnimationSpeed = 1;
    public string animationState = "idle";
    public bool loopState = true;
    public bool falling = false;
    public bool grounded = false;
    public bool jumping = false;
    public bool stateOverride = false;
    public float timeScale = 1;
    

    public string runAnimationName = "run";
    public string idleAnimationName = "idle";
    public string jumpAnimationName = "jumpstart";
    public string fallingAnimationName = "jumpair";

    public string lastAnimationState = "";

    public bool freezeAnimations = false;

    void Start () {
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        skeletonAnimation.AnimationState.Complete += AnimationEnded;

	}
	
	void Update () {
        if (!freezeAnimations) {
            //Update the current animation
            skeletonAnimation.timeScale = timeScale;
            if (animationState != lastAnimationState) {
                skeletonAnimation.AnimationState.SetAnimation(0, animationState, loopState);
                lastAnimationState = animationState;
            }
            //Check if the character is falling.
            falling = ( verticalSpeed < 0 && !grounded ) ? true : false;
            jumping = ( verticalSpeed > 0 && !grounded ) ? true : false;
            if (!stateOverride) {
                //Keep the running animation up to date.
                if (horizontalSpeed > 0 && ( !jumping && !falling )) {//If we are moving and not jumping
                    if (animationState != runAnimationName) {
                        Run();
                    } else {
                        skeletonAnimation.timeScale = horizontalSpeed * runSpeedMultiplier;
                    }
                }

                //If we are not moving, not falling, and on the ground
                if (horizontalSpeed < 0.01f && !falling && !jumping) {
                    if (animationState != idleAnimationName) {
                        Idle();
                    }
                }

                //If we are falling.
                if (falling) {
                    if (animationState != fallingAnimationName) {
                        Falling();
                    }
                }
            }
        }
    }

    public void Jump() {
        animationState = jumpAnimationName;
        loopState = false;
        timeScale = jumpAnimationSpeed;
        stateOverride = true;
        Invoke("ResetOverride", 0.7f);
    }

    public void FlipDirection(int direction) {
        skeletonAnimation.Skeleton.ScaleX = direction;
    }

    private void Run() {
        animationState = runAnimationName;
        loopState = true;
    }

    private void Idle() {
        animationState = idleAnimationName;
        loopState = true;
        timeScale = 1;
    }

    private void Falling() {
        animationState = fallingAnimationName;//Set to falling at some point in the future.
        loopState = true;
        timeScale = 1;
    }

    private void AnimationEnded(TrackEntry trackEntry) {

    }

    private void ResetOverride() {
        stateOverride = false;
    }
}
