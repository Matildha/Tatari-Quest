using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : Interactable {

    public InfoBox infoBox;
    Animator victimAnimCon;

    public override string PromptMessage { get { return PROMPT_MSG; } }
    public string symptom;

    static int[] decreaseFearLvls = { -30, -25, -20};

    float symptDisplayStart;
    const float SYMPT_DISP_TIME = 2;
    float bowAnimDuration;
    float bowAnimStart;
    bool symptDisplay;
    //bool isDying;

    const string PROMPT_MSG = "Press 'E' to view curse symptoms\nor 'R' to chant from scroll";
    const string SYMP_INFO = "Symptoms:\n";

    const string RESCUE_SUCCESS = "You succesfully cured the victim from the curse!";
    const string RESCUE_FAIL = "This chant could not cure the victim.";

    int areaID;


    private void Start()
    {
        victimAnimCon = GetComponent<Animator>();
        isDying = false;
        symptDisplay = false;

        foreach(AnimationClip clip in victimAnimCon.runtimeAnimatorController.animationClips)
        {
            if(clip.name == "Armature|Bow")
            {
                bowAnimDuration = clip.length;
                break;
            }
        }

        //print("Victim fear decr " + decreaseFearLvls[GameController.instance.diffLvl]);
    }

    public override void Interact()
    {
        if (!isDying)
        {
            string[] msg = { SYMP_INFO + symptom };
            infoBox.DisplayInfo(msg);
            areaID = GameController.instance.worldMan.currentWorldArea;
            print("Area id stored in victim " + areaID);
        }
    }

    public void Rescue(string curedSymptom)
    {
        if (curedSymptom == symptom)
        {
            print("Succesfully rescued victim!");

            string[] msg = { RESCUE_SUCCESS };
            infoBox.DisplayInfo(msg);

            /*Player player = GameObject.Find("Player").GetComponent<Player>();
            player.IncreaseNrRescues();
            player.fearMeter.ChangeFear((float) decreaseFearLvls[GameController.instance.diffLvl]); */

            victimAnimCon.SetTrigger("makeBow");
            bowAnimStart = Time.time;

            isDying = keepMeEnabled = true;
            InteractableManager intManager = GameObject.Find("Interactables").GetComponent<InteractableManager>();
            intManager.RemoveInteractable(this);
            intManager.ResetInRangeInteractable();
        }
        else
        {
            print("The chant did not cure this person!");
           // victimAnimCon.SetTrigger("makeBow");
            //bowAnimStart = Time.time;
            //isDying = keepMeEnabled = true;
            string[] msg = { RESCUE_FAIL };
            infoBox.DisplayInfo(msg);
            /*InteractableManager intManager = GameObject.Find("Interactables").GetComponent<InteractableManager>();  // FOR DEBUG!!
            intManager.RemoveInteractable(this);
            intManager.ResetInRangeInteractable();*/
        }
    }

    void Update()
    {
        if(isDying && Time.time - bowAnimStart > bowAnimDuration)
        {
            print("Victim to be removed");
            keepMeEnabled = false;

            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.IncreaseNrRescues();
            player.fearMeter.ChangeFear((float)decreaseFearLvls[GameController.instance.diffLvl]);

            Destroy(this.gameObject);
        }
    }
}
