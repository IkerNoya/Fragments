using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Forces unity to create a rigidbody into object if missing
[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour {
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask stairMask;
    [SerializeField] float groundDistance;
    [Header("Movement Data")]
    [SerializeField] float standardSpeed;
    [SerializeField] float walkingSpeed;
    [SerializeField] float sprintingSpeed;
    [SerializeField] float acceleration;
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

    public enum MovementState {
        jogging, walking, sprinting, inAir
    }

    Rigidbody rb;

    MovementState movementState;

    float currentSpeed;
    float lastSpeed;
    float startTime;
    float timeWalking;
    float accelerationTime = 0;

    bool isRunning = false;
    bool isWalking = false;
    bool isGrounded;
    bool isInStair;
    bool canMove = true;
    bool isJumping = false; // tal vez lo usemos despues
    bool setStartTimeAndSpeed = false;

    Vector2 xMovement = Vector2.zero;
    Vector2 zMovement = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    bool gamePaused = false;

    void Awake()
    {
        InitialCutscene.initialCutscene += SetCanMove;    
        InitialCutscene.endInitialCutscene += SetCanMove;    
        PauseController.SetPause += SetGamePause;
    }


    void Start() {
        rb = GetComponent<Rigidbody>();
        currentMovementAudio = jogSound;
    }

    private void OnDisable() {
        PauseController.SetPause -= SetGamePause;
        InitialCutscene.initialCutscene -= SetCanMove;
        InitialCutscene.endInitialCutscene -= SetCanMove;
    }

    private void OnDestroy() {
        PauseController.SetPause -= SetGamePause;
    }

    void Update() {
        if (gamePaused) {
            loopedSoundsSource.Stop();
            return;
        }


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isInStair = Physics.CheckSphere(groundCheck.position, groundDistance, stairMask);

        if (!canMove)
            return;

        SetSpeedFromMovementState();

        if (isGrounded)
        {
            isJumping = false;
            xMovement = new Vector2(Input.GetAxisRaw("Horizontal") * transform.right.x, Input.GetAxisRaw("Horizontal") * transform.right.z);
            zMovement = new Vector2(Input.GetAxisRaw("Vertical") * transform.forward.x, Input.GetAxisRaw("Vertical") * transform.forward.z);
        }

        if(isGrounded && ((Mathf.Abs(xMovement.x) > 0  || Mathf.Abs(xMovement.y) > 0) || (Mathf.Abs(zMovement.x) > 0 || Mathf.Abs(zMovement.y) > 0)))
            accelerationTime += Time.deltaTime;
        else if(isGrounded && !((Mathf.Abs(xMovement.x) > 0 || Mathf.Abs(xMovement.y) > 0) || (Mathf.Abs(zMovement.x) > 0 || Mathf.Abs(zMovement.y) > 0)))
        {
            accelerationTime = 0;
            currentSpeed = 0;
            lastSpeed = 0;
            startTime = 0;
        }

        if(currentSpeed < 0.5f)
        {
            lastSpeed = 0;
            startTime = 0;
        }
        
        velocity = (xMovement + zMovement).normalized * currentSpeed;


        if ((Mathf.Abs(rb.velocity.x) > 0.5f || Mathf.Abs(rb.velocity.z) > 0.5f) && !loopedSoundsSource.isPlaying && isGrounded) {
            loopedSoundsSource.Play();
        }
        else if (((Mathf.Abs(rb.velocity.x) <= 0.5f && Mathf.Abs(rb.velocity.z) <= 0.5f) && loopedSoundsSource.isPlaying) || !isGrounded) {
            loopedSoundsSource.Stop();
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            timeWalking += Time.deltaTime;
        else
            timeWalking = 0;

        SetMovementState();
        Inputs();
    }

    void FixedUpdate() {
        if (gamePaused)
            return;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);

        if (!isGrounded) {
            Vector3 gravity = this.gravity * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    void Inputs() {
        if (isGrounded) {

            //Check if i just pressed the button once to set physics values
            if (Input.GetKeyDown(sprintKey))
            {
                loopedSoundsSource.clip = runSound;
                setStartTimeAndSpeed = false;
            }
            if (Input.GetKeyDown(walkKey))
            {
                loopedSoundsSource.clip = walkSound;
                setStartTimeAndSpeed = false;
            }


            if (Input.GetKey(sprintKey)) {
                isRunning = true;
                isWalking = false;
            }
            else if (Input.GetKeyUp(sprintKey)) {
                setStartTimeAndSpeed = false;
                loopedSoundsSource.clip = jogSound;
                isRunning = false;
                isWalking = false;
            }

            if (Input.GetKey(walkKey)) {
                isRunning = false;
                isWalking = true;
            }
            else if (Input.GetKeyUp(walkKey)) {
                setStartTimeAndSpeed = false;
                loopedSoundsSource.clip = jogSound;
                isRunning = false;
                isWalking = false;
            }

            if (Input.GetKeyDown(jumpKey)) {
                sfxSource.Play();
                isRunning = false;
                isWalking = false;
                isJumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
        }

    }

    void SetSpeedFromMovementState()
    {
        //MRUV / UAM Formula = vf = am * (tf - ti) + vi

        switch (movementState)
        {
            case MovementState.jogging:
                if (!setStartTimeAndSpeed)
                {
                    startTime = accelerationTime;
                    lastSpeed = currentSpeed;
                    setStartTimeAndSpeed = true;
                }

                if (currentSpeed < standardSpeed - 0.3f)
                    currentSpeed = acceleration * (accelerationTime - startTime) + lastSpeed;
                else if (currentSpeed > standardSpeed + 0.5f)
                    currentSpeed = -acceleration * (accelerationTime - startTime) + lastSpeed;
                else
                    currentSpeed = standardSpeed;
                break;

            case MovementState.walking:
                if (!setStartTimeAndSpeed)
                {
                    startTime = accelerationTime;
                    lastSpeed = currentSpeed;
                    setStartTimeAndSpeed = true;
                }

                if (currentSpeed < walkingSpeed - 0.3f)
                    currentSpeed = acceleration * (accelerationTime - startTime) + lastSpeed;
                else if (currentSpeed > walkingSpeed + 0.5f)
                    currentSpeed = -acceleration * (accelerationTime - startTime) + lastSpeed;
                else
                    currentSpeed = walkingSpeed;
                break;

            case MovementState.sprinting:
                if (!setStartTimeAndSpeed)
                {
                    startTime = accelerationTime;
                    lastSpeed = currentSpeed;
                    setStartTimeAndSpeed = true;
                }

                if (currentSpeed < sprintingSpeed - 0.3f)
                    currentSpeed = acceleration * (accelerationTime - startTime) + lastSpeed;
                else if (currentSpeed > sprintingSpeed + 0.5f)
                    currentSpeed = -acceleration * (accelerationTime - startTime) + lastSpeed;
                else
                    currentSpeed = sprintingSpeed;
                break;
        }
    }

    void SetMovementState() {
        if (!isWalking && !isRunning) {
            movementState = MovementState.jogging;
        }
        else if (isWalking && !isRunning) {
            movementState = MovementState.walking;
        }
        else if (!isWalking && isRunning) {
            movementState = MovementState.sprinting;
        }
        else if(!isGrounded && !isInStair){
            movementState = MovementState.inAir;
        }
    }

    public bool GetIsRunning() {
        return isRunning;
    }
    public bool GetIsWalking() {
        return isWalking;
    }
    public bool GetIsGrounded() {
        return isGrounded;
    }
    public bool GetCanMove() {
        return canMove;
    }
    public float GetTimeWalking() {
        return timeWalking;
    }
    public MovementState GetMovementState() {
        return movementState;
    }
    public bool GetPauseState()
    {
        return gamePaused;
    }

    public void SetCanMove(bool value) {
        canMove = value;
    }
    void SetGamePause(bool value) {
        gamePaused = value;
    }
}
