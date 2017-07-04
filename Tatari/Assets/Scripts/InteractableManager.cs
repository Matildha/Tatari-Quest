using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableManager : MonoBehaviour {

    public Camera playerCamera;
    public GameObject interactionPrompt;
    
    public Interactable inRangeInteractable;
    LinkedList<Interactable> interactables;

    bool promptIsDisplayed;
    int rayLayerMask;


    public void Init()
    {   rayLayerMask = 1 << 9;  // Bit mask, shift index of Interactable layer (9)
        inRangeInteractable = null;
        promptIsDisplayed = false;
        interactables = new LinkedList<Interactable>();
    }

    /* Asynchronsouly adds a new interactable item to the InteractableManager update list.
    It is not safe to call this function after the cycles of update calls starts!
    (no interactables are suppose to be added during gameplay) */
    public void AddInteractable(Interactable item)
    {
        interactables.AddLast(item);
    }

    /* Removes an item from InteractableManager's update list. */
    public void RemoveInteractable(Interactable item)
    {
        interactables.Remove(item);
    }

    /* Sets the inRangeInteractable to null. */
    public void ResetInRangeInteractable()
    {
        inRangeInteractable = null;
        ToggleInteractionPrompt(false);
    }

    void Update()
    {
        if (inRangeInteractable == null)
        {
            foreach (Interactable other in interactables)
            {
                float newRange = Range(other);
                if (newRange < Mathf.Infinity)
                {
                    inRangeInteractable = other;
                    ToggleInteractionPrompt(true);
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

    /* Displays an interaction prompt with a message from the current inRangeInteractable. */
    void ToggleInteractionPrompt(bool toggle)
    {
        if (toggle  && inRangeInteractable != null)  // Safety check
        {
            interactionPrompt.GetComponent<Text>().text = inRangeInteractable.PromptMessage;
            print(inRangeInteractable.PromptMessage);
            interactionPrompt.SetActive(true);
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }

    /* Returns the distance if other is in a certain range and in covers the center of the screen, otherwise 
       returns infinity. */
    float Range (Interactable other)
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 3, rayLayerMask) && (hit.collider == other.GetComponent<Collider>()))
        {
            return hit.distance;
        }

        return Mathf.Infinity;
    }
}