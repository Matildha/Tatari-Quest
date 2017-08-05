using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainZone : MonoBehaviour {

    public int[] activeAreas;

    ParticleSystem system;
    AudioSource soundEffect;


    public void Init()
    {
        system = GetComponent<ParticleSystem>();
        soundEffect = transform.Find("Sound").gameObject.GetComponent<AudioSource>();
    }

    public void UpdateSystem(int newArea)
    {
        if (System.Array.Exists<int>(activeAreas, element => element == newArea))
        {
            system.Play();
            soundEffect.enabled = true;
            //print("Started rain" + newArea);
            soundEffect.enabled = true;
        }
        else
        {   
            soundEffect.enabled = false;
            //print("Stoped rain " + newArea);
            system.Stop();        
        }
    }
}
