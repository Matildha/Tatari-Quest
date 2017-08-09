using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * IntroSeq uses the camera cutSceneCam to simulate a recorded cutscene. 
 * 
 * The camera's transform is modified to follow the "path" array of given
 * transforms. 
 * 
 * Audio from AudioSource from the same GameObject is played and "forceSound"
 * of the given FootSteps is set at Start(). 
 * 
 * IntroSeq toggles various graphical components. 
 * 
 * When cutSceneCam reaches the last position in "path" IntroSeq will disable itself,
 * restore the INGAME scnee layout, enable WorldManager and call on Player InitMessages().   
*/

public class IntroSeq : MonoBehaviour {

    public WorldManager worldMan;

    public Camera cutSceneCam;
    public Camera playerCam;
    public Camera blackCam;

    public FootSteps footStepSound;

    public GameObject blackEdgesEffect;
    public GameObject playerStats;
    public GameObject inventory;
    public InfoBox infoBox;
    public RainZone rainZone;

    public Transform[] path;
    public int nrPoints;
    int currentPoint;
    const float SPEED = 1.25f;

    const int CLOSEST_WORLD_AREA = 19;

    bool transitionHasBegun;
    float transitionDuration = 1f;
    float transitionTime;
    float transitionStart;


    private void Start () {
        cutSceneCam.gameObject.SetActive(true);
        blackCam.gameObject.SetActive(false);
        blackEdgesEffect.SetActive(true);
        footStepSound.forceSound = true;
        playerCam.gameObject.SetActive(false);
        playerStats.SetActive(false);
        inventory.SetActive(false);
        infoBox.Close();
        worldMan.intManager.ResetInRangeInteractable();
        currentPoint = 0;
        cutSceneCam.transform.position = path[currentPoint].position;
        rainZone.Init();
        rainZone.UpdateSystem(CLOSEST_WORLD_AREA);
        GetComponent<AudioSource>().Play();
    }
	
	private void Update ()
    {
        if (cutSceneCam.transform.position != path[currentPoint].position)
        {
            cutSceneCam.transform.position = Vector3.MoveTowards(cutSceneCam.transform.position,
                                                                    path[currentPoint].position, SPEED * Time.deltaTime);

            cutSceneCam.transform.LookAt(path[nrPoints - 1], Vector3.up);
        }
        else if (currentPoint == nrPoints - 1)
        {
            if(!transitionHasBegun)
            {
                cutSceneCam.gameObject.SetActive(false);
                blackCam.gameObject.SetActive(true);
                transitionHasBegun = true;
                transitionStart = Time.time;
            }
            else if(Time.time - transitionStart > transitionDuration)
            {
                DisableStartSeq();
            }      
        }
        else
        {
            currentPoint++;
        }

    }

    private void DisableStartSeq()
    {
        this.enabled = false;
        //cutSceneCam.gameObject.SetActive(false);
        blackCam.gameObject.SetActive(false);
        footStepSound.forceSound = false;
        blackEdgesEffect.SetActive(false);
        GetComponent<AudioSource>().Stop();
        playerCam.gameObject.SetActive(true);
        playerStats.SetActive(true);
        inventory.SetActive(true);
        worldMan.enabled = true;
        worldMan.player.InitMessages();
    }
}
