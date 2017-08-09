using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * FootSteps use the "isWalking" boolean in PlayerController to determine when to play
 * the AudioSource attached to the same GameObject. 
 * 
 * FootSteps automatically creates a variated sound using random values
 * for audio volume and pitch. Footsteps also include an array of worldAreas which has
 * "soft floors" where the audio volume should be generally lower. 
 * 
 * The "fourceSound" boolean can be set to make FootSteps play audio even though 
 * "isWalking" is not set. 
*/

public class FootSteps : MonoBehaviour {

    public Player player;
    public int currentArea;
    public bool forceSound;

    int[] softFloorAreas = { 0, 4, 6, 10, 17, 18, 15, 14 };
    float volMin;
    float volMax;

	
	void Update () {

        Random.InitState(System.DateTime.Now.Millisecond);

        if((player.playerController.isWalking || forceSound) && !gameObject.GetComponent<AudioSource>().isPlaying)
            {
            gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.8f);

            if(System.Array.Exists<int>(softFloorAreas, element => element == currentArea)) {  // TODO: AVOID! 
                volMin = 0.3f;
                volMax = 0.5f;
            }
            else
            {
                volMin = 0.8f;
                volMax = 1f;
            }
            gameObject.GetComponent<AudioSource>().volume = Random.Range(volMin, volMax);
            gameObject.GetComponent<AudioSource>().Play();
        }
	}
}
