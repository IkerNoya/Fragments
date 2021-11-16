using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour {
    [SerializeField] Transform canvasParent;
    [SerializeField] UI_InventoryItem itemBase;
    [SerializeField] List<UI_InventoryItem> itemsInUI;
    [SerializeField] Image Key;
    [SerializeField] Sprite NoKey;
    [SerializeField] Sprite HaveKey;
    void Start()
    {
        Key.sprite = NoKey;
    }
    public void AddItemToUI(UI_InventoryItem item) {
        Key.sprite = HaveKey;
    }
    public void RemoveItemInUI(GameObject item) {
        Key.sprite = NoKey;
    }
}