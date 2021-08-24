using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour {
    [SerializeField] Transform canvasParent;
    [SerializeField] UI_InventoryItem itemBase;
    [SerializeField] List<UI_InventoryItem> itemsInUI;
    public void AddItemToUI(UI_InventoryItem item) {
        UI_InventoryItem i = Instantiate(itemBase, canvasParent);
        i.image.sprite = item.itemImage;
        i.text.text = item.itemInformation;
        i.itemGameObject = item.gameObject;
        itemsInUI.Add(i);
    }
    public void RemoveItemInUI(GameObject item) {
        for(int i=0;i<itemsInUI.Count;i++)
            if(itemsInUI[i].itemGameObject == item) {
                Destroy(itemsInUI[i].gameObject);
                itemsInUI.RemoveAt(i);
                break;
            }
    }
}