using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSeq : MonoBehaviour {

    public WorldManager worldMan;

    public Camera cutSceneCam;
    public Camera playerCam;
    public Camera blackCam;
    public FootSteps footStepSound;
    public GameObject blackEdgesEffect;
    public GameObject playerStats;
    public GameObject inventory;
    public RainZone rainZone;
    public InfoBox infoBox;
    public Transform[] path;
    public int nrPoints;
    int currentPoint;
    float speed = 1.25f;

    bool transitionHasBegun;
    float transitionDuration = 1f;
    float transitionTime;
    float transitionStart;


    void Start () {
        cutSceneCam.gameObject.SetActive(true);
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
        rainZone.UpdateSystem(19);  // The closest world area where the intro sequence take place 
        GetComponent<AudioSource>().Play();
    }
	
	void Update ()
    { 
        if (cutSceneCam.transform.position != path[currentPoint].position)
        {
            cutSceneCam.transform.position = Vector3.MoveTowards(cutSceneCam.transform.position,
                                                                    path[currentPoint].position, speed * Time.deltaTime);

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

    void DisableStartSeq()
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
