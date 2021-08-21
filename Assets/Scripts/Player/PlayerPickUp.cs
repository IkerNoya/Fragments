using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour {
    [SerializeField] Inventory inventory;
    [SerializeField] LayerMask pickUpKey;

   public void TryPickUpObject() {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 3))
            if (pickUpKey == (pickUpKey | (1 << hit.transform.gameObject.layer))) {
                Door_Key item = hit.collider.GetComponent<Door_Key>();
                if (item != null && item.GetCanPickUp())
                    inventory.AddKeyToInventory(item.PickUpItem());
            }
    }
}
