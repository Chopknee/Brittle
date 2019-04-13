using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextScrollInteractive: MonoBehaviour {
    public string componentText = "";
    [TextArea]
    public string scrollText;
    public char[] scrollTextSplit;
    public int selectedTextType, fontSize;
    [Range(1, 500)]
    public float CharacterDelay = 80;
    public float volume = .2f;
    public Vector2 size;
    public Color textColor;
    public Object scrollSound;
    public GameObject prefab;
    [Range(1, 500)]
    public float periodDelay = 150;
    [Range(1, 500)]
    public float commaDelay = 125;

    public delegate void Finished ();
    public Finished OnFinished;
    
    float next;
    bool scroll;

    string oldText = "";

    private void Awake () {

        scrollTextSplit = scrollText.ToCharArray();

        ScrollText(scrollText, selectedTextType, prefab);
        if (scrollSound == null) scrollSound = Resources.Load("Prefabs/boop");
        GetComponent<AudioSource>().clip = (AudioClip) scrollSound;
        GetComponent<AudioSource>().volume = volume;
        componentText = "";

        oldText = scrollText;
    }
    int i = 0;
    private void Update () {
        if (oldText != scrollText) {
            i = 0;
            scrollTextSplit = scrollText.ToCharArray();
            componentText = "";
            scroll = true;
            oldText = scrollText;
        }

        if (scroll && Time.time > next && i < scrollTextSplit.Length) {
            next = Time.time + (CharacterDelay / 1000);

            switch (scrollTextSplit[i]) {
                case ',':
                    next = ( commaDelay / 1000 );
                    break;
                case '.':
                    next = (periodDelay/ 1000);
                    break;
                case '!':
                    next = ( periodDelay / 1000 );
                    break;
                case '?':
                    next = ( periodDelay / 1000 );
                    break;
            }
            componentText += scrollTextSplit[i];
            GetComponent<AudioSource>().Play();

            switch (selectedTextType) {
                case 0:
                    GetComponent<TextMeshProUGUI>().text = componentText;

                    GetComponent<TextMeshProUGUI>().fontSize = fontSize;

                    GetComponent<TextMeshProUGUI>().color = textColor;

                    break;
                case 1:
                    GetComponent<Text>().text = componentText;
                    GetComponent<Text>().fontSize = fontSize;

                    GetComponent<Text>().color = textColor;

                    break;
            }

            i++;
            if (i >= scrollTextSplit.Length) {
                if (Application.isPlaying) {
                    if (OnFinished != null)
                        OnFinished();

                    scroll = false;
                } else {
                    i = 0;
                    componentText = "";
                }
            }
        }

    }
    
    public void ScrollText ( string scrollText, int selectedTextType, GameObject prefab ) {
        transform.GetComponent<RectTransform>().sizeDelta = size;

        switch (selectedTextType) {
            case 0:
                componentText = GetComponent<TextMeshProUGUI>().text;
                break;
            case 1:
                componentText = GetComponent<Text>().text;
                break;
        }
        scroll = true;

    }
}
