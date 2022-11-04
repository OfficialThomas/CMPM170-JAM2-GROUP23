using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CubeGameController : MonoBehaviour
{
    //Items and score
    public float score;
    private float scoreToWin;
    private bool movingSpawned = false;
    public List<GameObject> staticItems;
    public List<GameObject> movingItems;

    //UI
    private Label scoreText;
    public UIDocument uiDoc;
    private VisualElement frame;

    //Scene transition
    public string nextScene;
    public float delayBeforeTransition;
    public float delayTimer;
    
    void Start()
    {
        score = 0;
        scoreToWin = staticItems.Count + movingItems.Count;
        var rootVisualElement = uiDoc.rootVisualElement;
        frame = rootVisualElement.Q<VisualElement>("Frame");
        scoreText = frame.Q<Label>("Score");
        delayTimer = delayBeforeTransition;
    }

    void Update()
    {
        scoreText.text = score.ToString() + "/" + scoreToWin.ToString();
        
        //Once player has collected all items, transitions player to new scene after a time delay
        if (score == scoreToWin && delayTimer <= 0)
        {
            SceneManager.LoadScene(nextScene);
        }
        else if (score == scoreToWin)
        {
            delayTimer -= Time.deltaTime;
        }

        //Turns on the moving items once all static items are gone
        if (score >= staticItems.Count && !movingSpawned)
        {
            movingSpawned = true;
            for (int i = 0; i < movingItems.Count; i++)
            {
                movingItems[i].SetActive(true);
            }
        }
    }
}
