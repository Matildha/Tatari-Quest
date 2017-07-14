using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour {

    public GameObject player;
    public DemonSpawn demonSpawn;
    public float percSpeed;

    public Vector3 start;
    public GameObject controlP1;
    public GameObject controlP2;
    Vector3 end;

    public bool autoHoming;  // Will make the demon update the player position

    public const float FEAR_INCREASE = 20;

    float t;
    bool stop;
    float startTime;

    void Start () {
        stop = false;
        t = 0f;
        startTime = Time.time;
        end = player.transform.Find("Target").position;
	}
	
	void Update () {

        if (!stop)
        {
            if(autoHoming) end = player.transform.Find("Target").position;
            t += percSpeed * Time.deltaTime;
            // Bezier curve movement towards end position
            transform.position = Mathf.Pow(1 - t, 3) * start + 3 * Mathf.Pow(1 - t, 2) * t * controlP1.transform.position +
                                                    3 * (1 - t) * Mathf.Pow(t, 2) * controlP2.transform.position + Mathf.Pow(t, 3) * end;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerHitbox")
        {
            print("Demon found player, selfdestruct");
            stop = true;     
        }
        else
        {
            print("Demon found obstacle in trigger, selfdestruct");
        }
        demonSpawn.activeDemon = false;
        Destroy(gameObject);
    }

    // TODO: Remove!
    private void OnCollisionEnter(Collision collision)
    {
        print("Demon found obstacle in collision, selfdestruct");
        stop = true;
        demonSpawn.activeDemon = false;
        Destroy(gameObject);
    }
}
