using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScrollInteractive: MonoBehaviour {

    [TextArea]
    public string SpeechText;
    [Range(0f, 1f)]
    public float scrollVolume = .2f;
    public AudioClip scrollSound;

    [Range(1, 500)]
    public float CharacterDelay = 80;
    [Range(1, 500)]
    public float periodDelay = 150;
    [Range(1, 500)]
    public float commaDelay = 125;
    [Range(0, 5000)]
    public float onFinishDelay = 100;

    public delegate void Finished ();
    public Finished OnFinished;

    public Text myText;

    public char[] scrollTextSplit;
    public string currentText = "";

    public float next;
    public bool scroll;

    public int i = 0;

    public void Awake () {

        scrollTextSplit = SpeechText.ToCharArray();

        if (scrollSound != null && GetComponent<AudioSource>() != null) {
            GetComponent<AudioSource>().clip = scrollSound;
            GetComponent<AudioSource>().volume = scrollVolume;
        }

        if (myText == null) {
            myText = GetComponent<Text>();
        }

        currentText = "";
    }

    public void Update () {

        if (scroll) {

            if (i > 5 && (Input.GetButton("Interact") || Input.GetButton("Submit")) && i < scrollTextSplit.Length - 1) {
                //Skip to the end of the dialog, then add some delay befor the onfinished is run.
                currentText = SpeechText.Substring(0, SpeechText.Length - 1);
                i = scrollTextSplit.Length - 1;
            }

            if (Time.time > next) {

                next = Time.time + (CharacterDelay / 1000);
                if (i < scrollTextSplit.Length) {
                    switch (scrollTextSplit[i]) {
                        case ',':
                            next = Time.time + (commaDelay / 1000);
                            break;
                        case '.':
                            next = Time.time + (periodDelay / 1000);
                            break;
                        case '!':
                            next = Time.time + (periodDelay / 1000);
                            break;
                        case '?':
                            next = Time.time + (periodDelay / 1000);
                            break;
                    }
                    currentText += scrollTextSplit[i];

                    if (scrollSound != null && GetComponent<AudioSource>() != null) {
                        GetComponent<AudioSource>().Play();
                    }
                    myText.text = currentText;
                }

                i++;

                if (i == scrollTextSplit.Length) {

                    next = Time.time + (onFinishDelay / 1000);

                } else if (i > scrollTextSplit.Length) {
                    scroll = false;
                    if (OnFinished != null)
                        OnFinished();
                }
            }
        }
    }

    public void BeginScroll ( string speechText ) {
        SpeechText = speechText;
        scrollTextSplit = SpeechText.ToCharArray();
        currentText = "";
        myText.text = currentText;
        i = 0;
        next = Time.time;
        scroll = true;
        GetComponent<AudioSource>().clip = scrollSound;
    }

    public void EndScroll() {
        scroll = false;
    }


}