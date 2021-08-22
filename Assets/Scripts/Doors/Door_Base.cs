using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Base : MonoBehaviour {
    [SerializeField] Door_Key keyNecesaryToOpenDoor;
    [SerializeField] bool closedDoor = true;
    public bool TryOpenDoor(Door_Key key) {
        if (!closedDoor)
            return false;

        if (key == keyNecesaryToOpenDoor) 
            return true;

        return false;
    }

    public void OpenDoor() {
        closedDoor = false;
        transform.position += Vector3.up;
    }

}
