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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
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
}