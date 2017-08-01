using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawn : MonoBehaviour {

    public GameObject orgDemon;
    public Player player;
    public WorldManager worldMan;
    public AudioClip[] demonSounds;
    public int nrDemonSounds;
    public bool activeDemon;

    float increaseChance = 0.75f;
    float invertChance;

    float[] baseSafeTimes = { 7f, 5f, 3f };  // seconds
    float safeTime;
    float decreaseSafeTime = 0.05f;
    float startTime;

    bool hasReset;

	void Start ()
    {
        invertChance = 0;
        startTime = Time.time;
        safeTime = baseSafeTimes[GameController.instance.diffLvl];
        hasReset = false;
	}

    void SpawnDemon()
    {
        //print("Demon spawnas");

        // Check if there are any avaiable demon control points in current world area
        if (worldMan.worldAreas[worldMan.currentWorldArea].demonBezCP.Length == 0) return;

        // Spawn the demon somewhehere between the two start control points given by WorldManager
        Vector3 startCP1 = worldMan.worldAreas[worldMan.currentWorldArea].demonStart[0].transform.position;
        Vector3 startCP2 = worldMan.worldAreas[worldMan.currentWorldArea].demonStart[1].transform.position;

        Vector3 rndStart = player.transform.position;
        int whileSaver = 0;
        while (Vector3.Distance(rndStart, player.transform.position) < 1f && whileSaver < 5)  // max 4 attempts
        {
            rndStart = new Vector3(Random.Range(startCP1.x, startCP2.x),
                                            Random.Range(startCP1.y, startCP2.y),
                                            Random.Range(startCP1.z, startCP2.z));
            whileSaver++;
            print("Demon start pos rnd attempt");
        }

        GameObject newDemonGameObject = Instantiate(orgDemon) as GameObject;
        Demon newDemon = newDemonGameObject.GetComponent<Demon>();

        newDemon.controlP1 = worldMan.worldAreas[worldMan.currentWorldArea].demonBezCP[0];
        newDemon.controlP2 = worldMan.worldAreas[worldMan.currentWorldArea].demonBezCP[1];

        newDemon.start = rndStart;

        Random.InitState(System.DateTime.Now.Millisecond);

        
        newDemon.transform.position = newDemon.start;

        newDemon.sound = demonSounds[Random.Range(0, nrDemonSounds)];

        //print("Demon spawn at " + newDemon.start);
        newDemon.autoHoming = Random.Range(0, 2) == 1;
        print("New demon homing " + newDemon.autoHoming);
        newDemon.transform.SetParent(this.transform);

        newDemon.gameObject.SetActive(true);
        activeDemon = true;
    }

    public void ExUpdate ()
    {
        if (player.lantern.isLit && !activeDemon)
        {
            hasReset = false;

            safeTime -= decreaseSafeTime * Time.deltaTime;
            if (safeTime <= 0) safeTime = 0;
            
            if ((Time.time - startTime) > safeTime)
            {
                invertChance += increaseChance * Time.deltaTime;
                if (invertChance >= 0.75f) invertChance = 0.75f;

                Random.InitState(System.DateTime.Now.Millisecond);
                float rnd = Random.Range(0f, 1f - invertChance);

                if (rnd <= 0.25f)
                {
                    invertChance = 0;
                    SpawnDemon();
                }
                //else print("Demon spawnas inte");
                startTime = Time.time;
            }
        }
        else if(!hasReset && !activeDemon)
        {
            safeTime = baseSafeTimes[GameController.instance.diffLvl];
            invertChance = 0;
            startTime = Time.time;

            hasReset = true;
        }
	}
}