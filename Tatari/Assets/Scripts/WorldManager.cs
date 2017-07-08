using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public InteractableManager intManager;
    public ScrollFactory scrollFact;

    public int numberOfWorldAreas;
    public WorldArea[] worldAreas;
    public int currentWorldArea;


    public void SwitchArea(int area)
    {
        // Disable gameObjects in inactive world area
        foreach (Interactable obj in worldAreas[currentWorldArea].interactables)
        {
            if (obj.tag == "Door")
            {
                obj.gameObject.GetComponent<Door>().enabled = false;
                print("Disabled a door");
            }
        }

        currentWorldArea = area;
        // Enable gameObjects in current active world area
        foreach(Interactable obj in worldAreas[currentWorldArea].interactables)
        {
            if (obj.tag == "Door")
            {
                obj.gameObject.GetComponent<Door>().enabled = true;
                print("Enabled a door");
            }
        }
    }

	void Start () {
        scrollFact.Init();
        Init();
        intManager.Init();
	}

    private void Init()
    {
        // Set up world areas
        for (int i = 0; i < numberOfWorldAreas; i++)
        {
            worldAreas[i].interactables = new LinkedList<Interactable>();

            foreach(Door door in worldAreas[i].doors)
            {
                worldAreas[i].interactables.AddLast((Interactable) door);
            }

        }

        scrollFact.CreateScrolls();
    }
}