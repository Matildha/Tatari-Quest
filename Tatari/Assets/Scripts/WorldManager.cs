using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * WorldManager is responsible for initializing world areas and populate their 
 * lists of Interactable:s. It also places the original player position. 
 * 
 * WorldManager is futher more responsible for updating Player, 
 * InteractableManager and DemonSpawn and all RainZone:s when switching
 * "currentWorldArea". 
 * 
 * WorldManager is the primary coordinator of the world area system and the
 * dynamic gameObject:s set up in the game world. 
*/

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

    /* Sets the current world area to given index and updates rain zones. 
     If area == currentWorldArea this function will return immediately without
     expending any extra work. */
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

            // Doors are added this way because of Unity Editor's interface not enabling
            // graphical addition of gameObject:s to lists
            foreach(Door door in worldAreas[i].doors)
            {
                worldAreas[i].interactables.AddLast((Interactable) door);
            }
        }

        List<string> symptoms = scrollFact.LoadScrollInfo();
        scrollFact.CreateScrolls(symptoms);
        victFact.CreateVictims(symptoms);
        //print("Symptoms length " + symptoms.Count);
    }

    /* Update player on every call. Updates InteractableManager and DemonSpawn on intervals. */
    void Update()
    {
        player.ExUpdate();
        if (Time.time - intManLastUpdate > INTMAN_UPDATE_DELTA)
        {
            intManager.ExUpdate();
            intManLastUpdate = Time.time;
        }

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
}
