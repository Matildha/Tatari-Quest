using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimFactory : MonoBehaviour {

    public GameObject orgVictim;
    public WorldManager worldMan;
    static public int[] maxVictims = { 4, 5, 6 };  // TODO: Update when we have more spawn pos!!


    public void CreateVictims(List<string> symptoms)
    {
        Vector3[] occupiedPositions = new Vector3[maxVictims[GameController.instance.diffLvl]];

        Random.InitState(System.DateTime.Now.Millisecond);

        int i = 0;
        int whileSaver = 0;

        while (i < maxVictims[GameController.instance.diffLvl] && whileSaver < 100)
        {
            whileSaver++;
            if (whileSaver == 100) print("Could not place all victims!");

            // Set a random position
            int areaID = Random.Range(0, worldMan.numberOfWorldAreas);
            WorldArea worldArea = worldMan.worldAreas[areaID];
            if (worldArea.nrVictimPositions == 0)
                continue;
            GameObject rndPosition = worldArea.victimPositions[Random.Range(0, worldArea.nrVictimPositions)];
            // If position has already been assigned, go back and choose a new one
            if (rndPosition == null) print("is null");
            if (System.Array.Exists<Vector3>(occupiedPositions, element => element == rndPosition.transform.position))
                continue;

            occupiedPositions[i] = rndPosition.transform.position;

            string symptom = symptoms[Random.Range(0, Inventory.MAX_NR_SCROLLS - i)];

            GameObject newVictimGameObj = Instantiate(orgVictim) as GameObject;
            Victim newVictim = newVictimGameObj.GetComponent<Victim>();

            newVictim.symptom = symptom;
            newVictim.transform.position = rndPosition.transform.position;
            newVictim.transform.rotation = rndPosition.transform.rotation;
            newVictim.gameObject.transform.SetParent(transform);
            newVictim.gameObject.SetActive(true);
            newVictim.gameObject.layer = 9;

            worldMan.worldAreas[areaID].interactables.AddLast(newVictim);

            i++;

            print("Created victim symptom " + newVictim.symptom + " at pos " + newVictim.transform.position
                + " in area " + areaID);
        }
    }
}
