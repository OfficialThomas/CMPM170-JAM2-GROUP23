using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    private float timer;
    public float timerLength;
    private float fadeTimer;
    public float fadeTimerLength;
    public CameraManager fadeControl;
    private bool fadingStarted = false;

    //UI
    private Label scoreText;
    public UIDocument uiDoc;
    private VisualElement frame;


    private void Start()
    {
        fadeTimer = fadeTimerLength;
        timer = timerLength;
        var rootVisualElement = uiDoc.rootVisualElement;
        frame = rootVisualElement.Q<VisualElement>("Frame");
        scoreText = frame.Q<Label>("Score");
        scoreText.text = "The End.";
    }

    private void Update()
    {
        if (timer > 0)
        {

            timer -= Time.deltaTime;
        } 
        else if (timer <= 0 && !fadingStarted)
        {
            fadingStarted = true;
            fadeControl.FadeOut();
        }

        if (fadingStarted && fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
        } 
    }
}
