using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Key : MonoBehaviour {
    [SerializeField] bool canUseKey = true;
    [SerializeField] bool canPickUp = true;
    public bool GetCanPickUp() {
        return canPickUp;
    }
    public bool GetCanUseKey() {
        return canUseKey;
    }
    public void UseKey() {
        Destroy(this.gameObject, 0.1f);
    }
    public Door_Key PickUpItem() {
        transform.position = new Vector3(0, -999, 0);
        canPickUp = false;
        return this;
    }
}
