using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera sens
    public float sensX;
    public float sensY;

    //Camera objects
    public Camera cam;
    public Transform rotator;

    //Camera input/rotation
    float mouseX, mouseY;
    float multiplier = 0.01f;
    float xRotation, yRotation;
    public float modifier;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Moves the players camera, adding the modifier given by the player controller script
    //Uses late update so that the modifier is always calculated first
    void LateUpdate()
    {
        MouseInput();

        cam.transform.localRotation = Quaternion.Euler(cam.transform.localEulerAngles - Vector3.right * xRotation);
        rotator.localRotation = Quaternion.Euler(rotator.localEulerAngles + Vector3.up * (yRotation + modifier));
    }

    //Reads in the mouse input
    void MouseInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation = mouseX * sensX * multiplier;
        xRotation = mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
