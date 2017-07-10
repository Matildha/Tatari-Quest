using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public const int MAX_NR_SCROLLS = 2;

    public GameObject scrollDisplay;
    public GameObject readChannelBarFill;
    float readBarHeight;
    float readBarWidth;
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
        readBarHeight = readChannelBarFill.GetComponent<RectTransform>().rect.height;
        readBarWidth = readChannelBarFill.GetComponent<RectTransform>().rect.width;
        scrollDisplay.SetActive(false);
        readChannelBarFill.transform.parent.gameObject.SetActive(false);
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

    /* Needs to be called every frame to update read channel bar and to determine when channel time has ended. */
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

            readChannelBarFill.SetActive(true);
            readChannelBarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(0, readBarHeight);

            isReading = true;
            readStartTime = Time.time;
        }
        else
        {
            float deltaTime = Time.time - readStartTime;
            float newWidth = (deltaTime / readingTime) * readBarWidth;
            readChannelBarFill.transform.parent.gameObject.SetActive(true);
            readChannelBarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, readBarHeight);

            if (deltaTime > readingTime)
            {
                isReading = false;
                scrollDisplay.SetActive(false);
                readChannelBarFill.transform.parent.gameObject.SetActive(false);
            }
        }
        /*else
        {
            scrollDisplay.SetActive(false);
            isReading = false;
        }*/
    }
}