using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * VictimFactory contains a single method to randomly 
 * position victims throughout the game world in accordance to 
 * the WorldArea:s' list of victim positions. 
 * 
 * Each victim is given a randomly selected symptom. 
 * 
 * VictimFactory contains an array of max number of victims
 * for the different difficulty levels. (determined by GameController "diffLvl")
*/

public class VictimFactory : MonoBehaviour {

    public GameObject orgVictim;
    public WorldManager worldMan;
    static public int[] maxVictims = { 4, 6, 8 };


    public void CreateVictims(List<string> symptoms)
    {
        Vector3[] occupiedPositions = new Vector3[maxVictims[GameController.instance.diffLvl]];

        Random.InitState(System.DateTime.Now.Millisecond);

        int i = 0;
        int whileSaver = 0;

        // Keep looping until all victims haven succesfully has been positioned
        // (or worse case we fail to do this under 100 attempts,
        // NB: This scenario is currently unhandled!)
        while (i < maxVictims[GameController.instance.diffLvl] && whileSaver < 100)
        {
            whileSaver++;
            if (whileSaver == 100) print("Could not place all victims!");

            // Set a random position
            int areaID = Random.Range(0, worldMan.numberOfWorldAreas);
            WorldArea worldArea = worldMan.worldAreas[areaID];
            if (worldArea.nrVictimPositions == 0)
                continue;  // This area has no victim positions, go back and choose a new one

            GameObject rndPosition = worldArea.victimPositions[Random.Range(0, worldArea.nrVictimPositions)];
            // If position has already been assigned, go back and choose a new one
            if (System.Array.Exists<Vector3>(occupiedPositions, element => element == rndPosition.transform.position))
                continue;

            occupiedPositions[i] = rndPosition.transform.position;

            // Distribute a random symptom
            string symptom = symptoms[Random.Range(0, Inventory.MAX_NR_SCROLLS - i)];

            // Instantiate the actual new victim and assign all values
            GameObject newVictimGameObj = Instantiate(orgVictim) as GameObject;
            Victim newVictim = newVictimGameObj.GetComponent<Victim>();

            newVictim.symptom = symptom;
            newVictim.transform.position = rndPosition.transform.position;
            newVictim.transform.rotation = rndPosition.transform.rotation;
            newVictim.gameObject.transform.SetParent(transform);
            newVictim.gameObject.SetActive(true);
            newVictim.gameObject.layer = InteractableManager.INTERACTABLE_LAYER;

            worldMan.worldAreas[areaID].interactables.AddLast(newVictim);

            i++;

            //print("Created victim symptom " + newVictim.symptom + " at pos " + newVictim.transform.position
            //    + " in area " + areaID);
        }
    }
}
