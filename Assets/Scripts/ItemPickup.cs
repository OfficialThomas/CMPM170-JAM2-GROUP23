using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public CubeGameController gameController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //DO SOME UI STUFF
            gameController.score += 1;
            gameObject.SetActive(false);
        }
    }
}