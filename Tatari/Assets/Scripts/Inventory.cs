using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public const int MAX_NR_SCROLLS = 2;

    public GameObject scrollDisplay;

    Scroll.ScrollInfo[] scrolls;
    int currentNrScrolls;

    int selectedScroll;

    bool scrollIsDisplaying;

    void Start()
    {
        scrolls = new Scroll.ScrollInfo[MAX_NR_SCROLLS];
        selectedScroll = 0;
        currentNrScrolls = 0;

        scrollIsDisplaying = false;
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

    public void ReadSelected()
    {
        if (!scrollIsDisplaying)
        {
            Scroll.ScrollInfo info = scrolls[selectedScroll];
            print("Scroll: Color " + info.color + " Content " + info.content);

            scrollDisplay.SetActive(true);

            string displayText = "Color: " + info.color + "\n Symptoms: " + info.content;
            scrollDisplay.transform.Find("Scroll Content").GetComponent<Text>().text = displayText;

            scrollIsDisplaying = true;
        }
        else
        {
            scrollDisplay.SetActive(false);
            scrollIsDisplaying = false;
        }
    }
}