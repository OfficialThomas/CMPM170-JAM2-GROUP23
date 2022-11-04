using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;
    public CameraManager fadeControl;
    private bool isCollided = false;
    public float timer = 2f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            fadeControl.FadeOut();
            isCollided = true;
        }
    }

    void Update(){
        if (isCollided){
            timer -= Time.deltaTime;
            if (timer < 0){
                SceneManager.LoadScene(sceneName);
            }
        }
    }

}
