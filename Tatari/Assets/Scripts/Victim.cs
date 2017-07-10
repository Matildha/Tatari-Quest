using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victim : Interactable {

    public GameObject victimInfoBox;

    public override string PromptMessage { get { return PROMPT_MSG; } }
    public string symptom;

    const float DECREASE_FEAR = -20;

    float symptDisplayStart;
    const float SYMPT_DISP_TIME = 2;
    bool symptDisplay;

    const string PROMPT_MSG = "Press E to rescue person";

    
    public override void Interact()
    {
        //print("SAVE MEE~~~~");
        victimInfoBox.SetActive(true);
        victimInfoBox.GetComponent<Text>().text = "Victim symptoms:\n" + symptom;
        symptDisplay = true;
        symptDisplayStart = Time.time;
    }

    public void Rescue(string curedSymptom)
    {
        if (curedSymptom == symptom)
        {
            print("Succesfully rescued victim!");
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
        }
    }

    void Update()
    {
        if(symptDisplay && Time.time - symptDisplayStart > SYMPT_DISP_TIME )
        {
            symptDisplay = false;
            victimInfoBox.SetActive(false);
        }
    }

}
