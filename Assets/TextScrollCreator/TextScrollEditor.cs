using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class TextScrollEditor: EditorWindow {
    [MenuItem("TextScroller/Add New")]
    static void Init () {
        TextScrollEditor window = (TextScrollEditor) EditorWindow.GetWindow(typeof(TextScrollEditor));
        window.Show();
    }

    string scrollingTextField = "This Text Will Scroll.";
    float stfHeight = 100;
    string[] radioDesc = { "Text Mesh Pro", "Unity3D Text" };
    int xCount = 1;
    int selectedType = 0;
    public GameObject tmpPrefab;
    public Object scrollSound;
    int pauses;
    float pauseDuration;
    float scrollSpeed = .06f;
    float movementSpeed;
    int[] pauseLocations;
    Vector2 dimensions = Vector2.one * 400;
    Color textColor = new Color(0, 0, 0);
    Vector2 scroll = Vector2.zero;
    int fontSize = 14;
    bool playSound = true;

    string[] fontSizes = { "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "24", "32", "48", "72" };

    void OnGUI () {
        GUI.backgroundColor = new Color(.5f, .5f, .5f);
        GUILayout.Label("Text Settings:", EditorStyles.boldLabel);

        tmpPrefab = (GameObject) EditorGUILayout.ObjectField("Text Prefab/Object", tmpPrefab, typeof(GameObject), true);
        if (Selection.activeGameObject != null) tmpPrefab = Selection.activeGameObject;


        scrollSound = EditorGUILayout.ObjectField("Scroll Sound Click", scrollSound, typeof(Object), true);

        EditorGUILayout.LabelField("Select from the inspector or as a prefab from the project explorer.", EditorStyles.helpBox);


        GUILayout.Label("Text to be scrolled:", EditorStyles.whiteMiniLabel);
        scroll = GUILayout.BeginScrollView(scroll, false, false);
        scrollingTextField = EditorGUILayout.TextArea(scrollingTextField, GUILayout.Height(80));
        
        GUILayout.EndScrollView();

        GUILayout.Label("Character Scroll Speed:", EditorStyles.largeLabel);
        scrollSpeed = EditorGUILayout.Slider(scrollSpeed, .01f, .5f);
        EditorGUILayout.LabelField("This slider controls the speed at which the individual characters will be shown.", EditorStyles.helpBox);


        GUILayout.Label("Full Text Scroll Speed:", EditorStyles.largeLabel);
        movementSpeed = EditorGUILayout.Slider(movementSpeed, 0f, 5f);
        EditorGUILayout.LabelField("This slider controls the speed at which the entire text chunk will move.", EditorStyles.helpBox);


        playSound = EditorGUILayout.Toggle("Play Sound Effect", playSound);
        EditorGUILayout.LabelField("Checking this will enable clicking sounds. Change this effect at the top of the window.", EditorStyles.helpBox);


        GUILayout.Label("Font Size:", EditorStyles.largeLabel);
        fontSize = EditorGUILayout.Popup(fontSize, fontSizes);

        GUILayout.Label("Text Color:", EditorStyles.largeLabel);
        textColor = EditorGUILayout.ColorField(textColor);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Dimensions:", EditorStyles.boldLabel);

        GUILayout.Label("Width", EditorStyles.boldLabel);
        dimensions.x = EditorGUILayout.FloatField(dimensions.x);
        GUILayout.Label("Height", EditorStyles.boldLabel);
        dimensions.y = EditorGUILayout.FloatField(dimensions.y);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.Label("*Emphasis* Pauses:", EditorStyles.largeLabel);
        //        GUILayout.Label("Number of Pauses:", EditorStyles.whiteMiniLabel);
        GUILayout.Label("Pause locations:", EditorStyles.whiteMiniLabel);
        EditorGUILayout.LabelField("Currently, pauses automatically occur for each comma or period.", EditorStyles.helpBox);




        GUILayout.Label("Text Type:", EditorStyles.boldLabel);
        selectedType = GUILayout.SelectionGrid(selectedType, radioDesc, xCount, EditorStyles.radioButton);


        bool btn = GUILayout.Button("Submit");
        if (btn) SetText(scrollingTextField, selectedType, tmpPrefab);
    }

    void SetText ( string scrollText, int selectedTextType, GameObject prefab ) {
        if (prefab != null) {
            TextScrollInteractive tsi = prefab.AddComponent<TextScrollInteractive>();
            AudioSource x = prefab.AddComponent<AudioSource>();
            x.playOnAwake = false;
            x.loop = false;


            switch (selectedTextType) {
                case 0:
                    if (prefab.GetComponent<TextMeshProUGUI>() == null) prefab.AddComponent<TextMeshProUGUI>();
                    break;
                case 1:
                    if (prefab.GetComponent<Text>() == null) prefab.AddComponent<Text>();
                    break;
            }
            tsi.scrollText = scrollText;
            tsi.prefab = prefab;
            tsi.selectedTextType = selectedTextType;
            tsi.textColor = textColor;
            tsi.CharacterDelay = scrollSpeed;
            tsi.fontSize = int.Parse(fontSizes[fontSize]);
            tsi.size = dimensions;
            if (playSound == false) {
                tsi.volume = 0;
            }
            tsi.scrollSound = scrollSound;
        }
    }
}