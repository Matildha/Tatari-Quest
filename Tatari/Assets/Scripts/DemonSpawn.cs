using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawn : MonoBehaviour {

    public Player player;

    Demon activeDemon;
    Vector3[] spawnPositions = { new Vector3(-5.7f, 3.48f, 4.89f), new Vector3(-7.26f, 2.19f, 4.89f) };

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

	void Update ()
    {
        if (player.lantern.isLit)
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
        else
        {
            if (!hasReset)
            {
                safeTime = baseSafeTime;
                invertChance = 0;
                startTime = Time.time;

                hasReset = true;
            }
        }
	}

    void SpawnDemon()
    {
        print("Demon spawnas");
    }
}
