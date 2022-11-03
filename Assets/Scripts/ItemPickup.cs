using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public CubeGameController gameController;
    public AudioSource audioSource;
    public AudioClip gotBall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameController.score += 1;
            audioSource.PlayOneShot(gotBall);
            gameObject.SetActive(false);
        }
    }
}
