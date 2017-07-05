using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawn : MonoBehaviour {

    public GameObject orgDemon;
    public Player player;
    public WorldManager worldMan;
    public bool activeDemon;

    float increaseChance = 0.75f;
    float invertChance;

    float baseSafeTime = 5f;  // seconds
    float safeTime;
    float decreaseSafeTime = 0.05f;
    float startTime;

    bool hasReset;

	void Start ()
    {
        invertChance = 0;
        startTime = Time.time;
        safeTime = baseSafeTime;
        hasReset = false;
	}

    void SpawnDemon()
    {
        print("Demon spawnas");

        GameObject newDemonGameObject = Instantiate(orgDemon) as GameObject;
        Demon newDemon = newDemonGameObject.GetComponent<Demon>();

        newDemon.controlP1 = worldMan.demonBezCP[0];
        newDemon.controlP2 = worldMan.demonBezCP[1];

        Random.InitState(System.DateTime.Now.Millisecond);

        // Spawn the demon somewhehere between the two start control points given by WorldManager
        Vector3 startCP1 = worldMan.demonStart[0].transform.position;
        Vector3 startCP2 = worldMan.demonStart[1].transform.position;
        newDemon.start = new Vector3(Random.Range(startCP1.x, startCP2.x),
                                        Random.Range(startCP1.y, startCP2.y),
                                        Random.Range(startCP1.z, startCP2.z));
        newDemon.autoHoming = Random.Range(0, 2) == 1;
        print("New demon homing " + newDemon.autoHoming);
        newDemon.transform.SetParent(this.transform);

        newDemon.gameObject.SetActive(true);
        activeDemon = true;
    }

    void Update ()
    {
        if (player.lantern.isLit && !activeDemon)
        {
            hasReset = false;

            Random.InitState(System.DateTime.Now.Millisecond);

            safeTime -= decreaseSafeTime * Time.deltaTime;
            if (safeTime <= 0) safeTime = 0;
            
            if ((Time.time - startTime) > safeTime)
            {
                invertChance += increaseChance * Time.deltaTime;
                if (invertChance >= 0.75f) invertChance = 0.75f;

                float rnd = Random.Range(0f, 1f - invertChance);

                if (rnd <= 0.25f)
                {
                    invertChance = 0;
                    SpawnDemon();
                }
                else print("Demon spawnas inte");
                startTime = Time.time;
            }
        }
        else if(!hasReset && !activeDemon)
        {
            safeTime = baseSafeTime;
            invertChance = 0;
            startTime = Time.time;

            hasReset = true;
        }
	}
}