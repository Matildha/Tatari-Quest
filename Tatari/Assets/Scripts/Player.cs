using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerController playerController;
    public InteractableManager intMan;
    public Inventory inventory;
    public Lantern lantern;

    const float MAX_FEAR = 100;
    float fear;

    int nrRescuedVictims;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        fear = 0f;
        nrRescuedVictims = 0;
    }

    /* Increases or decreases the player's fear. Use negative value to decrement. */
    public void ChangeFear(float delta)
    {
        fear += delta;
        if(delta > 5 || delta < -5) print("Player fear: " + fear);
        if(fear >= MAX_FEAR)
        {
            print("GAMEEEE OOOOOVEEEERRRRR!!!! :O");
        }
    }

    public void IncreaseNrRescues()
    {
        nrRescuedVictims++;
        if(nrRescuedVictims == VictimFactory.MAX_VICTIMS)
        {
            print("You rescued all victims!!! Yokatta!!");
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

            // Detect if reading to victim right when stopped reading
            if(!inventory.isReading && intMan.inRangeInteractable != null && intMan.inRangeInteractable.tag == "Victim")
            {
                ((Victim)intMan.inRangeInteractable).Rescue(inventory.GetScrollContent());
            }

            playerController.unableToMove = inventory.isReading;
        }
        else if(Input.GetButtonDown("Interact") && (intMan.inRangeInteractable != null))
        {
            intMan.inRangeInteractable.Interact();
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