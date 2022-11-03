using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBouncing : MonoBehaviour
{
    public float bounceHeight;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float bounceSpeed;
    private float currentPos;
    private bool goingUp;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + (transform.up * bounceHeight);
        currentPos = 0;
        goingUp = true;
    }

    private void FixedUpdate()
    {
        if (goingUp)
        {
            currentPos += bounceSpeed * Time.deltaTime;
        }
        else
        {
            currentPos -= bounceSpeed * Time.deltaTime;
        }
        transform.position = Vector3.Lerp(startPosition, endPosition, currentPos);
        if (currentPos >= 1)
        {
            goingUp = false;
        }
        else if (currentPos <= 0)
        {
            goingUp = true;
        }
        
    }

}
