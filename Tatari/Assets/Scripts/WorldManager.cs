using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public InteractableManager intManager;
    public ScrollFactory scrollFact;
    public VictimFactory victFact;
    public DemonSpawn demonSpawn;
    public Player player;

    public int numberOfWorldAreas;
    public WorldArea[] worldAreas;
    public int currentWorldArea;

    public int nrPlayerSpawnAreas;
    public int[] playerSpawnAreas; // These areas are expected to have playerSpawn != null

    public RainZone[] rainZones;
    public AudioClip rainSound;

    float intManLastUpdate;
    const float intManDeltaUpdate = 0.5f;


	void Start () {
        scrollFact.Init();
        Init();
        intManager.Init();
        GeneratePlayerPosition();

        foreach(RainZone zone in rainZones)
        {
            zone.Init();
            zone.UpdateSystem(currentWorldArea);
        }
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
            rainZone.UpdateSystem(area);
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

    void Update()
    {
        player.ExUpdate();
        if (Time.time - intManLastUpdate > intManDeltaUpdate)
        {
            intManager.ExUpdate();
            intManLastUpdate = Time.time;
        }
        demonSpawn.ExUpdate();
    }

    private void FixedUpdate()
    {
        player.ExFixedUpdate();
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

    void GeneratePlayerPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int area = playerSpawnAreas[Random.Range(0, nrPlayerSpawnAreas)];
        if(worldAreas[area].playerSpawn != null)  // Debug check
        {
            player.transform.position = worldAreas[area].playerSpawn.transform.position;
            player.transform.rotation = worldAreas[area].playerSpawn.transform.rotation;
            currentWorldArea = area;
            print("Set player pos to " + player.transform.position);
        }
        else
        {
            print("Forgot to add playerSpawn into worldarea!!");
        }
    }
}