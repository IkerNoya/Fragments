using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform cam;
    [SerializeField] Transform minimumHeadHeight;
    [Space]
    [SerializeField] float defaultColliderHeight;
    [Space]
    [SerializeField] float headBobbingIntensity;
    [SerializeField] float joggingHeadBobFrequency;
    [SerializeField] float sprintingHeadBobFrequency;
    [SerializeField] float standingHeadBobVerticalAmplitude;
    [SerializeField] float stairsHeadBobAmplitude;
    [Space]
    [SerializeField] MouseLook mouseLook;

    FPSController player;
    Animator anim;
    bool alive = true;

    float hBobFrequency;
    float hBobVerticalAmplitude;
    float axisDifference = 0.001f;
    bool landing = false;


    private void Awake()
    {
        FPSController.Land += Land;
        HeadAnimationValues.IsLanding += SetLandingBool;
        PlayerController.PlayerDead += PlayerDead;
    }
    void Start()
    {
        player = GetComponent<FPSController>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (!alive)
            return;

        if (!player.GetCanMove())
            return;

    }
    void LateUpdate()
    {
        if (!alive)
            return;

        if (!player.GetCanMove())
            return;
        //Crouch();
        HeadBobbing();
    }

    void HeadBobbing()
    {
        if (landing)
            return;
        

        Vector3 newHeadPosition;
        newHeadPosition = head.position + CalculateHeadBobOffset(player.GetTimeWalking());

        switch (player.GetMovementState())
        {
            case FPSController.MovementState.jogging:
                if (player.GetIsInStair())
                {
                    hBobFrequency = joggingHeadBobFrequency;
                    hBobVerticalAmplitude = stairsHeadBobAmplitude;
                }
                else
                {
                    hBobFrequency = joggingHeadBobFrequency;
                    hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
                }
                break;
            case FPSController.MovementState.sprinting:
                if (player.GetIsInStair())
                {
                    hBobFrequency = sprintingHeadBobFrequency;
                    hBobVerticalAmplitude = stairsHeadBobAmplitude;
                }
                else
                {
                    hBobFrequency = sprintingHeadBobFrequency;
                    hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
                }
                break;
        }
        if (player.GetIsGrounded())
        {
             cam.position = Vector3.Lerp(cam.position, newHeadPosition, headBobbingIntensity);
             if ((cam.position - newHeadPosition).magnitude <= axisDifference)
             {
                 cam.position = newHeadPosition;
             }
        }
    }
    Vector3 CalculateHeadBobOffset(float value)
    {
        float movement;
        Vector3 offset = Vector3.zero;
        if (value > 0)
        {
            movement = Mathf.Sin(value * hBobFrequency * 2) * hBobVerticalAmplitude;
            offset = head.transform.up * movement;
        }

        return offset;
    }

    void Land(float value)
    {
        anim.SetTrigger("Land");
        landing = true;

    }
    void SetLandingBool(bool value)
    {
        landing = value;
    }

    private void OnDisable()
    {
        FPSController.Land -= Land;
        HeadAnimationValues.IsLanding -= SetLandingBool;
        PlayerController.PlayerDead -= PlayerDead;
    }
    private void OnDestroy()
    {
        FPSController.Land -= Land;
        PlayerController.PlayerDead -= PlayerDead;
        HeadAnimationValues.IsLanding -= SetLandingBool;
    }

    void PlayerDead() {
        alive = false;
        anim.SetTrigger("Death");
    }

}

