using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Victim inherits from Interactable. Its Interact() implementaion
 * causes a the "symptom" to be displayed using "infoBox". 
 * 
 * At a succesfull call on Rescue() the current instance will trigger
 * an animation and then destroy the gameObject this script is attached to.
 * 
 * Player's fear and number of rescued victims statistic is updated right before the destruction. 
*/

public class Victim : Interactable {

    public InfoBox infoBox;

    public override string PromptMessage { get { return PROMPT_MSG; } }
    public string symptom;

    static int[] decreaseFearLvls = { -30, -25, -20};

    Animator victimAnimCon;
    float bowAnimDuration;
    float bowAnimStart;

    bool symptDisplay;

    const string PROMPT_MSG = "Press 'E' to view curse symptoms\nor 'R' to chant from scroll";
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
        //print("Victim fear decr " + decreaseFearLvls[GameController.instance.diffLvl]);
    }

    public override void Interact()
    {
        if (!isDying)
        {
            string[] msg = { SYMP_INFO + symptom };
            infoBox.DisplayInfo(msg);
        }
    }

    /* Sets isDying and the trigger "makeBow" of the Animator attached to the same gameObject as this script, 
     if the given string matches "symptom". At a match this Victim instance is removed from InteractableManager. */
    public void Rescue(string curedSymptom)
    {
        if (curedSymptom == symptom)
        {
            print("Succesfully rescued victim!");

            string[] msg = { RESCUE_SUCCESS };
            infoBox.DisplayInfo(msg);

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
           // victimAnimCon.SetTrigger("makeBow");  // FOR DEBUG! 
            //bowAnimStart = Time.time;
            //isDying = keepMeEnabled = true;
            string[] msg = { RESCUE_FAIL };
            infoBox.DisplayInfo(msg);
            /*InteractableManager intManager = GameObject.Find("Interactables").GetComponent<InteractableManager>();  // FOR DEBUG!
            intManager.RemoveInteractable(this);
            intManager.ResetInRangeInteractable();*/
        }
    }

    /* Checks if "isDying" is true and if so waits for bow animation to finish. When the animation 
     finishes the gameObject this script is attached to will be destroyed. */
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
