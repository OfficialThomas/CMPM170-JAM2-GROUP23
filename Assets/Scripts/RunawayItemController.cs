using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunawayItemController : MonoBehaviour
{
    //Scene stuff
    public bool planetGravity;

    //Movement
    public float moveSpeed = 8f;
    public float movementMultiplier = 10f;
    private Vector3 moveDirection;
    private float horizontalMovement;
    private float verticalMovement;
    public float groundDrag = 5f;

    //Random Movement Timer
    private float movementTimer;
    public float movementTimerLength;

    //Jump Movement
    public float airDrag = 1f;
    public float jumpPower = 10;
    public float airMultiplier = .2f;
    private bool isGrounded;
    public float gravity = -20f;
    private Vector3 gravityVector;

    public Rigidbody rb;

    //Layer masks
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask planetLayer;

    //Item rotation
    public Transform core;
    public Transform player;
    private Vector3 lastSide, currentSide;
    public Transform rotator;
    public RunawayItemRotation itemRotationScript;
    public float rotationSpeed = 6f;
    private float currentRotation;
    RaycastHit hit;
    private float camCorrection;
    private bool camFlipped = true;

    void Start()
    {
        rb.freezeRotation = true;
        lastSide = new Vector3(0, 1, 0);
        currentSide = new Vector3(0, 1, 0);
        gravityVector = lastSide * gravity;
        currentRotation = 0;
        camCorrection = 0;
        movementTimer = 0;
        core = GameObject.Find("Core").transform;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, groundMask);
        GravityCheck();
        ItemInput();
        ControlDrag();
    }

    private void FixedUpdate()
    {
        MoveItem();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        rb.AddForce(gravityVector, ForceMode.Acceleration);
    }

    //Item Jump
    private void Jump()
    {
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }


    //Checks if the item is trying to move to a different side of the planet by shooting
    //a line from the item to the center of the planet and checking the normals
    //of the surface hit. If it is a new surface, updates the gravity and item orientation
    private void GravityCheck()
    {
        hit = new RaycastHit();
        Physics.Linecast(transform.position, core.position, out hit, planetLayer);
        itemRotationScript.modifier = 0;
        //checks if item needs to rotate to new side
        if (currentSide != hit.normal)
        {
            currentSide = hit.normal;
            gravityVector = currentSide * gravity;
            currentRotation = 0;
            camCorrection = 0;
            camFlipped = false;
        }
        //Checks if the item needs to rotate more
        if (transform.up != hit.normal)
        {
            Rotate();
        }
        else
        {
            lastSide = hit.normal;
        }
    }

    //Uses lerp to gradually rotate item when moving to a new side
    private void Rotate()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        transform.up = Vector3.Lerp(lastSide, hit.normal, currentRotation);
        EditModifier();
    }

    //Sets the camController modifier which corrects the rotation of the item
    //when going between certain sides
    private void EditModifier()
    {
        //front to left and right to front
        if ((lastSide == Vector3.forward && hit.normal == Vector3.left) || (lastSide == Vector3.right && hit.normal == Vector3.forward))
        {
            itemRotationScript.modifier = Mathf.Lerp(0, -90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, -90, currentRotation);
        }
        //left to front and front to right
        else if ((lastSide == Vector3.left && hit.normal == Vector3.forward) || (lastSide == Vector3.forward && hit.normal == Vector3.right))
        {
            itemRotationScript.modifier = Mathf.Lerp(0, 90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, 90, currentRotation);
        }
        //back to left and right to back
        if ((lastSide == Vector3.back && hit.normal == Vector3.left) || (lastSide == Vector3.right && hit.normal == Vector3.back))
        {
            itemRotationScript.modifier = Mathf.Lerp(0, 90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, 90, currentRotation);
        }
        //left to back and back to right
        else if ((lastSide == Vector3.left && hit.normal == Vector3.back) || (lastSide == Vector3.back && hit.normal == Vector3.right))
        {
            itemRotationScript.modifier = Mathf.Lerp(0, -90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, -90, currentRotation);
        }
        //bottom to left and bottom to right, flips camera 180 at the start of the rotation
        if ((lastSide == Vector3.down && hit.normal == Vector3.left) || (lastSide == Vector3.down && hit.normal == Vector3.right))
        {
            if (!camFlipped)
            {
                camFlipped = true;
                itemRotationScript.modifier = 180;
            }
        }
        //right to bottom and left to bottom, flips camera 180 at the veery end of the rotaiton
        else if ((lastSide == Vector3.right && hit.normal == Vector3.down) || (lastSide == Vector3.left && hit.normal == Vector3.down))
        {
            if (!camFlipped && (transform.rotation.x == 1))
            {
                camFlipped = true;
                itemRotationScript.modifier = 180;
            }
        }
    }

    //Movement input for the input
    private void ItemInput()
    { 
        if (movementTimer <= 0)
        {
            movementTimer = movementTimerLength;
            verticalMovement = Random.Range(1f, 2f) * RandomSign();
            horizontalMovement = Random.Range(1f, 2f) * RandomSign();
        } 
        else
        {
            movementTimer -= Time.deltaTime;
        } 
        moveDirection = rotator.forward * verticalMovement + rotator.right * horizontalMovement;
    }

    //Returns either 1 or -1
    private float RandomSign()
    {
        if (Random.value < 0.5)
        {
            return -1;
        } 
        else
        {
            return 1;
        }
    }

    //Moves the item
    private void MoveItem()
    {
        //Helps gets the block unstuck when it gets stuck on corners (when its trying to move but isnt)
        if (rb.velocity.magnitude < .25)
        {
            Jump();
        }
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    //Updates the drag of the item based on whether it is in the air or on ground
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
