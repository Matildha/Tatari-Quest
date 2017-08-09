using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * ScrollFactory instantiate "orgScroll" objects and randomly positions
 * them in the game world according to WorldArea's array of scroll positions. 
 * ScrollFactory also assigns them unique but randomly distributed colors.
 * 
 * ScrollFactory declares a set of enum constants to represent the scroll
 * color property. 
 * 
 * ScrollFactory contains method LoadScrollInfo() whicch contains hard-coded
 * symptoms strings. 
*/

public class ScrollFactory : MonoBehaviour {

    public enum ScrollColors { Red, Blue, White, Purple, Green, Yellow };


    public GameObject orgScroll;
    public WorldManager worldMan;

    List<ScrollColors> colors;


    public void Init()
    {
        colors = new List<ScrollColors>(new ScrollColors[] { ScrollColors.Red, ScrollColors.Blue, ScrollColors.White,
                                                        ScrollColors.Purple, ScrollColors.Green, ScrollColors.Yellow });
    }

    public void CreateScrolls(List<string> _symptoms)
    {
        List<string> symptoms = new List<string>(_symptoms);  // So we can destructively remove items
        Vector3[] occupiedPositions = new Vector3[Inventory.MAX_NR_SCROLLS];

        int content;
        int color;

        Random.InitState(System.DateTime.Now.Millisecond);
        int i = 0;
        int whileSaver = 0;

        // Keep looping until all scrolls haven succesfully has been positioned
        // (or worse case we fail to do this under 100 attempts,
        // NB: This scenario is currently unhandled!)
        while(i < Inventory.MAX_NR_SCROLLS && whileSaver < 100)
        {
            whileSaver++;
            if (whileSaver == 100) print("Could not place all scrolls");  // For debug, should not happen! But number is adjusted to the current number
                                                                         // of scroll positions and max number scrolls. 

            Scroll.ScrollInfo info = new Scroll.ScrollInfo();
            
            // Set a random position
            int areaID = Random.Range(0, worldMan.numberOfWorldAreas);
            WorldArea worldArea = worldMan.worldAreas[areaID];
            if (worldArea.nrScrollPositions == 0)
                continue;
            GameObject rndPosition = worldArea.scrollPositions[Random.Range(0, worldArea.nrScrollPositions)];

            // If position has already been assigned, go back and choose a new one
            if (System.Array.Exists<Vector3>(occupiedPositions, element => element == rndPosition.transform.position))
                continue;

            occupiedPositions[i] = rndPosition.transform.position;

            // Add random content and random color        
            content = Random.Range(0, Inventory.MAX_NR_SCROLLS - i);
            color = Random.Range(0, Inventory.MAX_NR_SCROLLS - i);
            info.content = symptoms[content];
            symptoms.RemoveAt(content);
            info.color = colors[color];
            colors.RemoveAt(color);

            // Add properties to new scroll and add it to the correct WorldArea's list of Interactable:s
            
            GameObject newScrollGameObj = Instantiate(orgScroll) as GameObject;
            Scroll newScroll = newScrollGameObj.GetComponent<Scroll>();

            newScroll.info = info;
            newScroll.transform.position = rndPosition.transform.position;
            newScroll.transform.rotation = rndPosition.transform.rotation;
            newScroll.gameObject.transform.SetParent(transform);
            newScroll.gameObject.SetActive(true);
            newScroll.gameObject.layer = InteractableManager.INTERACTABLE_LAYER;

            worldMan.worldAreas[areaID].interactables.AddLast(newScroll);

            i++;

            //print("Created scroll " + newScroll + " " + newScroll.info.content + " " + 
            //                               newScroll.info.color + " " + newScroll.transform.position);
        }
    }

    public List<string> LoadScrollInfo()
    {
        List<string> symptoms = new List<string>();

        string[] sympt = { "hallucinations, panic attacks",
                                        "blindness, hallucinations",
                                        "paranoia, hysteria",
                                        "sleep deprivation",
                                        "blind rage",
                                        "paranoia, panic attacks"};

        symptoms = new List<string>(sympt);

        return symptoms;
    }
}
