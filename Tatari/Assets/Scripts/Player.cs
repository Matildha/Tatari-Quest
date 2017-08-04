using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public PlayerController playerController;
    public InteractableManager intMan;
    //public GameObject ingameUI;
    public Inventory inventory;
    public FearMeter fearMeter;
    public GameObject victimStatIndicator;
    public InfoBox infoBox;
    const string VICTIM_INDICATOR_TEXT = "Victims saved: ";
    public Lantern lantern;

    public int currentArea;

    public bool hintInfo;

    public bool hasEncounteredVictim;
    public int demonsEncountered;
    public bool hasFoundScroll;

    public int nrRescuedVictims;
    public int gamePlayStartTime;

    bool normalReading;
    bool cursorToggle;
    

    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        gamePlayStartTime = (int) Time.time;
        nrRescuedVictims = 0;
        hasEncounteredVictim = false;
        victimStatIndicator.GetComponent<Text>().text = VICTIM_INDICATOR_TEXT + nrRescuedVictims
                                                               + " / " + VictimFactory.maxVictims[GameController.instance.diffLvl];
        demonsEncountered = 0;
        hintInfo = GameController.instance.hintInfo;
        infoBox.Init();
        string[] message = { "Press 'Space' to toggle light." };
        if (hintInfo) infoBox.DisplayInfo(message);
    }

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

    public void SwitchArea(int area)
    {
        currentArea = area;
        gameObject.GetComponent<FootSteps>().currentArea = area;
    }

    public void GameOver()
    {
        /*GameController.instance.nrRescuedVictims = nrRescuedVictims;
        GameController.instance.gameplayTime = (int) Time.time - gamePlayStartTime;
        GameController.instance.gameSuccess = nrRescuedVictims == VictimFactory.MAX_VICTIMS;
        GameController.instance.SwitchScene(GameController.GAME_OVER);*/
        GameController.instance.GameOver();
    }

    public void ExUpdate() {

        playerController.ExUpdate();
 
        if (inventory.isReading || (Input.GetKeyDown(KeyCode.R) && lantern.isLit))
        {
            if (!normalReading && intMan.inRangeInteractable != null && intMan.inRangeInteractable.tag == "Victim") {
                inventory.ReadSelected("Chant");
                intMan.ToggleInteractionPrompt(false);
                // Rescue attempt when ReadSelected() sets isReading to false
                if (!inventory.isReading) ((Victim)intMan.inRangeInteractable).Rescue(inventory.GetScrollContent());
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
            //intMan.ToggleInteractionPrompt(false);
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

    public void FirstVictimEncounter()
    {
        //if (!hintInfo) return;

        string[] messages;
        if (demonsEncountered == 0)
        {
            messages = new string[]{" I'M SORRY FOR INTRUDING", "I'M JUST TAKING SHELTER FROM THE STORM...",
                                    "... HEY, ARE YOU OKAY?"};
            
        }
        else
        {
            messages = new string[]{" I'M SORRY FOR INTRUDING", "I'M JUST TAKING SHELTER FROM THE STORM...",
                                    "... HEY, ARE YOU OKAY?", "... THE THING I SAW BEFORE... DID IT DO THIS TO YOU?"};
        }
        infoBox.DisplayInfo(messages);
        hasEncounteredVictim = true;
    }

    public void DemonEncounter()
    {
        //if (!hintInfo) return;

        if (demonsEncountered == 1)
        {
            string[] message = { " 'WHAT THE HELL WAS THAT?!' " };
            infoBox.DisplayInfo(message);
        }
        else if (demonsEncountered == 2)
        {
            string[] messages;

            if (hasEncounteredVictim)
            {
                messages = new string[]{ " 'ANOTHER ONE..!' ", " ' ARE THOSE THINGS ATTRACTED TO LIGHT? ' ",
                                    " ' ... I SHOULDN'T KEEP THE LIGHT ON FOR TOO LONG' ",
                                    " 'THE PERSON FROM BEFORE... DID THEY GET ATTACKED TOO?' "};
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
}