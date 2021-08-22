using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] Inventory playerInventory;
    [SerializeField] PlayerPickUp playerPickUpController;

    [SerializeField] KeyCode keyToPickUpItem;
    [SerializeField] KeyCode keyToInteractWithEnviroment;
    [SerializeField] LayerMask layerDoors;

    void Update() {
        if (Input.GetKeyDown(keyToPickUpItem))
            playerPickUpController.TryPickUpObject();
        if (Input.GetKeyDown(keyToInteractWithEnviroment)) 
            TryInteractWithDoor();
    }

    void TryInteractWithDoor() {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 3))
            if (layerDoors == (layerDoors | (1 << hit.transform.gameObject.layer))) {
                Door_Base d = hit.transform.GetComponent<Door_Base>();
                if (d != null) {
                    List<Door_Key> keyListAux = playerInventory.GetInventoryKeysList();
                    for (int i = 0; i < keyListAux.Count; i++)
                        if (d.TryOpenDoor(keyListAux[i])) {
                            d.OpenDoor();
                            keyListAux[i].UseKey();
                            keyListAux.RemoveAt(i);
                            break;
                        }
                }
            }
    }
}
