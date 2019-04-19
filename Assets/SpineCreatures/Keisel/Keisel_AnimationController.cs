using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Keisel_AnimationController : MonoBehaviour, IPausable {

    private SkeletonAnimation skeletonAnimation;
    public float horizontalSpeed = 0;
    public float verticalSpeed = 0;
    public float runSpeedMultiplier = 1;
    public float jumpAnimationSpeed = 1;
    public float climbAnimationSpeedMultiplier = 1;
    public string animationState = "idle";
    public bool loopState = true;
    public bool falling = false;
    public bool running = false;
    public bool grounded = false;
    public bool jumping = false;
    public bool climbing = false;
    public bool stateOverride = false;
    public float timeScale = 1;
    

    public string runAnimationName = "run";
    public string idleAnimationName = "idle";
    public string jumpAnimationName = "jumpstart";
    public string fallingAnimationName = "jumpair";
    public string walkAnimationame = "Walk";
    public string climbAnimationName = "climb";
    public string damageAnimationName = "Damage";

    public string lastAnimationState = "";

    public bool freezeAnimations = false;

    void Start () {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();

        skeletonAnimation.AnimationState.Complete += AnimationEnded;

        //skeletonAnimation.initialSkinName = "front";

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
            if (!stateOverride) {
                //Keep the running animation up to date.
                if (running && !climbing) {//If we are moving and not jumping
                    if (animationState != runAnimationName) {
                        Run();
                    } else {
                        skeletonAnimation.timeScale = horizontalSpeed * runSpeedMultiplier;
                    }
                } else if (!running && !climbing) {
                    Idle();
                }

                //If we are not moving, not falling, and on the ground
                if (horizontalSpeed < 0.01f && !falling && !jumping && !climbing) {
                    if (animationState != idleAnimationName) {
                        Idle();
                    }
                }

                //If we are falling.
                if (falling && !climbing) {
                    if (animationState != fallingAnimationName) {
                        Falling();
                    }
                }

                if (climbing) {
                    if (animationState != climbAnimationName) {
                        Climb();
                    } else {
                        skeletonAnimation.timeScale = Mathf.Abs(verticalSpeed) * climbAnimationSpeedMultiplier;
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
        skeletonAnimation.skeleton.SetSkin("front");
    }

    public void Climb() {
        animationState = climbAnimationName;
        timeScale = 1;
        skeletonAnimation.skeleton.SetSkin("back");
        stateOverride = true;
        Invoke("ResetOverride", 0.1f);
    }

    public void FlipDirection(int direction) {
        skeletonAnimation.Skeleton.ScaleX = direction;
    }

    private void Run() {
        animationState = runAnimationName;
        loopState = true;
        skeletonAnimation.skeleton.SetSkin("front");
    }

    private void Idle() {
        animationState = idleAnimationName;
        loopState = true;
        timeScale = 1;
        skeletonAnimation.skeleton.SetSkin("front");
    }

    private void Falling() {
        animationState = fallingAnimationName;//Set to falling at some point in the future.
        loopState = true;
        timeScale = 1;
        skeletonAnimation.skeleton.SetSkin("front");
    }

    private void AnimationEnded(TrackEntry trackEntry) {

    }

    private void ResetOverride() {
        stateOverride = false;
    }

    public void OnPause() {
        freezeAnimations = true;
        skeletonAnimation.timeScale = 0;
    }

    public void OnUnPause() {
        freezeAnimations = false;
        skeletonAnimation.timeScale = 1;
    }
}
