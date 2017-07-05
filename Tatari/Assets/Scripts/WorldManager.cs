using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public InteractableManager intManager;
    public ScrollFactory scrollFact;

    public int numberOfWorldAreas;
    public WorldArea[] worldAreas;
    public int currentWorldArea;


	void Start () {
        scrollFact.Init();
        Init();
        intManager.Init();
	}

    private void Update()
    {
        
    }

    private void Init()
    {
        // Set up world areas
        for (int i = 0; i < numberOfWorldAreas; i++)
        {
            worldAreas[i].interactables = new LinkedList<Interactable>();
        }

        scrollFact.CreateScrolls();

        // Add doors to interactable list in each world area
        foreach (Transform door in GameObject.Find("Doors").transform)
        {
            int area = door.gameObject.GetComponent<Door>().worldAreaID;
            worldAreas[area].interactables.AddLast(door.gameObject.GetComponent<Door>());
        }
    }
}