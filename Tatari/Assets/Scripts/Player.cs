using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public PlayerController playerController;
    public InteractableManager intMan;
    public Inventory inventory;
    public FearMeter fearMeter;
    public GameObject victimStatIndicator;
    const string VICTIM_INDICATOR_TEXT = "Victims saved: ";
    public Lantern lantern;

    int nrRescuedVictims;


	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        nrRescuedVictims = 0;
    }

    public void IncreaseNrRescues()
    {
        nrRescuedVictims++;
        victimStatIndicator.GetComponent<Text>().text = VICTIM_INDICATOR_TEXT + nrRescuedVictims;
        if(nrRescuedVictims == VictimFactory.MAX_VICTIMS)
        {
            print("You rescued all victims!!! Yokatta!!");
        }
    }

    void Update() {

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
            intMan.ToggleInteractionPrompt(false);
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