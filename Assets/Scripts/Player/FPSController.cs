using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Forces unity to create a rigidbody into object if missing
[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance;
    [Header("Movement Data")]
    [SerializeField] float standardSpeed;
    [SerializeField] float walkingSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float sprintingSpeed;
    [Header("Jump Data")]
    [SerializeField] float jumpForce;
    [Header("Physics Data")]
    [SerializeField] float gravity;
    [SerializeField] float gravityScale;
    [Header("Input Data")]
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode crouchKey;
    [SerializeField] KeyCode walkKey;
    [SerializeField] KeyCode sprintKey;
    [Header("Audio Data")]
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip jogSound;
    [SerializeField] AudioClip runSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource loopedSoundsSource;

    AudioClip currentMovementAudio;

    public enum MovementState
    {
        jogging, walking, crouching, sprinting
    }

    Rigidbody rb;

    MovementState movementState;

    float currentSpeed;
    float timeWalking;

    bool isRunning = false;
    bool isCrouching = false;
    bool isWalking = false;
    bool isGrounded;
    bool canMove = true;
    bool isJumping = false; // tal vez lo usemos despues

    Vector2 xMovement = Vector2.zero;
    Vector2 zMovement = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentMovementAudio = jogSound;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (!canMove)
            return;


        switch (movementState)
        {
            case MovementState.jogging:
                currentSpeed = standardSpeed;
                break;
            case MovementState.walking:
                currentSpeed = walkingSpeed;
                break;
            case MovementState.sprinting:
                currentSpeed = sprintingSpeed;
                break;
            case MovementState.crouching:
                currentSpeed = crouchSpeed;
                break;
        }

        if (isGrounded)
        {
            isJumping = false;
            xMovement = new Vector2(Input.GetAxisRaw("Horizontal") * transform.right.x, Input.GetAxisRaw("Horizontal") * transform.right.z);
            zMovement = new Vector2(Input.GetAxisRaw("Vertical") * transform.forward.x, Input.GetAxisRaw("Vertical") * transform.forward.z);
        }

        velocity = (xMovement + zMovement).normalized * currentSpeed;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);

        if ((Mathf.Abs(rb.velocity.x) > 0.5f || Mathf.Abs(rb.velocity.z) > 0.5f) && !loopedSoundsSource.isPlaying && isGrounded)
        {
            Debug.Log("PITOTEEEEE");
            loopedSoundsSource.Play();
        }
        else if(((Mathf.Abs(rb.velocity.x) <= 0.5f && Mathf.Abs(rb.velocity.z) <= 0.5f) && loopedSoundsSource.isPlaying) || !isGrounded)
        {
            Debug.Log("PARA");
            loopedSoundsSource.Stop();
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            timeWalking += Time.deltaTime;
        else
            timeWalking = 0;

        SetMovementState();
        Inputs();
    }

    void FixedUpdate()
    {
        if (!isGrounded) 
        {
            Vector3 gravity = this.gravity * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    void Inputs()
    {
        if (isGrounded)
        {
            if (Input.GetKey(sprintKey))
            {
                loopedSoundsSource.clip = runSound;
                isRunning = true;
                isWalking = false;
                isCrouching = false;
            }
            else if (Input.GetKeyUp(sprintKey))
            {
                loopedSoundsSource.clip = jogSound;
                isRunning = false;
                isWalking = false;
                isCrouching = false;
            }

            if (Input.GetKeyDown(crouchKey) && !isCrouching)
            {
                loopedSoundsSource.clip = walkSound;
                isRunning = false;
                isWalking = false;
                isCrouching = true;
            }
            else if (Input.GetKeyDown(crouchKey) && isCrouching)
            {
                loopedSoundsSource.clip = jogSound;
                isRunning = false;
                isWalking = false;
                isCrouching = false;
            }

            if (Input.GetKey(walkKey))
            {
                loopedSoundsSource.clip = walkSound;
                isRunning = false;
                isWalking = true;
                isCrouching = false;
            }
            else if (Input.GetKeyUp(walkKey))
            {
                loopedSoundsSource.clip = jogSound;
                isRunning = false;
                isWalking = false;
                isCrouching = false;
            }

            if (Input.GetKeyDown(jumpKey))
            {
                sfxSource.Play();
                isRunning = false;
                isWalking = false;
                isCrouching = false;
                isJumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
        }
        
    }
    void SetMovementState()
    {
        if (!isCrouching && !isWalking && !isRunning)
        {
            movementState = MovementState.jogging;
        }
        else if (isCrouching && !isWalking && !isRunning)
        {
            movementState = MovementState.crouching;
        }
        else if (!isCrouching && isWalking && !isRunning)
        {
            movementState = MovementState.walking;
        }
        else if (!isCrouching && !isWalking && isRunning)
        {
            movementState = MovementState.sprinting;
        }
    }

    #region GETTERS
    public bool GetIsCrouching()
    {
        return isCrouching;
    }
    public bool GetIsRunning()
    {
        return isRunning;
    }
    public bool GetIsWalking()
    {
        return isWalking;
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
    #endregion

    #region Setters
    public void SetIsCrounching(bool value)
    {
        isCrouching = value;
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    #endregion
}
