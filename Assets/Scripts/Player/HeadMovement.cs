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

    float hBobFrequency;
    float hBobVerticalAmplitude;
    float axisDifference = 0.001f;

    void Start()
    {
        player = GetComponent<FPSController>();
    }
    void Update()
    {
        if (!player.GetCanMove())
            return;

    }
    void LateUpdate()
    {
        if (!player.GetCanMove())
            return;
        //Crouch();
        HeadBobbing();
    }

    void HeadBobbing()
    {
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


}

