using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunawayItemRotation : MonoBehaviour
{
    //Objects
    public Transform rotator;

    //rotation
    float xRotation, yRotation;
    public float modifier;

    //Late update so that the rotation modifer is always calculated first
    void LateUpdate()
    {
        rotator.localRotation = Quaternion.Euler(rotator.localEulerAngles + Vector3.up * modifier);
    }
}
