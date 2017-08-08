﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour {

    public GameObject player;
    public DemonSpawn demonSpawn;
    public float percSpeed;

    public bool autoHoming;  // Will make the demon update the player position
    static public int[] fearIncreaseLvls = {5, 8, 10}; 

    public Vector3 start;
    public GameObject controlP1;
    public GameObject controlP2;
    public AudioClip sound;
    Vector3 end;
    Transform target;

    GameObject body;

    bool isDying;
    bool hasPlayedSound;
    float t;
    float startTime;

    void Start () {
        hasPlayedSound = false;
        t = 0f;
        startTime = Time.time;
        target = player.transform.Find("Target");
        end = target.position;
        body = transform.Find("Demon").gameObject;    
	}
	
	void Update () {

        if(isDying && GetComponent<AudioSource>().isPlaying)
        {
            return;
        }
        else if(isDying)
        {
            Destroy(gameObject);
        }

        if(autoHoming) end = target.position;

        //Vector3 frameStartPos = transform.position;

        t += percSpeed * Time.deltaTime;
        // Bezier curve movement towards end position
        transform.position = Mathf.Pow(1 - t, 3) * start + 3 * Mathf.Pow(1 - t, 2) * t * controlP1.transform.position +
                                                3 * (1 - t) * Mathf.Pow(t, 2) * controlP2.transform.position + Mathf.Pow(t, 3) * end;

        body.transform.LookAt(target.transform, Vector3.up);
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
        // We don't want to register hits on the player's physical collider or thresholds
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Threshold")
        {
            //print("Demon hit player collider or threshold. Cont. to live.");
            return;
        }
        transform.Find("Demon").gameObject.SetActive(false);
        //print("Demon found player hitbox or environment obstacle in trigger, selfdestruct");
        isDying = true;
        demonSpawn.activeDemon = false;
        print("Sets active demon to false");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("Demon found obstacle in collision, selfdestruct");
        transform.Find("Demon").gameObject.SetActive(false);
        //demonSpawn.activeDemon = false;
        isDying = true;
        demonSpawn.activeDemon = false;
        print("Sets active demon to false");
        //Destroy(gameObject);
    }
}
