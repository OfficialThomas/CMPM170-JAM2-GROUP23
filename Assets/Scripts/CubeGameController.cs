using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CubeGameController : MonoBehaviour
{
    public float score;
    private float scoreToWin;
    private bool movingSpawned = false;
    public List<GameObject> staticItems;
    public List<GameObject> movingItems;

    private Label scoreText;
    public UIDocument uiDoc;
    private VisualElement frame;
    

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreToWin = staticItems.Count + movingItems.Count;
        var rootVisualElement = uiDoc.rootVisualElement;
        frame = rootVisualElement.Q<VisualElement>("Frame");
        scoreText = frame.Q<Label>("Score");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString() + "/6";

        //Turns on the moving items once as static items are gone
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
