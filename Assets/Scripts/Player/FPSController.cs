using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Forces unity to create a rigidbody into object if missing
[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance;
    [Space]
    [SerializeField] float standardSpeed;
    [SerializeField] float walkingSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float sprintingSpeed;
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

    Vector2 xMovement = Vector2.zero;
    Vector2 zMovement = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log(isGrounded);
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
        
        xMovement = new Vector2(Input.GetAxisRaw("Horizontal") * transform.right.x, Input.GetAxisRaw("Horizontal") * transform.right.z);
        zMovement = new Vector2(Input.GetAxisRaw("Vertical") * transform.forward.x, Input.GetAxisRaw("Vertical") * transform.forward.z);

        velocity = (xMovement + zMovement).normalized * currentSpeed;

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            timeWalking += Time.deltaTime;
        else
            timeWalking = 0;

        SetMovementState();
        Inputs();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
    }

    void Inputs()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            isWalking = false;
            isCrouching = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            isWalking = false;
            isCrouching = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && !isCrouching)
        {
            isRunning = false;
            isWalking = false;
            isCrouching = true;
        }
        else if(Input.GetKeyDown(KeyCode.C) && isCrouching)
        {
            isRunning = false;
            isWalking = false;
            isCrouching = false;
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isRunning = false;
            isWalking = true;
            isCrouching = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            isRunning = false;
            isWalking = false;
            isCrouching = false;
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
