using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MenuController {

    public GameObject PauseMenu;
    public GameObject menu;
    public GameObject helpInfo;
    bool helpIsDisplay;

	void Start () {
        helpIsDisplay = false;

        actions = new List<System.Func<int>>();
        actions.Add(Continue);
        actions.Add(ToggleHelp);
        actions.Add(Exit);

    }
	
	int Continue()
    {
        GameController.instance.UnPause();
        return 0;
    }

    int ToggleHelp()
    {
        helpIsDisplay = !helpIsDisplay;
        menu.SetActive(!helpIsDisplay);
        helpInfo.SetActive(helpIsDisplay);
        if (helpIsDisplay) Init();  // Reset menu when returning to it
        return 0;
    }


    void Update()
    {
        if (helpIsDisplay)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                menu.SetActive(true);
                helpInfo.SetActive(false);
                ToggleHelp();
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

    public void Pause()
    {
        PauseMenu.SetActive(true);
        this.enabled = true;     
    }

    public void UnPause()
    {
        PauseMenu.SetActive(false);
        this.enabled = false;
    }

}
