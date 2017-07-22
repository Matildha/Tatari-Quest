using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : Interactable {

    public InfoBox infoBox;
    Animator victimAnimCon;

    public override string PromptMessage { get { return PROMPT_MSG; } }
    public string symptom;

    const float DECREASE_FEAR = -20;

    float symptDisplayStart;
    const float SYMPT_DISP_TIME = 2;
    float bowAnimDuration;
    float bowAnimStart;
    bool symptDisplay;
    bool isDying;

    const string PROMPT_MSG = "Press E to view symptoms\nor R to chant from scroll";
    const string SYMP_INFO = "Symptoms:\n";

    const string RESCUE_SUCCESS = "You succesfully cured the victim from the curse!";
    const string RESCUE_FAIL = "This chant could not cure the victim.";


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
    }

    public override void Interact()
    {
        infoBox.DisplayInfo(SYMP_INFO + symptom);  
    }

    public void Rescue(string curedSymptom)
    {
        if (curedSymptom == symptom)
        {
            print("Succesfully rescued victim!");

            infoBox.DisplayInfo(RESCUE_SUCCESS);

            InteractableManager intManager = GameObject.Find("Interactables").GetComponent<InteractableManager>();
            intManager.RemoveInteractable(this);  
            intManager.ResetInRangeInteractable();

            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.IncreaseNrRescues();
            player.fearMeter.ChangeFear(DECREASE_FEAR);

            victimAnimCon.SetTrigger("makeBow");
            bowAnimStart = Time.time;

            isDying = true;     
        }
        else
        {
            print("The chant did not cure this person!");
            //victimAnimCon.SetTrigger("makeBow");
            //bowAnimStart = Time.time;
            //isDying = true;
            infoBox.DisplayInfo(RESCUE_FAIL);
        }
    }

    void Update()
    {
        if(isDying && Time.time - bowAnimStart > bowAnimDuration)
        {
            Destroy(this.gameObject);
        }
    }
}
