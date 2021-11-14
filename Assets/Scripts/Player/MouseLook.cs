using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float verticalSensitivity;
    [SerializeField] float horizontalSensitivity;
    [SerializeField] Transform body;


    float xRotation = 0;
    float verticalRecoil = 0;
    float horizontalRecoil = 0;
    float rotateZ = 0;

    bool canMove = true;
    bool pause = false;
    bool alive = true;

    void Awake()
    {
        InitialCutscene.initialCutscene += SetMoveBool;
        Console.ConsolePause += SetGamePause;
        InitialCutscene.endInitialCutscene += SetMoveBool;
        PlayerController.PlayerDead += PlayerDead;
    }

    void OnDisable()
    {
        InitialCutscene.initialCutscene -= SetMoveBool;
        Console.ConsolePause -= SetGamePause;
        InitialCutscene.endInitialCutscene -= SetMoveBool;
        PlayerController.PlayerDead -= PlayerDead;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!alive)
            return;
        if (!canMove)
            return;
        if (pause)
            return;

        float mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime;

        mouseX += horizontalRecoil;
        mouseY += verticalRecoil;
        
        //restart recoil after 1 frame
        verticalRecoil = 0;
        horizontalRecoil = 0;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, transform.localRotation.eulerAngles.z);

        if (body != null)
            body.Rotate(Vector3.up * mouseX);   
    }
    public void AddRecoil(float vertical, float horizontal)
    {
        verticalRecoil += vertical;
        horizontalRecoil += horizontal;
    }

    public float GetVerticalSensitivity()
    {
        return verticalSensitivity;
    }
    public float GetHorizontalSensitivity()
    {
        return horizontalSensitivity;
    }

    public void SetVerticalSensitivity(float value)
    {
        verticalSensitivity = value;
    }
    public void SetHorizontalSensitivity(float value)
    {
        horizontalSensitivity = value;
    }

    void SetMoveBool(bool value)
    {
        canMove = value;
    }
    void SetGamePause(bool value)
    {
        pause = value;
    }

    void PlayerDead() {
        alive = false;
    }
}
