using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    Scroll.ScrollInfo[] scrolls;
    const int MAX_NR_SCROLLS = 6;
    int currentNrScrolls;

    int selectedScroll;

    void Start()
    {
        scrolls = new Scroll.ScrollInfo[MAX_NR_SCROLLS];
        selectedScroll = 0;
        currentNrScrolls = 0;
    }

    public void AddScroll(Scroll.ScrollInfo scroll)
    {
        scrolls[currentNrScrolls] = scroll;
        print("Added scroll with index " + currentNrScrolls);
        currentNrScrolls++;
    }

    public void Browse()
    {
        if (selectedScroll == currentNrScrolls) selectedScroll = 0;
        else selectedScroll++;
        print("Scroll nr " + selectedScroll + " selected.");
    }
}
