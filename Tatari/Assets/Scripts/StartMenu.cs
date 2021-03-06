﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * StartMenu inherits from MenuController with added functionality to
 * toggle "hintInfo" and choose difficult level. It manages all
 * GUI gameObjects associated with the START scene. 
 * 
 * StartMenu can also display and scroll through "creditsDisplay", which
 * contents are added by StartMenu from TextAsset "creditFile". 
*/

public class StartMenu : MenuController {

    public GameObject menu;
    public GameObject creditsDisplay;
    public GameObject creditsTextBox;
    public GameObject hintToggleIcon;
    public GameObject diffLvlIndicator;
    public Sprite hintTrue;
    public Sprite hintFalse;
    public TextAsset creditFile;

    string[] diffLvls = {"Noob", "Normal", "Nightmare"};
    int diffLvl;

    bool hintInfo;
    bool displayCredits;
    float minCreditPos;
    float maxCreditPos;


    float textMoveY; 

	public void StartMenuInit () {
        Init();

        actions = new List<System.Func<int>>();
        actions.Add(StartGame);
        actions.Add(ToggleHints);
        actions.Add(ChangeDiffLvl);
        actions.Add(ToggleCreditsDisplay);
        actions.Add(Exit);

        hintToggleIcon.GetComponent<Image>().sprite = hintTrue;
        hintInfo = GameController.instance.hintInfo = true;

        diffLvl = 1;
        diffLvlIndicator.GetComponent<Text>().text = diffLvls[diffLvl];

        menu.SetActive(true);
        creditsDisplay.SetActive(false);

        minCreditPos = creditsTextBox.transform.position.y;
        Vector3[] corners = new Vector3[4];
        creditsTextBox.GetComponent<RectTransform>().GetWorldCorners(corners);
        float boxHeight = corners[1].y - corners[0].y;
        maxCreditPos = minCreditPos + boxHeight / 2;  // Subtract margin

        creditsTextBox.GetComponent<Text>().text = creditFile.text;

        textMoveY = boxHeight * 0.01f;
        print("box height " + boxHeight);
    }

    /* Handle user input depending on whether the displayCredits is set or not. */
    void Update()
    {
        if (displayCredits)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                ToggleCreditsDisplay();
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                creditsTextBox.transform.position += new Vector3(0, -textMoveY, 0);

                if (creditsTextBox.transform.position.y < minCreditPos)
                {
                    Vector3 pos = creditsTextBox.transform.position;
                    creditsTextBox.transform.position = new Vector3(pos.x, minCreditPos, pos.z);
                }
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                creditsTextBox.transform.position += new Vector3(0, textMoveY, 0);

                if (creditsTextBox.transform.position.y > maxCreditPos)
                {
                    Vector3 pos = creditsTextBox.transform.position;
                    creditsTextBox.transform.position = new Vector3(pos.x, maxCreditPos, pos.z);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Next();
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Prev();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                Select();
            }
        }
    }

    int ToggleHints()
    {
        hintInfo = GameController.instance.hintInfo = !hintInfo;
        hintToggleIcon.GetComponent<Image>().sprite = hintInfo ? hintTrue : hintFalse;
        Deselect();
        return 0;
    }

    int ChangeDiffLvl()
    {
        diffLvl++;
        if (diffLvl > GameController.MAX_DIFF_LVL) diffLvl = 0;
        GameController.instance.diffLvl = diffLvl;
        diffLvlIndicator.GetComponent<Text>().text = diffLvls[diffLvl];
        Deselect();
        return 0;
    }

    int ToggleCreditsDisplay()
    {
        displayCredits = !displayCredits;
        menu.SetActive(!displayCredits);
        creditsDisplay.SetActive(displayCredits);
        Deselect();
        
        return 0;
    }
}
