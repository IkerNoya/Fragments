using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    private Camera mainCamera;


    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 camPos = mainCamera.transform.position;
        camPos.y = 0;
        Vector3 pos = transform.position;
        pos.y = 0;

        Vector3 dirToCamera = (camPos - pos).normalized;
        float angleToCam = GetAngle(dirToCamera);
        transform.eulerAngles = new Vector3(0f, -angleToCam + 90 + 180, 0f);
    }

    float GetAngle(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
