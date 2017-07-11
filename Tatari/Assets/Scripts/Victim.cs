using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : Interactable {

    public InfoBox infoBox;

    public override string PromptMessage { get { return PROMPT_MSG; } }
    public string symptom;

    const float DECREASE_FEAR = -20;

    float symptDisplayStart;
    const float SYMPT_DISP_TIME = 2;
    bool symptDisplay;

    const string PROMPT_MSG = "Press E to view symptoms\nor R to chant from scroll";
    const string SYMP_INFO = "Symptoms:\n";

    const string RESCUE_SUCCESS = "You succesfully cured the victim from the curse!";
    const string RESCUE_FAIL = "This chant could not cure the victim.";


    private void Start()
    {
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
            intManager.RemoveInteractable(this);  // So this victim is not updated in InteractableManager
            intManager.ResetInRangeInteractable();

            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.IncreaseNrRescues();
            player.fearMeter.ChangeFear(DECREASE_FEAR);

            Destroy(this.gameObject);        
        }
        else
        {
            print("The chant did not cure this person!");
            infoBox.DisplayInfo(RESCUE_FAIL);
        }
    }

    /*void Update()
    {
        if(symptDisplay && Time.time - symptDisplayStart > SYMPT_DISP_TIME )
        {
            symptDisplay = false;
            victimInfoBox.SetActive(false);
        }
    }*/

}
