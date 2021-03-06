﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Inventory manages the inventory and reading channel bar GUI components.
 * Inventory also contains informaion about aquired scrolls and number of matches. 
 * 
 * Inventory contains functionality to read and display content from an aquired scroll.
 * It will also display help messages through InfoBox if Player "hintInfo" is set, as well 
 * as messages as reaction to the first scroll reading. 
*/

public class Inventory : MonoBehaviour {

    public const int MAX_NR_SCROLLS = 6;
    const int INVENTORY_SPOTS = 6;  // For debugging when nr scroll less then inventory spots

    public Player player;
    public GameObject scrollDisplay;
    public GameObject readChannelBarFill;
    public InfoBox infoBox;

    public GameObject orgScrollBase;
    public Sprite[] scrollColors;
    public GameObject scrollSelect;
    float scrollVertOffset;
    float scrollHoriOffset;

    public GameObject matchCount;
    const string MATCH_TEXT = "X ";

    float readBarHeight;
    float readBarWidth;
    public bool isReading;
    float readStartTime;
    float readingTime = 5f;
    float chantingTime = 8f;
    bool hasReadFirst;

    GameObject[] scrollIcons;
    Scroll.ScrollInfo[] scrolls;
    int currentNrScrolls;
    int selectedScroll;

    int[] maxNrMatches = { 9, 7, 5 };
    int currentNrMatches;

    
    private void Start()
    {
        scrolls = new Scroll.ScrollInfo[MAX_NR_SCROLLS];
        scrollIcons = new GameObject[MAX_NR_SCROLLS];
        scrollIcons[0] = orgScrollBase;
        selectedScroll = 0;
        currentNrScrolls = 0;
        isReading = false;
        currentNrMatches = maxNrMatches[GameController.instance.diffLvl];
        readBarHeight = readChannelBarFill.GetComponent<RectTransform>().rect.height;
        readBarWidth = readChannelBarFill.GetComponent<RectTransform>().rect.width;
        scrollDisplay.SetActive(false);
        readChannelBarFill.transform.parent.gameObject.SetActive(false);
        matchCount.SetActive(true);
        matchCount.GetComponent<Text>().text = MATCH_TEXT + currentNrMatches;
        orgScrollBase.SetActive(false);
        scrollSelect.SetActive(false);
        scrollHoriOffset = orgScrollBase.GetComponent<RectTransform>().anchoredPosition.x;
        scrollVertOffset = orgScrollBase.GetComponent<RectTransform>().anchoredPosition.y;
    }

    /* Returns true if the inventory has a match to expand, false otherwise. */
    public bool ExpandMatch()
    {
        if (currentNrMatches != 0)
        {
            currentNrMatches--;
            //print("Number of matches left " + currentNrMatches);
            matchCount.GetComponent<Text>().text = MATCH_TEXT + currentNrMatches;
            return true;
        }
        return false;
    }

