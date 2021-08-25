using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] List<Door_Key> inventoryKeys;
    [SerializeField] UI_Inventory inventoryUI;

    private void Start() {
        Door_Key.UsedKey += RemoveKeyInInventory;
    }

    private void OnDisable() {
        Door_Key.UsedKey -= RemoveKeyInInventory;
    }

    private void OnDestroy() {
        Door_Key.UsedKey -= RemoveKeyInInventory;
    }

    public List<Door_Key> GetInventoryKeysList() {
        return inventoryKeys;
    }
    public void AddKeyToInventory(Door_Key key) {
        inventoryKeys.Add(key);
        inventoryUI.AddItemToUI(key.itemUI);
    }
    void RemoveKeyInInventory(GameObject item) {
        inventoryUI.RemoveItemInUI(item);
    }
}
