using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCamController : MonoBehaviour
{
    //Camera input/rotation
    float mouseX, mouseY;
    float multiplier = 0.01f;
    float xRotation, yRotation;

    //Camera sens
    public float sensX;
    public float sensY;

    //Camera objects
    public Camera cam;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Late update so that the rotation modifer is always calculated first
    void LateUpdate()
    {
        MouseInput();

        cam.transform.localRotation = Quaternion.Euler(cam.transform.localEulerAngles - Vector3.right * xRotation);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles + Vector3.up * yRotation);
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
