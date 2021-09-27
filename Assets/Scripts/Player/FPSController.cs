﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Forces unity to create a rigidbody into object if missing
public class FPSController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask stairMask;
    [SerializeField] float groundDistance;
    [SerializeField] float slopeDistance;
    [Header("Movement Data")]
    [SerializeField] Transform orientation;
    [SerializeField] float standardSpeed;
    [SerializeField] float sprintingSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float slopeForce;
    [Header("Jump Data")]
    [SerializeField] float jumpHeight;
    [Header("Physics Data")]
    [SerializeField] float gravity;
    [SerializeField] float gravityScale;
    [Header("Input Data")]
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode sprintKey;
    [Header("Audio Data")]
    [SerializeField] AudioClip jogSound;
    [SerializeField] AudioClip runSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource loopedSoundsSource;

    AudioClip currentMovementAudio;

    public enum MovementState
    {
        jogging, sprinting, inAir
    }

   // Rigidbody rb;
    CharacterController controller;

    MovementState movementState;

    float currentSpeed;
    float lastSpeed;
    float startTime;
    float timeWalking;
    float accelerationTime = 0;
    float verticalInput;
    float horizontalInput;

    bool isRunning = false;
    bool isGrounded;
    bool isInStair;
    bool canMove = true;
    bool isJumping = false; // tal vez lo usemos despues
    bool setStartTimeAndSpeed = false;

    Vector3 forwardMovement = Vector3.zero;
    Vector3 rightMovement = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    Vector3 movement = Vector3.zero;
    RaycastHit slopeHit;


    bool gamePaused = false;

    void Awake()
    {
        InitialCutscene.initialCutscene += SetCanMove;
        InitialCutscene.endInitialCutscene += SetCanMove;
        PauseController.SetPause += SetGamePause;
    }


    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentMovementAudio = jogSound;
    }

    private void OnDisable()
    {
        PauseController.SetPause -= SetGamePause;
        InitialCutscene.initialCutscene -= SetCanMove;
        InitialCutscene.endInitialCutscene -= SetCanMove;
    }

    private void OnDestroy()
    {
        PauseController.SetPause -= SetGamePause;
    }

    void Update()
    {
        if (gamePaused)
        {
            loopedSoundsSource.Stop();
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isInStair = Physics.CheckSphere(groundCheck.position, groundDistance, stairMask);


        if (!canMove)
            return;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2;

        if ((Mathf.Abs(controller.velocity.x) > 0.5f || Mathf.Abs(controller.velocity.z) > 0.5f) && !loopedSoundsSource.isPlaying && isGrounded)
        {
            loopedSoundsSource.Play();
        }
        else if (((Mathf.Abs(controller.velocity.x) <= 0.5f && Mathf.Abs(controller.velocity.z) <= 0.5f) && loopedSoundsSource.isPlaying) || !isGrounded)
        {
            loopedSoundsSource.Stop();
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            timeWalking += Time.deltaTime;
        else
            timeWalking = 0;





        SetMovementState();
        Inputs();
        Move();
    }

    void Move()
    {
        if (isGrounded || isInStair)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if(velocity.y <= 0) isJumping = false;
        }

        movement = transform.right * horizontalInput + transform.forward * verticalInput;
        SetSpeedFromMovementState();


        if (currentSpeed > sprintingSpeed) currentSpeed = sprintingSpeed;
        if ((isGrounded || isInStair) && ((Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.z) > 0) || (Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.z) > 0)))
            accelerationTime += Time.deltaTime;
        else if ((isGrounded || isInStair) && !((Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.z) > 0) || (Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.z) > 0)))
        {
            accelerationTime = 0;
            currentSpeed = 0;
            lastSpeed = 0;
            startTime = 0;
        }

        if (currentSpeed < 0.5f)
        {
            lastSpeed = 0;
            startTime = 0;
        }

        controller.Move(movement.normalized * Mathf.Abs(currentSpeed) * Time.deltaTime);

        if (!isJumping && (verticalInput != 0 || horizontalInput != 0) && OnSlope())
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);

        velocity.y += (gravity * gravityScale) * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Inputs()
    {
        if (isGrounded || isInStair)
        {

            //Check if i just pressed the button once to set physics values
            if (Input.GetKeyDown(sprintKey))
            {
                loopedSoundsSource.clip = runSound;
                setStartTimeAndSpeed = false;
            }
            if (Input.GetKey(sprintKey))
            {
                isRunning = true;
            }
            else if (Input.GetKeyUp(sprintKey))
            {
                setStartTimeAndSpeed = false;
                loopedSoundsSource.clip = jogSound;
                isRunning = false;
            }

            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
            }
        }

    }

    void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            sfxSource.Play();
            isRunning = false;
            isJumping = true;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * (gravity * gravityScale)); // result = sqrt(h * -2 * g)
        }
    }



    void SetSpeedFromMovementState()
    {

        switch (movementState)
        {
            case MovementState.jogging:
                    currentSpeed = standardSpeed;
                break;

            case MovementState.sprinting:
                    currentSpeed = sprintingSpeed;
                break;
        }
    }

    void SetMovementState()
    {
        if (!isRunning)
        {
            movementState = MovementState.jogging;
        }
        else if (isRunning)
        {
            movementState = MovementState.sprinting;
        }
        else if (!isGrounded && !isInStair)
        {
            movementState = MovementState.inAir;
        }
    }

    bool OnSlope()
    {
        if (isJumping) return false;

        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (controller.height * 0.5f) * slopeDistance))
        {
            if (slopeHit.normal != Vector3.up)
                return true;
        }

        return false;
    }

    public bool GetIsRunning()
    {
        return isRunning;
    }
    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    public bool GetCanMove()
    {
        return canMove;
    }
    public float GetTimeWalking()
    {
        return timeWalking;
    }
    public MovementState GetMovementState()
    {
        return movementState;
    }
    public bool GetPauseState()
    {
        return gamePaused;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    void SetGamePause(bool value)
    {
        gamePaused = value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
