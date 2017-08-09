using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Demon modifies the transform of the gameObject and child transform "Demon". 
 * At enabling, Demon will move in a bezier curve using given control points until colliding
 * with a collider that is not the player's physical hitbox or a threshold. 
 * 
 * End point is set to be player child object "Target". 
 * If "autohoming" is true the end point will be constantly updated. 
 *    
 * Demon plays a given "audioClip" when within certain range of "Target". The sound
 * will finish playing even if Demon has collided.  
 *    
 * The Demon gameObject will be destroyed after colliding with a valid collider and
 * when audio is not playing. 
*/ 

public class Demon : MonoBehaviour {

    public GameObject player;
    public DemonSpawn demonSpawn;

    public bool autoHoming;  // Will make the demon update the player's target position
    static public int[] fearIncreaseLvls = {4, 6, 8};

    public AudioClip sound;

    public Vector3 start;
    public GameObject controlP1;
    public GameObject controlP2;
    Vector3 end;
    Transform target;

    const float PERC_SPEED = 0.35f;

    GameObject body;

    bool isDying;
    bool hasPlayedSound;
    float t;  // how far on the curve the demon has come
    float startTime;

    private void Start () {
        hasPlayedSound = false;
        t = 0f;
        startTime = Time.time;
        target = player.transform.Find("Target");
        end = target.position;
        body = transform.Find("Demon").gameObject;    
	}
	
	private void Update () {

        if(isDying && GetComponent<AudioSource>().isPlaying)
        {
            // Waiting for audio to finish
            return;
        }
        else if(isDying)
        {
            Destroy(gameObject);
        }

        if(autoHoming) end = target.position;

        t += PERC_SPEED * Time.deltaTime;

        // Bezier curve movement towards end position
        transform.position = Mathf.Pow(1 - t, 3) * start + 3 * Mathf.Pow(1 - t, 2) * t * controlP1.transform.position +
                                                3 * (1 - t) * Mathf.Pow(t, 2) * controlP2.transform.position + Mathf.Pow(t, 3) * end;

        body.transform.LookAt(target.transform, Vector3.up);

        if(!hasPlayedSound && Vector3.Distance(transform.position, end) < 3f)
        {
            GetComponent<AudioSource>().clip = sound;
            GetComponent<AudioSource>().Play();
            hasPlayedSound = true;
            //print("Demon plays sound");
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        // We don't want to register hits on the player's physical collider or thresholds
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Threshold")
        {
            //print("Demon hit player collider or threshold. Cont. to live.");
            return;
        }
        body.gameObject.SetActive(false);
        //print("Demon found player hitbox or environment obstacle in trigger, selfdestruct");
        isDying = true;
        demonSpawn.activeDemon = false;
        demonSpawn.SetSafeStartTime();
        print("Sets active demon to false");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("Demon found obstacle in collision, selfdestruct");
        body.gameObject.SetActive(false);
        isDying = true;
        demonSpawn.activeDemon = false;
        demonSpawn.SetSafeStartTime();
        print("Sets active demon to false");
    }
}
