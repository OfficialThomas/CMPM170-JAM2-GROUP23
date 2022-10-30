using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
    //Movement
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    private Vector3 moveDirection;
    private float horizontalMovement;
    private float verticalMovement;
    public float groundDrag = 2f;

    //Jump Movement
    public float airDrag = 0.5f;
    public float jumpPower;
    public float airMultiplier = .2f;
    bool isGrounded;
    public float gravity = -9.81f;

    public Rigidbody rb;

    //Layer masks
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask planetLayer;

    //Planet rotation
    public Transform core;
    private Vector3 lastSide;
    public Transform rotator;
    public CameraController camControl;
    RaycastHit hit;

    void Start()
    {
        rb.freezeRotation = true;
        lastSide = new Vector3(0, 1, 0);
        Physics.gravity = lastSide * gravity;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, groundMask);

        GravityCheck();
        PlayerInput();
        ControlDrag();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    //Player Jump
    private void Jump()
    {
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    //Checks if the player is trying to move to a different side of the planet by shooting
    //a line from the player to the center of the planet and checking the normals
    //of the surface hit. If it is a new surface, updates the gravity and player orientation
    private void GravityCheck()
    {
        hit = new RaycastHit();
        Physics.Linecast(transform.position, core.position, out hit, planetLayer);
        camControl.modifier = 0;
        if (lastSide != hit.normal)
        {
            EditModifier();
            lastSide = hit.normal;
            transform.up = hit.normal;
            Physics.gravity = hit.normal * gravity;
        }
    }

    //Sets the camController modifier which corrects the rotation of the player
    //when going between certain sides
    private void EditModifier()
    {
        //front to left and right to front
        if ((lastSide == Vector3.forward && hit.normal == Vector3.left) || (lastSide == Vector3.right && hit.normal == Vector3.forward))
        {
            camControl.modifier = -90;
        }
        //left to front and front to right
        else if ((lastSide == Vector3.left && hit.normal == Vector3.forward) || (lastSide == Vector3.forward && hit.normal == Vector3.right))
        {
            camControl.modifier = 90;
        }
        //back to left and right to back
        if ((lastSide == Vector3.back && hit.normal == Vector3.left) || (lastSide == Vector3.right && hit.normal == Vector3.back))
        {
            camControl.modifier = 90;
        }
        //left to back and back to right
        else if ((lastSide == Vector3.left && hit.normal == Vector3.back) || (lastSide == Vector3.back && hit.normal == Vector3.right))
        {
            camControl.modifier = -90;
        }
        //bottom to left and right to bottom
        if ((lastSide == Vector3.down && hit.normal == Vector3.left) || (lastSide == Vector3.right && hit.normal == Vector3.down))
        {
            camControl.modifier = 180;
        }
        //left to bottom and bottom to right
        else if ((lastSide == Vector3.left && hit.normal == Vector3.down) || (lastSide == Vector3.down && hit.normal == Vector3.right))
        {
            camControl.modifier = -180;
        }
    }

    //Reads in players input
    private void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = rotator.forward * verticalMovement + rotator.right * horizontalMovement;
    }

    //Moves the player, speed of movement is based on whether player is in the air or on the ground
    private void MovePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }

    }

    //Updates the drag of the player based on whether they are in the air or on ground
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

    }
}