    /* Adds the scroll ScrollInfo to Inventory's array of ScrollInfo. 
     Special cases occur when the number of aquired scrolls reach 1 and 2 to display
     messages. The newest added scroll is automatically selected. 
        Selects the correct color sprite according to the given ScrollInfo. */
    public void AddScroll(Scroll.ScrollInfo scroll)
    {
        scrolls[currentNrScrolls] = scroll;
        currentNrScrolls++;
        selectedScroll = currentNrScrolls - 1; 
        
        if(currentNrScrolls == 1)
        {
            // Scroll sprite number 1 does not need to be positioned. 
            orgScrollBase.SetActive(true);
            orgScrollBase.transform.Find("Scroll Color").GetComponent<Image>().sprite = scrollColors[(int)scroll.color];
            scrollSelect.GetComponent<RectTransform>().anchoredPosition = orgScrollBase.GetComponent<RectTransform>().anchoredPosition;
            scrollSelect.SetActive(true);
            if (player.hintInfo)
            {
                //infoBox.gameObject.SetActive(true);
                string[] msg = { "Press 'R' to read scroll.\nYou can only read with light on." };
                infoBox.DisplayInfo(msg);
            }
        }
        else
        {
            if (currentNrScrolls == 2 && player.hintInfo)
            {
                string[] msg = { "Use 'Tab' to browse scrolls" };
                infoBox.DisplayInfo(msg);
            }

            // Position new scroll sprite
            GameObject newScroll = Instantiate(orgScrollBase) as GameObject;
            newScroll.transform.Find("Scroll Color").GetComponent<Image>().sprite = scrollColors[(int) scroll.color];
            newScroll.transform.SetParent(this.transform.Find("Scroll Holder").transform);
            newScroll.GetComponent<RectTransform>().anchorMax = orgScrollBase.GetComponent<RectTransform>().anchorMax;
            newScroll.GetComponent<RectTransform>().anchorMin = orgScrollBase.GetComponent<RectTransform>().anchorMin;
            scrollIcons[currentNrScrolls - 1] = newScroll;
            if (currentNrScrolls == 5) scrollVertOffset = -scrollVertOffset;
            newScroll.GetComponent<RectTransform>().anchoredPosition = new Vector3(
                                                            (currentNrScrolls % 2 == 0) ? -scrollHoriOffset : scrollHoriOffset,
                                                            currentNrScrolls == 3 || currentNrScrolls == 4 ? 0 :
                                                            scrollVertOffset,  
                                                            0);
            newScroll.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            scrollSelect.GetComponent<RectTransform>().anchoredPosition = newScroll.GetComponent<RectTransform>().anchoredPosition;
            newScroll.SetActive(true);          
        }
        if(player.lantern.isLit)
            ReadSelected("Read");
    }

    /* Increments selectedScroll and moves the selection sprite. selectedScroll is set
     to 0 if the number exceeds the max index. */
    public void Browse()
    {
        if (selectedScroll == currentNrScrolls - 1) selectedScroll = 0;
        else selectedScroll++;
        scrollSelect.GetComponent<RectTransform>().anchoredPosition = scrollIcons[selectedScroll].
                                                                        GetComponent<RectTransform>().anchoredPosition;
    }

    /* Returns the content of the currently selected scrolls. If inventory has no scrolls
     an empty string is returned. */
    public string GetScrollContent()
    {
        if (currentNrScrolls == 0) return "";

        return scrolls[selectedScroll].content;
    }

    /* Given the reading type sets a reading time during the contents of the currently selected scroll 
     will be displayed together with a readin channel bar to indicate the time passed. 
     Needs to be called every frame to update read channel bar and to determine when channel time has ended. */
    public void ReadSelected(string type)
    {
        float readDuration;
        if (type == "Read")
        {
            readDuration = readingTime;
        }
        else if (type == "Chant")
        {
            readDuration = chantingTime;
        }
        else return;

        if (currentNrScrolls == 0) return;  // No scroll to read

        if (!isReading)
        {
            Scroll.ScrollInfo info = scrolls[selectedScroll];
            //print("Scroll: Color " + info.color + " Content " + info.content);

            scrollDisplay.SetActive(true);
            string displayText = "Symptoms:\n" + info.content;
            scrollDisplay.transform.Find("Scroll Content").GetComponent<Text>().text = displayText;

            readChannelBarFill.SetActive(true);
            readChannelBarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(0, readBarHeight);

            isReading = true;
            readStartTime = Time.time;
        }
        else
        {
            float deltaTime = Time.time - readStartTime;
            float newWidth = (deltaTime / readDuration) * readBarWidth;
            readChannelBarFill.transform.parent.gameObject.SetActive(true);
            readChannelBarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, readBarHeight);

            if (deltaTime > readDuration)
            {
                isReading = false;
                scrollDisplay.SetActive(false);
                readChannelBarFill.transform.parent.gameObject.SetActive(false);

                if(!hasReadFirst)
                {
                    string[] messages = { " 'THIS SEEMS TO BE SOME KIND OF CHANT...' " };                                                
                    infoBox.DisplayInfo(messages);
                    hasReadFirst = true;
                }
            }
        }
    }

    public void CloseReadDisplay()
    {
        scrollDisplay.SetActive(false);
        readChannelBarFill.transform.parent.gameObject.SetActive(false);
    }
}
