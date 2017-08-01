using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSeq : MonoBehaviour {

    public Camera cutSceneCam;
    public Camera playerCam;
    public GameObject playerStats;
    public GameObject inventory;
    public Transform[] path;
    public int nrPoints;
    public Animator hurtEffectAnim;
    float animStart;
    float animDuration;
    const float ANIM_DUR_SUBTR = 1f;
    int hurtHash = Animator.StringToHash("HurtEffect 0");
    int currentPoint;
    float speed = 2.5f;
    bool hasPlayedSound;

	public void Init () {
        cutSceneCam.gameObject.SetActive(true);
        playerCam.gameObject.SetActive(false);
        cutSceneCam.transform.position = path[0].position;
        currentPoint = 1;
        animDuration = hurtEffectAnim.runtimeAnimatorController.animationClips[0].length - ANIM_DUR_SUBTR;
        playerStats.SetActive(false);
        inventory.SetActive(false);
        hurtEffectAnim.Play("HurtEffect");
        animStart = Time.time;
    }
	
	void Update () {

        if (Time.time - animStart > animDuration) hurtEffectAnim.Play("HurtEffect");

        if(cutSceneCam.transform.position != path[currentPoint].position)
        {
            cutSceneCam.transform.position = Vector3.MoveTowards(cutSceneCam.transform.position, 
                                                                    path[currentPoint].position, speed * Time.deltaTime);

            cutSceneCam.transform.LookAt(path[nrPoints-1], Vector3.up);
        }
        else if(currentPoint == nrPoints -1 ) {
            this.enabled = false;
            GameController.instance.SwitchScene(GameController.GAME_OVER);
        }
        else
        {
            currentPoint++;
        }

        if (!hasPlayedSound)
        {
            GetComponent<AudioSource>().Play();
            hasPlayedSound = true;
        }
		
	}
}
