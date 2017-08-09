using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * BackgroundMusic is added to "DontDestroyOnLoad". 
 * 
 * Contains functionality to play a from a audioSource in a normal loop
 * or a variated loop. The latter is variated by volume in a continuously random 
 * calculated sinus curve shape. Quiet periods with volume = 0 is also randomly 
 * generated in the variated loop. 
*/
public class BackgroundMusic : MonoBehaviour {

    AudioSource audioSource;
    bool normalLoop;

    float volumeX;
    float volumePeriod;
    float volumeAmp;
    const float NORMAL_VOL = 0.75f;

    int quietPeriod;  // 0,1 => new quiet period, 2 => volume > 0
    float quietPeriodStart;
    float quietPeriodTime;


	public void Init () {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) print("null in music start");
        DontDestroyOnLoad(gameObject);
        normalLoop = true;
        volumeX = 0;
        volumePeriod = 0.1f;
        volumeAmp = 0.2f;
        quietPeriod = 0;
    }
	
	private void Update () {
        if (normalLoop) return;  // We don't want to modify the sound volume

		if(quietPeriod == 2)
        {
            volumeX++;
            if (volumeX > (2 * Mathf.PI) / volumePeriod)
            {
                volumeX = 0;
                Random.InitState(System.DateTime.Now.Millisecond);
                volumePeriod = Random.Range(0.0075f, 0.03f);  // values set from testing
                volumeAmp = Random.Range(0f, 0.4f);
                quietPeriod = Random.Range(0, 3);  // Determine whether the next period should be quiet or not

                if (quietPeriod < 2) quietPeriodTime = Random.Range(5, 11);
            }
            float volume = volumeAmp * Mathf.Sin(volumePeriod * volumeX) + volumeAmp;
            audioSource.volume = volume;
        }

        if(quietPeriod < 2 && Time.time - quietPeriodStart > quietPeriodTime)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            quietPeriod = Random.Range(0, 3);
            if (quietPeriod == 1)
            {
                quietPeriodTime = Random.Range(5, 11);
                audioSource.volume = 0;
                quietPeriodStart = Time.time;
            }
            //print("New period is " + quietPeriod);
            //print("With time " + quietPeriodTime);
        }
	}

    public void StartNormalLoop()
    {
        audioSource.Play();
        normalLoop = true;
        audioSource.volume = NORMAL_VOL;
        //print("Starts normal bg music");
    }

    public void StartVariatedLoop()
    {
        audioSource.Play();
        normalLoop = false;
        //print("Start variated bg music");
    }
}
