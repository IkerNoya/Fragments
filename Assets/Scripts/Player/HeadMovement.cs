﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform cam;
    [SerializeField] Transform minimumHeadHeight;
    [Space]
    [SerializeField] float defaultColliderHeight;
    [SerializeField] float crouchColliderHeight;
    [Space]
    [SerializeField] float headBobbingIntensity;
    [SerializeField] float walkingHeadBobFrequency;
    [SerializeField] float joggingHeadBobFrequency;
    [SerializeField] float sprintingHeadBobFrequency;
    [SerializeField] float crouchingHeadVerticalAmplitude;
    [SerializeField] float standingHeadBobVerticalAmplitude;
    [SerializeField] float crouchingSpeed;
    [Space]
    [SerializeField] MouseLook mouseLook;

    CapsuleCollider CapsuleColl;
    FPSController player;

    float hBobFrequency;
    float hBobVerticalAmplitude;
    float axisDifference = 0.001f;

    void Start()
    {
        player = GetComponent<FPSController>();
        CapsuleColl = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if (!player.GetIsCrouching())
        {
            CapsuleColl.height = defaultColliderHeight;
        }
        else
        {
            CapsuleColl.height = crouchColliderHeight;
        }
    }
    void LateUpdate()
    {
        //Crouch();
        HeadBobbing();
    }
    void Crouch()
    {
        float height = cam.position.y;
        if (player.GetIsCrouching() && height > minimumHeadHeight.position.y + 0.5f)
        {
            height -= Time.deltaTime * crouchingSpeed;
        }

        cam.position = new Vector3(cam.position.x, height, cam.position.z);
    }

    void HeadBobbing()
    {
        Vector3 newHeadPosition;
        if (!player.GetIsCrouching())
            newHeadPosition = head.position + CalculateHeadBobOffset(player.GetTimeWalking());
        else
            newHeadPosition = minimumHeadHeight.position + CalculateHeadBobOffset(player.GetTimeWalking());

        switch (player.GetMovementState())
        {
            case FPSController.MovementState.walking:
                hBobFrequency = walkingHeadBobFrequency;
                hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
                break;
            case FPSController.MovementState.jogging:
                hBobFrequency = joggingHeadBobFrequency;
                hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
                break;
            case FPSController.MovementState.sprinting:
                hBobFrequency = sprintingHeadBobFrequency;
                hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
                break;
            case FPSController.MovementState.crouching:
                hBobFrequency = joggingHeadBobFrequency;
                hBobVerticalAmplitude = crouchingHeadVerticalAmplitude;
                break;
        }
    }
    Vector3 CalculateHeadBobOffset(float value)
    {
        float movement;
        Vector3 offset = Vector3.zero;
        if (value > 0)
        {
            movement = Mathf.Sin(value * hBobFrequency * 2) * hBobVerticalAmplitude;
            if (!player.GetIsCrouching())
                offset = head.transform.up * movement;
            else
                offset = minimumHeadHeight.transform.up * movement;
        }
        return offset;
    }


}
