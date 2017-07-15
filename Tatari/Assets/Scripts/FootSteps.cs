using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour {

    public int currentArea;

    AudioSource audioSource;
    int[] softFloorAreas = { 0, 4, 6, 10 };
    float volMin;
    float volMax;

	void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	void Update () {

        Random.InitState(System.DateTime.Now.Millisecond);

    if(gameObject.GetComponent<PlayerController>().isWalking && !audioSource.isPlaying)
        {
            audioSource.pitch = Random.Range(0.5f, 0.8f);

            if(System.Array.Exists<int>(softFloorAreas, element => element == currentArea)) {
                volMin = 0.2f;
                volMax = 0.4f;
            }
            else
            {
                volMin = 0.7f;
                volMax = 1f;
            }
            audioSource.volume = Random.Range(volMin, volMax);
            audioSource.Play();
        }
	}
}
