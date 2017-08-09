using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * InteractableManager is in charge of the interactive system that enables the player to 
 * interact with objects in the game world. InteractableManager makes use of objects
 * inherited from Interactable. 
 * 
 * InteractableManager continuously searches the Interactable:s of the current world area
 * and tests them with a raycast for visibilty and proximity to the player. When in range
 * an interact prompt message from the specific Interactable is displayed. 
 * 
 * InteractableManager contains functionality to disable and enable Interactable:s given
 * a new world area and to permanently remove an Interactable from the current world area's 
 * list of Interactable:s. 
*/ 

public class InteractableManager : MonoBehaviour {

    public WorldManager worldMan;
    public Camera playerCamera;  // used when raycasting to Interactable 
    public GameObject interactionPrompt;
    
    public Interactable inRangeInteractable;  // the Interactable the player currently can interact with

    public const int INTERACTABLE_LAYER = 9;

    bool promptIsDisplayed;
    int rayLayerMask;


    public void Init()
    {   rayLayerMask = 1 << 9;  // Bit mask, shift index of Interactable layer (9)
        inRangeInteractable = null;
        promptIsDisplayed = false;
        ToggleInteractionPrompt(false);
    }

    /* Disables Interactable:s of the current area and enables Interactable:s of WorldArea newArea. 
     Interactables that have their "keepMeEnabled" flag set is not disabled. Door:s as a special case
     gets their "toBeDisabled" flag set instead of being disabled when they have "keepMeEnabled" set. */
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
    public bool RemoveInteractable(Interactable item)
    {
        bool success = worldMan.worldAreas[worldMan.currentWorldArea].interactables.Remove(item);
        print("Removed interactable " + success);
        return success;
    }

    /* Sets inRangeInteractable to null. */
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

    /* Loops through the current WorldArea's list of Interactable:s to find a valid inRangeInteractable. 
     If inRangeInteractable is set to an object, this object is the only one updated. */
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

  
    /* Returns the distance if other is in a certain range and covers the center of the screen, otherwise 
       returns infinity. */
    private float Range (Interactable other)
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
