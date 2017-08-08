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
    public GameObject playerSpawnPos;
    public int playerSpawnArea;

    public int numberOfWorldAreas;
    public WorldArea[] worldAreas;
    public int currentWorldArea;

    public RainZone[] rainZones;
    public AudioClip rainSound;

    List<string> symptoms;

    float intManLastUpdate;
    const float INTMAN_UPDATE_DELTA = 0.5f;

    float demonSpawnLastUpdate;
    const float DEMONSPAWN_UPDATE_DELTA = 0.25f;


	void Start () {
        scrollFact.Init();
        Init();
        intManager.Init();

        player.transform.position = playerSpawnPos.transform.position;
        player.transform.rotation = playerSpawnPos.transform.rotation;
        currentWorldArea = playerSpawnArea;

        foreach(RainZone zone in rainZones)
        {
            zone.Init();
            zone.UpdateSystem(currentWorldArea);
        }
        print("Start in worldmanager");
	}

    public void SwitchArea(int area)
    {
        if (area == currentWorldArea) return;

        intManager.UpdateActive(area);
        currentWorldArea = area;
        player.SwitchArea(area);

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
        if (Time.time - intManLastUpdate > INTMAN_UPDATE_DELTA)
        {
            intManager.ExUpdate();
            intManLastUpdate = Time.time;
        }
        //demonSpawn.ExUpdate();
        if (Time.time - demonSpawnLastUpdate > DEMONSPAWN_UPDATE_DELTA)
        {
            demonSpawn.ExUpdate();
            demonSpawnLastUpdate = Time.time;
        }
    }

    private void FixedUpdate()
    {
        player.ExFixedUpdate();
    }

    /*List<string> LoadScrollInfo()
    {
        List<string> symptoms = new List<string>();
        FileStream stream;

        stream = new FileStream("Assets/TextFiles/scroll-info.txt", FileMode.Open, FileAccess.Read);
        stream = new FileStream("TatariQuest_Data/Resources/scroll-info.txt", FileMode.Open, FileAccess.Read);

        StreamReader reader = new StreamReader(stream);

        for (int i = 0; i < Inventory.MAX_NR_SCROLLS; i++)
        {
            symptoms.Add(reader.ReadLine());
        }

        reader.Close();
        stream.Close();
        
        return symptoms;
    }*/

    List<string> LoadScrollInfo()
    {
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