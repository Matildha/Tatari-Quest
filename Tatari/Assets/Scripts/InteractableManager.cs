using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour {

    public Camera playerCamera;
    public LinkedList<Interactable> interactables;

    public Interactable inRangeInteractable;
    bool promptIsDisplayed;

    int rayLayerMask;

    private void Start()
    {
        rayLayerMask = 1 << 9;  // Bit mask, shift index of Interactable layer (9)
        inRangeInteractable = null;
        promptIsDisplayed = false;
        interactables = new LinkedList<Interactable>();

        foreach(Transform transChild in transform)
        {
            Interactable child = transChild.GetComponent<Interactable>();     
            interactables.AddLast(child);
        }
    }

    void Update()
    {
        if (inRangeInteractable == null)
        {
            inRangeInteractable = null;
            foreach (Interactable other in interactables)
            {
                float newRange = Range(other);
                if (newRange < Mathf.Infinity)
                {
                    inRangeInteractable = other;
                    inRangeInteractable.InteractPrompt(true);
                    break;
                }
            }
        }
        //Only update status for current in range interactable
        else if(Range(inRangeInteractable) == Mathf.Infinity)
        {
            inRangeInteractable.InteractPrompt(false);
            inRangeInteractable = null;
        }
    }

    /* Returns infinity if (center of) 'other' is not visible by player, otherwise the
     distance to 'other'. */
    float Range (Interactable other)
    {
        //Ray ray = playerCamera.ScreenPointToRay(other.transform.position);
        /*RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, other.transform.position, out hit))
                            //&& (hit.transform.gameObject.GetInstanceID() == other.GetInstanceID()))
                            //&& hit.transform.gameObject.tag == "Scroll")
        {
            print("An interactable is visible");
            print(hit.distance + hit.transform.gameObject.tag);
            return hit.distance;
        }
        
        //print(hit.transform.gameObject.tag);
        return Mathf.Infinity;*/

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 4, rayLayerMask))
        {
            //print(hit.distance + " " + hit.transform.gameObject.tag);
            //Debug.DrawLine(ray.origin, hit.point, Color.green);
            return hit.distance;
        }

        return Mathf.Infinity;
    }

  
}
