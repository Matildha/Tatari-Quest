using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public InteractableManager interactManager;
    public Inventory inventory;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Input.GetButtonDown("Interact") && (interactManager.inRangeInteractable != null))
        {
            print("Player interacts with interactable");
            interactManager.inRangeInteractable.Interact();
        }
        else if(Input.GetButtonDown("Select"))
        {
            inventory.Browse();
        }




    }
}
