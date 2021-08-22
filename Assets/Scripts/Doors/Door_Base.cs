using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Base : MonoBehaviour {
    [SerializeField] Door_Key keyNecesaryToOpenDoor;
    [SerializeField] bool closedDoor = true;
    [SerializeField] float doorRotation = 90;
    [SerializeField] float openingSpeed = 5;

    Quaternion originalRotation = Quaternion.identity;
    Quaternion newRotation = Quaternion.identity;

    bool isDoorOpening = false;
    void Start()
    {
        originalRotation = transform.parent.rotation;
        newRotation = Quaternion.Euler(transform.parent.rotation.x, doorRotation, transform.parent.rotation.z);
    }
    void Update()
    {
        if (!isDoorOpening)
            return;

        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, newRotation, Time.deltaTime * openingSpeed);

    }
    public bool TryOpenDoor(Door_Key key) {
        if (!closedDoor)
            return false;

        if (key == keyNecesaryToOpenDoor) 
            return true;

        return false;
    }

    public void OpenDoor() {
        closedDoor = false;
        isDoorOpening = true;
    }

}
