using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainZone : MonoBehaviour {

    public int[] visibleAreas;
    public int[] audibleAreas;

    ParticleSystem system;
    AudioSource soundEffect;


    private void Start()
    {
        system = GetComponent<ParticleSystem>();
        soundEffect = transform.Find("Sound").gameObject.GetComponent<AudioSource>();
    }

    public void UpdateSystems(int newArea)
    {
        // Rain that is visible is also audible
        if (System.Array.Exists<int>(visibleAreas, element => element == newArea))
        {
            system.Play();
            soundEffect.enabled = true;
            print("Started rain and sound" + newArea);
            return;
        }
        else
        {
            print("Stoped rain " + newArea);
            system.Stop();        
        }

        // Rain can be audible but not visible
        if (System.Array.Exists<int>(audibleAreas, element => element == newArea))
       {
           soundEffect.enabled = true;
           print("Started rain sound " + newArea);
       }
       else
       {
           print("Stoped rain sound " + newArea);
           soundEffect.enabled = false;
       }
    }
}
