using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    //public GameObject player;

    public int direction = 1;
    public float move = 25f;

    // Start is called before the first frame update
    void Start()
    {
        switch (direction){
            case 1:
                move = 25f;
                break;
            case 2:
                move = -25f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + move, other.transform.position.z);
        }
    }
}
