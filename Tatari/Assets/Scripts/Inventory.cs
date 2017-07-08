using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public const int MAX_NR_SCROLLS = 2;

    public GameObject scrollDisplay;
    public bool isReading;
    float readStartTime;
    float readingTime = 5f;

    Scroll.ScrollInfo[] scrolls;
    int currentNrScrolls;
    int selectedScroll;

    int maxNrMatches;
    int currentNrMatches;

    
    void Start()
    {
        scrolls = new Scroll.ScrollInfo[MAX_NR_SCROLLS];
        selectedScroll = 0;
        currentNrScrolls = 0;
        isReading = false;
        maxNrMatches = 3;
        currentNrMatches = maxNrMatches;    
    }

    /* Returns true if the inventory has a match to expand, false otherwise. */
    public bool ExpandMatch()
    {
        if (currentNrMatches != 0)
        {
            currentNrMatches--;
            print("Number of matches left " + currentNrMatches);
            return true;
        }
        return false;
    }

    public void AddScroll(Scroll.ScrollInfo scroll)
    {
        scrolls[currentNrScrolls] = scroll;
        print("Added scroll with index " + currentNrScrolls + " to inventory.");
        currentNrScrolls++;
        selectedScroll = currentNrScrolls - 1;  // Automatically selects the newly added scroll
    }

    public void Browse()
    {
        if (selectedScroll == currentNrScrolls - 1) selectedScroll = 0;
        else selectedScroll++;
        print("Scroll nr " + selectedScroll + " selected.");
    }

    /* Returns the content of the currently selected scrolls. If inventory has no scrolls
     an empty string is returned. */
    public string GetScrollContent()
    {
        if (currentNrScrolls == 0) return "";

        return scrolls[selectedScroll].content;
    }

    public void ReadSelected()
    {
        if (currentNrScrolls == 0) return;  // No scroll to read

        if (!isReading)
        {
            Scroll.ScrollInfo info = scrolls[selectedScroll];
            print("Scroll: Color " + info.color + " Content " + info.content);

            scrollDisplay.SetActive(true);

            string displayText = "Color: " + info.color + "\n Symptoms: " + info.content;
            scrollDisplay.transform.Find("Scroll Content").GetComponent<Text>().text = displayText;

            isReading = true;
            readStartTime = Time.time;
        }
        else
        {
            if(Time.time - readStartTime > readingTime)
            {
                isReading = false;
                scrollDisplay.SetActive(false);
            }
        }
        /*else
        {
            scrollDisplay.SetActive(false);
            isReading = false;
        }*/
    }
}