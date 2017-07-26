﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public PlayerController playerController;
    public InteractableManager intMan;
    public GameObject ingameUI;
    public Inventory inventory;
    public FearMeter fearMeter;
    public GameObject victimStatIndicator;
    public InfoBox infoBox;
    const string VICTIM_INDICATOR_TEXT = "Victims saved: ";
    public Lantern lantern;

    public int currentArea;

    public bool hintInfo;

    int nrRescuedVictims;
    int gamePlayStartTime;

    bool normalReading;
    bool cursorToggle;
    


    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        gamePlayStartTime = (int) Time.time;
        nrRescuedVictims = 0;
        hintInfo = GameController.instance.hintInfo;
    }

    public void IncreaseNrRescues()
    {
        nrRescuedVictims++;
        victimStatIndicator.GetComponent<Text>().text = VICTIM_INDICATOR_TEXT + nrRescuedVictims;
        if(nrRescuedVictims == VictimFactory.MAX_VICTIMS)
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
        GameController.instance.nrRescuedVictims = nrRescuedVictims;
        GameController.instance.gameplayTime = (int) Time.time - gamePlayStartTime;
        GameController.instance.gameSuccess = nrRescuedVictims == VictimFactory.MAX_VICTIMS;
        GameController.instance.SwitchScene(GameController.GAME_OVER);
    }

    public void ExUpdate() {

        playerController.ExUpdate();
 
        if (inventory.isReading || (Input.GetButtonDown("Read") && lantern.isLit))
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
        else if (Input.GetButtonDown("Interact") && (intMan.inRangeInteractable != null))
        {
            intMan.inRangeInteractable.Interact();
            //intMan.ToggleInteractionPrompt(false);
        }
        else if (Input.GetButtonDown("Select"))
        {
            inventory.Browse();
        }
        else if (Input.GetKeyDown("space"))
        {
            if (lantern.isLit || inventory.ExpandMatch())
                lantern.ToggleLight();
        }
        else if (Input.GetButtonDown("Close"))
        {
            infoBox.Close();
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

    void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}