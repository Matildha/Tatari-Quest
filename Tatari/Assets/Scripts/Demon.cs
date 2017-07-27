using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour {

    public GameObject player;
    public DemonSpawn demonSpawn;
    public float percSpeed;

    public bool autoHoming;  // Will make the demon update the player position
    public const float FEAR_INCREASE = 5;

    public Vector3 start;
    public GameObject controlP1;
    public GameObject controlP2;
    public AudioClip sound;
    Vector3 end;

    GameObject body;

    bool isDying;
    bool hasPlayedSound;
    float t;
    float startTime;

    void Start () {
        hasPlayedSound = false;
        t = 0f;
        startTime = Time.time;
        end = player.transform.Find("Target").position;
        body = transform.Find("Demon").gameObject;
        
	}
	
	void Update () {

        if(isDying && GetComponent<AudioSource>().isPlaying)
        {
            return;
        }
        else if(isDying)
        {
            demonSpawn.activeDemon = false;
            Destroy(gameObject);
        }

        if(autoHoming) end = player.transform.Find("Target").position;

        //Vector3 frameStartPos = transform.position;

        t += percSpeed * Time.deltaTime;
        // Bezier curve movement towards end position
        transform.position = Mathf.Pow(1 - t, 3) * start + 3 * Mathf.Pow(1 - t, 2) * t * controlP1.transform.position +
                                                3 * (1 - t) * Mathf.Pow(t, 2) * controlP2.transform.position + Mathf.Pow(t, 3) * end;

        body.transform.LookAt(player.transform, Vector3.up);
        if(!hasPlayedSound && Vector3.Distance(transform.position, end) < 3f)
        {
            GetComponent<AudioSource>().clip = sound;
            //print("Demon sound: " + sound);
            GetComponent<AudioSource>().Play();
            hasPlayedSound = true;
            //print("Demon plays sound");
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.tag == "PlayerHitbox")
        {
            //print("Demon found player, selfdestruct");
            //demonSpawn.activeDemon = false;
            //Destroy(gameObject);         
        }*/
        // We don't want to register hits on the player's physical collider
        if(other.gameObject.tag == "Player")
        {
            print("Demon hit player collider. Cont. to live.");
            return;
        }
        transform.Find("Demon").gameObject.SetActive(false);
        print("Demon found player hitbox or environment obstacle in trigger, selfdestruct");
        isDying = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Demon found obstacle in collision, selfdestruct");
        demonSpawn.activeDemon = false;
        Destroy(gameObject);
    }
}
