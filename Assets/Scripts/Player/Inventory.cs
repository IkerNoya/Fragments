using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] List<Door_Key> inventoryKeys;
    
    public List<Door_Key> GetInventoryKeysList() {
        return inventoryKeys;
    }
    public void AddKeyToInventory(Door_Key key) {
        inventoryKeys.Add(key);
    }
}
