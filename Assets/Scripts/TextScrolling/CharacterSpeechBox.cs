using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpeechBox : MonoBehaviour {

    [SerializeField]
    public ChatBox[] Dialog;
    public Image CharacterImage;
    public Image nextImage;
    public bool playOnAwake = false;

    public delegate void Finished ();
    public Finished OnFinished;

    public int DialogIndex {
        get {
            return dialogIndex;
        }
        set {
            if (value > -1 && value < Dialog.Length) {
                dialogIndex = value;
                SetCurrentDialog();
                myTextScrollInteractive.BeginScroll(Dialog[dialogIndex].Speech);
            } else {
                throw new Exception("Invalid range!");
            }
        }
    }

    private int dialogIndex = 0;

    private bool canPlay = false;
    private bool willWaitForUser = false;
    private bool isWaitingForUser = false;

    public bool Playing {
        get {
            return playing;
        }
        set {
            playing = value;
            if (playing) {
                BeginDialog();
            } else {
                myTextScrollInteractive.EndScroll();
            }
        }
    }

    public void Update () {
        if (Playing && isWaitingForUser && Input.GetButton("Interact")) {
            nextImage.gameObject.SetActive(false);
            isWaitingForUser = false;
            if (dialogIndex >= Dialog.Length) {
                playing = false;
                if (OnFinished != null) {
                    OnFinished();
                }
                return;
            }
            SetCurrentDialog();
            myTextScrollInteractive.BeginScroll(Dialog[dialogIndex].Speech);
        }
    }

    private TextScrollInteractive myTextScrollInteractive;

    private bool playing = false;
	// Use this for initialization
	void Start () {
		if (Dialog != null && Dialog.Length > 0 && GetComponentInChildren<TextScrollInteractive>() != null) {
            myTextScrollInteractive = GetComponentInChildren<TextScrollInteractive>();
            myTextScrollInteractive.OnFinished += SpeechFinished;
            canPlay = true;
        }
        if (playOnAwake) {
            Playing = true;
        }
	}

    public void SpeechFinished() {

        dialogIndex++;
        if (dialogIndex >= Dialog.Length && !willWaitForUser || !playing) {
            playing = false;
            if (OnFinished != null) {
                OnFinished();
            }
            return;
        }
        if (!willWaitForUser) {
            SetCurrentDialog();
            myTextScrollInteractive.BeginScroll(Dialog[dialogIndex].Speech);
        } else {
            isWaitingForUser = true;
            nextImage.gameObject.SetActive(true);
        }
    }

    public void BeginDialog() {
        playing = true;
        SetCurrentDialog();
        myTextScrollInteractive.BeginScroll(Dialog[dialogIndex].Speech); 
    }

    public void SetCurrentDialog () {
        if (canPlay) {
            myTextScrollInteractive.SpeechText = Dialog[dialogIndex].Speech;
            myTextScrollInteractive.CharacterDelay = Dialog[dialogIndex].CharacterDelay;
            myTextScrollInteractive.periodDelay = Dialog[dialogIndex].periodDelay;
            myTextScrollInteractive.commaDelay = Dialog[dialogIndex].commaDelay;
            myTextScrollInteractive.onFinishDelay = Dialog[dialogIndex].OnFinishDelay;
            if (Dialog[dialogIndex].beep != null) {
                myTextScrollInteractive.scrollSound = Dialog[dialogIndex].beep;
            }
            if (CharacterImage != null) {
                CharacterImage.sprite = Dialog[dialogIndex].Character;
            }
            willWaitForUser = Dialog[dialogIndex].WaitforUser;
            if (willWaitForUser) {
                myTextScrollInteractive.onFinishDelay = 100;//Fixed value since the user needs to skip it.
            }
        }
    }

    [Serializable]
    public class ChatBox {
        public Sprite Character;
        [TextArea]
        public string Speech = "";
        [Range(1, 500)]
        public float CharacterDelay = 80;
        [Range(1, 500)]
        public float periodDelay = 150;
        [Range(1, 500)]
        public float commaDelay = 125;
        [Range(0, 5000)]
        public float OnFinishDelay = 100;
        public bool WaitforUser = true;
        public AudioClip beep = null;
    }
}
