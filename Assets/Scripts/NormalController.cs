using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalController : MonoBehaviour
{
    //Movement
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    private Vector3 moveDirection;
    private float horizontalMovement;
    private float verticalMovement;
    public float groundDrag = 2f;

    //Jump Movement
    public Transform groundCheck;
    public float airDrag = 0.5f;
    public float jumpPower;
    public float airMultiplier = .2f;
    bool isGrounded;
    public float gravity = -9.81f;

    //sound stuff
    public AudioSource audioSource;
    public AudioClip footSteps;
    public AudioClip jumpSound;
    public AudioClip jumpSound2;
    public AudioClip jumpSound3;
    public AudioClip jumpSound4;
    public AudioClip jumpSound5;

    public ArrayList soundArray = new ArrayList();

    public Rigidbody rb;

    //Layer masks
    public LayerMask groundMask;


    private void Start()
    {
        rb.freezeRotation = true;
        Physics.gravity = Vector3.up * gravity;

        soundArray.Add(jumpSound);
        soundArray.Add(jumpSound2);
        soundArray.Add(jumpSound3);
        soundArray.Add(jumpSound4);
        soundArray.Add(jumpSound5);
    }
    private void Update()
    {
        int num = new System.Random().Next(0, soundArray.Count);

        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, groundMask);

        PlayerInput();
        ControlDrag();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            audioSource.Stop();
            audioSource.PlayOneShot((AudioClip)soundArray[num]);
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    //Reads in players input
    private void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    //Moves the player, speed of movement is based on whether player is in the air or on the ground
    private void MovePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            if (moveDirection.magnitude > 0)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(footSteps);
                }
            }
            else
            {
                audioSource.Stop();
            }
        }
        else
        {
            // audioSource.Stop();
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

    //Player Jump
    private void Jump()
    {
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }
}
