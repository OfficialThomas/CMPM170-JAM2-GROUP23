using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RigidbodyController : MonoBehaviour
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

    //Jump Movement
    public float airDrag = 1f;
    public float jumpPower = 10;
    public float airMultiplier = .2f;
    private bool isGrounded;
    public float gravity = -20f;

    //SFX
    //public AudioSource jumpSound;
    //public AudioSource footSteps;
    public AudioSource audioSource;
    public AudioClip footSteps;
    public AudioClip jumpSound;
    public AudioClip jumpSound2;
    public AudioClip jumpSound3;
    public AudioClip jumpSound4;
    public AudioClip jumpSound5;
    private AudioClip currentClip;

    public ArrayList soundArray = new ArrayList();
    // AudioClip changeClip;
    // System.Random r = new System.Random();

    public Rigidbody rb;

    //Layer masks
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask planetLayer;

    //Player rotation
    public Transform core;
    private Vector3 lastSide, currentSide;
    public Transform rotator;
    public CameraController camControl;
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
        Physics.gravity = lastSide * gravity;
        currentRotation = 0;
        camCorrection = 0;
        soundArray.Add(jumpSound);
        soundArray.Add(jumpSound2);
        soundArray.Add(jumpSound3);
        soundArray.Add(jumpSound4);
        soundArray.Add(jumpSound5);

    }

    void Update()
    {
        int num = new System.Random().Next(0, soundArray.Count);

        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, groundMask);
        if (planetGravity)
        {
            GravityCheck();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            audioSource.Stop();
            audioSource.PlayOneShot((AudioClip)soundArray[num]);
            currentClip = (AudioClip)soundArray[num];
            Jump();
        }
        PlayerInput();
        ControlDrag();      
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
        //checks if player needs to rotate to new side
        if (currentSide != hit.normal)
        {
            currentSide = hit.normal;
            Physics.gravity = hit.normal * gravity;
            currentRotation = 0;
            camCorrection = 0;
            camFlipped = false;
        }
        //Checks if the player needs to rotate more
        if (transform.up != hit.normal)
        {
            Rotate();
        }
        else
        {
            lastSide = hit.normal;         
        }
    }

    //Uses lerp to gradually rotate player when moving to a new side
    private void Rotate()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        transform.up = Vector3.Lerp(lastSide, hit.normal, currentRotation);
        EditModifier();         
    }

    //Sets the camController modifier which corrects the rotation of the player
    //when going between certain sides
    private void EditModifier()
    {
        //front to left and right to front
        if ((lastSide == Vector3.forward && hit.normal == Vector3.left) || (lastSide == Vector3.right && hit.normal == Vector3.forward))
        {
            camControl.modifier = Mathf.Lerp(0, -90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, -90, currentRotation);
        }
        //left to front and front to right
        else if ((lastSide == Vector3.left && hit.normal == Vector3.forward) || (lastSide == Vector3.forward && hit.normal == Vector3.right))
        {
            camControl.modifier = Mathf.Lerp(0, 90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, 90, currentRotation);
        }
        //back to left and right to back
        if ((lastSide == Vector3.back && hit.normal == Vector3.left) || (lastSide == Vector3.right && hit.normal == Vector3.back))
        {
            camControl.modifier = Mathf.Lerp(0, 90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, 90, currentRotation);
        }
        //left to back and back to right
        else if ((lastSide == Vector3.left && hit.normal == Vector3.back) || (lastSide == Vector3.back && hit.normal == Vector3.right))
        {
            camControl.modifier = Mathf.Lerp(0, -90, currentRotation) - camCorrection;
            camCorrection = Mathf.Lerp(0, -90, currentRotation);
        }
        //bottom to left and bottom to right, flips camera 180 at the start of the rotation
        if ((lastSide == Vector3.down && hit.normal == Vector3.left) || (lastSide == Vector3.down && hit.normal == Vector3.right))
        {
            if (!camFlipped)
            {
                camFlipped = true;
                camControl.modifier = 180;
            }
        }
        //right to bottom and left to bottom, flips camera 180 at the veery end of the rotaiton
        else if ((lastSide == Vector3.right && hit.normal == Vector3.down) || (lastSide == Vector3.left && hit.normal == Vector3.down))
        {
            if (!camFlipped && (transform.rotation.x == 1))
            {
                camFlipped = true;
                camControl.modifier = 180;
            }
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
            //Plays footstep noise when player is moving
            if (moveDirection.magnitude > 0)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(footSteps);
                    currentClip = footSteps;
                }
            }
            else
            {
                //Stops the footsteps audio when player isnt moving
                if (currentClip == footSteps)
                {
                    audioSource.Stop();
                }
                
            }
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
