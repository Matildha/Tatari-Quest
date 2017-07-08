using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerController playerController;
    public InteractableManager interactManager;
    public Inventory inventory;
    public Lantern lantern;

    const float MAX_FEAR = 100;
    float fear;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        fear = 0f;
    }

    /* Increases or decreases the player's fear. Use negative value to decrement. */
    public void ChangeFear(float delta)
    {
        fear += delta;
        print("Player fear: " + fear);
        if(fear >= MAX_FEAR)
        {
            print("GAMEEEE OOOOOVEEEERRRRR!!!! :O");
        }
    }

    void Update () {

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(inventory.isReading || (Input.GetButtonDown("Read") && lantern.isLit))
        {
            inventory.ReadSelected();
            playerController.unableToMove = inventory.isReading;
        }
        else if(Input.GetButtonDown("Interact") && (interactManager.inRangeInteractable != null))
        {
            interactManager.inRangeInteractable.Interact();
        }
        else if(Input.GetButtonDown("Select"))
        {
            inventory.Browse();
        }      
        else if (Input.GetKeyDown("space"))
        {
            if(lantern.isLit || inventory.ExpandMatch())
                lantern.ToggleLight();
        }

    }
}