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
    [Header("Meme")]
    [SerializeField] bool cursedDoor = false;
    Quaternion newRotation = Quaternion.identity;



    bool isDoorOpening = false;
    void Start()
    {
        newRotation = Quaternion.Euler(transform.rotation.x, doorRotation, transform.rotation.z);
    }
    void Update()
    {
        if (!isDoorOpening)
            return;
        if(!cursedDoor)
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * openingSpeed);
        else
            transform.parent.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * openingSpeed);


    }
    public bool TryOpenDoor(Door_Key key) {
        if (!closedDoor)
            return false;

        if (key == keyNecesaryToOpenDoor)
            return true;

        source.Play();

        return false;
    }

    public void OpenDoor() {
        if (keyNecesaryToOpenDoor)
            source.clip = openDoor;
        else
            source.clip = lockedDoor;

        source.Play();
        closedDoor = false;
        isDoorOpening = true;
        gameObject.layer = 0; // Solucion temporal para evitar que se muestre texto de abrir cuando la puerta esta abierta 
    }

    public bool GetClosedDoor()
    {
        return closedDoor;
    }

    public void PlayLockedDoorSound()
    {
        source.Play();
    }
    public void SetLockedSound()
    {
        source.clip = lockedDoor;
    }
    public void SetUnlockedDoorSound()
    {
        source.clip = openDoor;
    }
}
