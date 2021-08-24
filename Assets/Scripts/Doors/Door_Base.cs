using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Base : MonoBehaviour {
    [SerializeField] Door_Key keyNecesaryToOpenDoor;
    [SerializeField] bool closedDoor = true;
    [SerializeField] float doorRotation = 90;
    [SerializeField] float openingSpeed = 5;
    [Header("Audio")]
    [SerializeField] AudioClip openDoor;
    [SerializeField] AudioClip lockedDoor;
    [SerializeField] AudioSource source;

    Quaternion newRotation = Quaternion.identity;

    bool isDoorOpening = false;
    void Start()
    {
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
        {
            return false;
        }

        if (key == keyNecesaryToOpenDoor)
        {
            source.clip = openDoor;
            source.Play();
            return true;
        }

        return false;
    }

    public void OpenDoor() {
        closedDoor = false;
        isDoorOpening = true;
    }

    public void PlayLockedDoorSound()
    {
        source.Play();
    }

}
