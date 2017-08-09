using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Player manages user input that does not have to do with movement of the player character. 
 * It contains references to player associated objects such as PlayerController,
 * Inventory, FearMeter and Lantern. 
 * 
 * Player together with PlayerController and PlayerCollider are the central pieces
 * making up the behaviour of the player character. 
 * 
 * Player keeps track of number of rescued victims and the start game time and
 * controls the GUI component "victimStatIndicator". Player will also use its InfoBox
 * reference to display messages during game play. 
 * 
 * Player is responsible for updating PlayerController. 
*/

public class Player : MonoBehaviour {

    public PlayerController playerController;
    public InteractableManager intMan;
    public Lantern lantern;
    public Inventory inventory;
    public FearMeter fearMeter;
    public GameObject victimStatIndicator;
    public InfoBox infoBox;

    const string VICTIM_INDICATOR_TEXT = "Victims saved: ";

    public int currentArea;

    public bool hintInfo;

    public bool hasEncounteredVictim;
    public int demonsEncountered;
    public bool hasFoundScroll;

    public int nrRescuedVictims;
    public int gamePlayStartTime;

    bool normalReading;  // is the reading of type "read" or "chant"?
    

    private void Start () {
        gamePlayStartTime = (int) Time.time;
        nrRescuedVictims = 0;
        hasEncounteredVictim = false;
        victimStatIndicator.GetComponent<Text>().text = VICTIM_INDICATOR_TEXT + nrRescuedVictims
                                                               + " / " + VictimFactory.maxVictims[GameController.instance.diffLvl];
        demonsEncountered = 0;
        hintInfo = GameController.instance.hintInfo;
        infoBox.Init();
        infoBox.Close();

        if (!GameController.instance.showIntro) InitMessages();  // If the InitMessages() has not already been called by InitSeq
    }

    /* Increases the number of victims rescued and the GUI stat indicator. If this number
     matches the max number of victims game over will be initiated. */
    public void IncreaseNrRescues()
    {
        nrRescuedVictims++;
        victimStatIndicator.GetComponent<Text>().text = VICTIM_INDICATOR_TEXT + nrRescuedVictims 
                                                                + " / " + VictimFactory.maxVictims[GameController.instance.diffLvl];
        if (nrRescuedVictims == VictimFactory.maxVictims[GameController.instance.diffLvl])
        {
            print("You rescued all victims!!! Yokatta!!");
            GameOver();
        }
    }

    /* Updated player currentArea varable and passes this on to child "Footstep Sound" of 
     the gameObject this script is attached to. */
    public void SwitchArea(int area)
    {
        currentArea = area;
        transform.Find("Footstep Sound").GetComponent<FootSteps>().currentArea = area;
    }

    public void GameOver()
    {
        GameController.instance.GameOver();
    }

    /* Updates PlayerController and handles other non-movement related input. If player is reading
     (inventory.isReading == true) no other input will be accepted and instead inventory.ReadSelected()
     will be called until finished reading. */
    public void ExUpdate() {

        playerController.ExUpdate();
 
        if (inventory.isReading || (Input.GetKeyDown(KeyCode.R) && lantern.isLit))
        {
            if (!normalReading && intMan.inRangeInteractable != null && intMan.inRangeInteractable.tag == "Victim") {
                if (!inventory.isReading)
                {
                    transform.Find("Chant Sound").GetComponent<AudioSource>().Play();  // When we start reading
                    intMan.ToggleInteractionPrompt(false);
                }
                inventory.ReadSelected("Chant");  // Sets inventory.isReading to false, if we just finished reading            
                // Rescue attempt when ReadSelected() sets isReading to false
                if (!inventory.isReading)
                {
                    ((Victim)intMan.inRangeInteractable).Rescue(inventory.GetScrollContent());
                    transform.Find("Chant Sound").GetComponent<AudioSource>().Stop();
                }
            }
            else
            {
                inventory.ReadSelected("Read");
                if (!inventory.isReading) normalReading = false;
                else normalReading = true;
            }

            playerController.unableToMove = inventory.isReading;
        }
        else if (Input.GetKeyDown(KeyCode.E) && (intMan.inRangeInteractable != null))
        {
            intMan.inRangeInteractable.Interact();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory.Browse();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lantern.isLit || inventory.ExpandMatch())
                lantern.ToggleLight();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            infoBox.Continue();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Z))
        {
            print("Pause input in player");
            GameController.instance.Pause();
        }
    }

    public void ExFixedUpdate()
    {
        playerController.ExFixedUpdate();
    }


    /* -- InfoBox messages from player character -- */


    public void FirstVictimEncounter()
    {
        string[] messages;
        if (demonsEncountered == 0)
        {
            messages = new string[]{" 'I'M SORRY FOR INTRUDING' ", " 'I'M JUST TAKING SHELTER FROM THE STORM...' ",
                                    " '... HEY, ARE YOU OKAY?' "};
            
        }
        else
        {
            messages = new string[]{" 'I'M SORRY FOR INTRUDING' " , " 'I'M JUST TAKING SHELTER FROM THE STORM...' " ,
                                    " '... HEY, ARE YOU OKAY?' " , " '... THE THING I SAW BEFORE... DID IT DO THIS TO YOU?' "};
        }
        infoBox.DisplayInfo(messages);
        hasEncounteredVictim = true;
    }

    public void DemonEncounter()
    {
        if (demonsEncountered == 1)
        {
            string[] message = { " 'WHAT THE HELL IS THAT?!' " };
            infoBox.DisplayInfo(message);
        }
        else if (demonsEncountered == 2)
        {
            string[] messages;

            if (hasEncounteredVictim)
            {
                messages = new string[]{ " 'ANOTHER ONE..!' ", " ' ARE THOSE THINGS ATTRACTED TO LIGHT? ' ",
                                    " ' ... I SHOULDN'T KEEP THE LIGHT ON FOR TOO LONG' ",
                                    " 'IT MUST HAVE ATTACKED THE PERSON I SAW BEFORE TOO...' "};
            }
            else
            {
                messages = new string[]{ " 'ANOTHER ONE..!' ", " ' ARE THOSE THINGS ATTRACTED TO LIGHT? ' ",
                                    " ' ... I SHOULDN'T KEEP THE LIGHT ON FOR TOO LONG' "};
            }
            infoBox.DisplayInfo(messages);
        }      
    }

    public void FirstScrollEncounter()
    {
        string[] msg = { " 'WHAT'S THIS THING DOING HERE?' " };
        infoBox.DisplayInfo(msg);
        hasFoundScroll = true;
    }

    public void InitMessages()
    {
        if(hintInfo)
        {
            string[] message = { " '... PHEW! SUCH NASTY WEATHER...' ",
                                        " 'EXCUSE ME FOR INTRUDING! IS ANYONE HOME?' ",
                                        "Press 'Space' to toggle light." };
            infoBox.DisplayInfo(message);
        }
        else
        {
            string[] message = { " '... PHEW! SUCH NASTY WEATHER...' ",
                                        " 'EXCUSE ME FOR INTRUDING! IS ANYONE HOME?' "};
            infoBox.DisplayInfo(message);
        }
    }
}
