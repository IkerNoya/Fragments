using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour {
    [SerializeField] int ammoPerBox;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player) 
                if (player.GetActualWeapon().GetMaxAmmo() != player.GetActualWeapon().GetTotalAmmo()) {
                    player.AddAmmo(ammoPerBox);
                    Destroy(this.gameObject);
                }
            
        }
    }
}
