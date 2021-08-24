using System;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour {
    [SerializeField] Inventory inventory;
    [Space]
    [SerializeField] LayerMask pickUpKey;

    public static event Action<bool> ActivateText;

   public void TryPickUpObject(KeyCode key) {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 3))
            if (pickUpKey == (pickUpKey | (1 << hit.transform.gameObject.layer))) {
                ActivateText?.Invoke(true);
                Door_Key item = hit.collider.GetComponent<Door_Key>();
                if (item != null && item.GetCanPickUp() && Input.GetKeyDown(key))
                    inventory.AddKeyToInventory(item.PickUpItem());
            }
        else
            ActivateText?.Invoke(false);
    }
}
