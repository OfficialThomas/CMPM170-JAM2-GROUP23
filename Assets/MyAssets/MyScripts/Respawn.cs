using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint;
    public CameraManager fadeControl;

    private Label skipText;
    public UIDocument uiDoc;
    private VisualElement frame;

    private int deathCount = 0;

    //sounds N' Stuff
    public AudioSource audioSource;

    public AudioClip reviveSound1;
    public AudioClip reviveSound2;
    public AudioClip reviveSound3;

    public ArrayList soundArray = new ArrayList();

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            other.transform.position = respawnPoint.transform.position;
            int num = new System.Random().Next(0, soundArray.Count);
            audioSource.PlayOneShot((AudioClip)soundArray[num]);
            fadeControl.FadeIn();
            deathCount++;
        }
        //print(other.tag);
    }
    // Start is called before the first frame update
    void Start()
    {
        soundArray.Add(reviveSound1);
        soundArray.Add(reviveSound2);
        soundArray.Add(reviveSound3);

        var rootVisualElement = uiDoc.rootVisualElement;
        frame = rootVisualElement.Q<VisualElement>("Frame");
        skipText = frame.Q<Label>("Skip");
        skipText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (deathCount > 3){
            skipText.text = "Press H to skip to the next room";
        }
    }
}
