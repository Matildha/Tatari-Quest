using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public InteractableManager intManager;
    public ScrollFactory scrollFact;
    public VictimFactory victFact;
    public Player player;

    public int numberOfWorldAreas;
    public WorldArea[] worldAreas;
    public int currentWorldArea;

    public RainZone[] rainZones;
    public AudioClip rainSound;


	void Start () {
        scrollFact.Init();
        Init();
        intManager.Init();
	}

    public void SwitchArea(int area)
    {

        if (area == currentWorldArea) return;

        // Disable gameObjects in inactive world area
        foreach (Interactable obj in worldAreas[currentWorldArea].interactables)
        {
            if (obj.tag == "Door")
            {
                obj.gameObject.GetComponent<Door>().enabled = false;
                //print("Disabled a door");
            }
        }

        currentWorldArea = area;
        player.SwitchArea(area);

        // Enable gameObjects in current active world area
        foreach(Interactable obj in worldAreas[currentWorldArea].interactables)
        {
            if (obj.tag == "Door")
            {
                obj.gameObject.GetComponent<Door>().enabled = true;
                //print("Enabled a door");
            }
        }

        // Update rain zones
        foreach(RainZone rainZone in rainZones)
        {
            rainZone.UpdateSystems(area);
        }
    }

    void Init()
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

        List<string> symptoms = LoadScrollInfo();
        scrollFact.CreateScrolls(symptoms);
        victFact.CreateVictims(symptoms);
        //print("Symptoms length " + symptoms.Count);
    }

    List<string> LoadScrollInfo()
    {
        List<string> symptoms = new List<string>();
        FileStream stream = new FileStream("Assets/TextFiles/scroll-info.txt", FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(stream);

        for (int i = 0; i < Inventory.MAX_NR_SCROLLS; i++)
        {
            symptoms.Add(reader.ReadLine());
        }

        reader.Close();
        stream.Close();
        
        return symptoms;
    }
}