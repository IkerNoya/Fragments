using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Door_Key : MonoBehaviour {
    [SerializeField] bool canUseKey = true;
    [SerializeField] bool canPickUp = true;
    [SerializeField] AudioSource audio;
    public UI_InventoryItem itemUI;

    public static Action<GameObject> UsedKey;

    public bool GetCanPickUp() {
        return canPickUp;
    }
    public bool GetCanUseKey() {
        return canUseKey;
    }
    public void UseKey() {
        UsedKey?.Invoke(this.gameObject);
        Destroy(this.gameObject, 0.1f);
        canUseKey = false;
    }
    public Door_Key PickUpItem() {
        transform.position = new Vector3(0, -999, 0);
        canPickUp = false;
        audio.Play();
        return this;
    }
}
