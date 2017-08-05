using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableManager : MonoBehaviour {

    public WorldManager worldMan;
    public Camera playerCamera;
    public GameObject interactionPrompt;
    public GameObject Doors;
    
    public Interactable inRangeInteractable;

    bool promptIsDisplayed;
    int rayLayerMask;


    public void Init()
    {   rayLayerMask = 1 << 9;  // Bit mask, shift index of Interactable layer (9)
        inRangeInteractable = null;
        promptIsDisplayed = false;
        ToggleInteractionPrompt(false);
    }

    public void UpdateActive(int newArea)
    {
        // Disable update calls for doors and victims in inactive world area
        foreach (Interactable obj in worldMan.worldAreas[worldMan.currentWorldArea].interactables)
        {
            if(obj.keepMeEnabled)
            {
                // Let the door know it should disable itself when it is ready
                if (obj.tag == "Door") obj.gameObject.GetComponent<Door>().toBeDisabled = true;
                continue;
            }

            if (obj.tag == "Door")
            {
                obj.gameObject.GetComponent<Door>().enabled = false;
                //print("Disabled a door");
            }
            else if (obj.tag == "Victim")
            {
                obj.gameObject.GetComponent<Victim>().enabled = false;
            }
        }

        // Enable update calls for doors and victims in current active world area
        foreach (Interactable obj in worldMan.worldAreas[newArea].interactables)
        {
            if (obj.tag == "Door")
            {
                obj.gameObject.GetComponent<Door>().enabled = true;
                obj.gameObject.GetComponent<Door>().toBeDisabled = false;
                //print("Enabled a door");
            }
            else if (obj.tag == "Victim")
            {
                obj.gameObject.GetComponent<Victim>().enabled = true;
            }
        }
    }

    /* Attempts to remove 'item' from the current world area's list of interactables. */
    public void RemoveInteractable(Interactable item)
    {
        bool success = worldMan.worldAreas[worldMan.currentWorldArea].interactables.Remove(item);
        print("Removed interactable " + success);

    }

    /* Sets the inRangeInteractable to null. */
    public void ResetInRangeInteractable()
    {
        inRangeInteractable = null;
        ToggleInteractionPrompt(false);
    }

     /* Displays an interaction prompt with a message from the current inRangeInteractable. */
    public void ToggleInteractionPrompt(bool toggle)
    {
        if (toggle && inRangeInteractable != null)  // Safety check
        {
            interactionPrompt.GetComponent<Text>().text = inRangeInteractable.PromptMessage;
            //print(inRangeInteractable.PromptMessage);
            interactionPrompt.SetActive(true);
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }

    public void ExUpdate()
    {
        if (inRangeInteractable == null)
        {
            foreach (Interactable other in worldMan.worldAreas[worldMan.currentWorldArea].interactables)
            {
                float newRange = Range(other);
                if (newRange < Mathf.Infinity)
                {
                    inRangeInteractable = other;
                    ToggleInteractionPrompt(true);
                    if (inRangeInteractable.tag == "Victim" && !worldMan.player.hasEncounteredVictim)
                    {
                        worldMan.player.FirstVictimEncounter();
                        print("First victim encounter in int man");
                    }
                    if(inRangeInteractable.tag == "Scroll" && !worldMan.player.hasFoundScroll)
                    {
                        worldMan.player.FirstScrollEncounter();
                    }
                    break;
                }
            }
        }
        //Only update status for current in range interactable
        else if(Range(inRangeInteractable) == Mathf.Infinity)
        {
            ToggleInteractionPrompt(false);
            inRangeInteractable = null;
        }
    }

  
    /* Returns the distance if other is in a certain range and in covers the center of the screen, otherwise 
       returns infinity. */
    float Range (Interactable other)
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 2, rayLayerMask) && (hit.collider == other.GetComponent<Collider>()))
        {
            return hit.distance;
        }

        return Mathf.Infinity;
    }
}