using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PauseScreen inherits from MenuController and can be toggled by 
 * Pause() and UnPause(). A call to Continue() is required to actually
 * UnPause() through GameController. This function is associated with 
 * a menu item. 
 * 
 * PauseScreen can also toggle a help display (gameObject "helpInfo"). 
*/

public class PauseScreen : MenuController {

    public GameObject PauseMenu;
    public GameObject menu;
    public GameObject background;
    public GameObject helpInfo;
    bool helpIsDisplay;


	private void Start () {
        Init();
        helpIsDisplay = false;

        actions = new List<System.Func<int>>();
        actions.Add(Continue);
        actions.Add(ToggleHelp);
        actions.Add(Exit);
    }
	
	private int Continue()
    {
        Deselect();
        GameController.instance.UnPause();
        return 0;
    }

    private int ToggleHelp()
    {
        helpIsDisplay = !helpIsDisplay;
        menu.SetActive(!helpIsDisplay);
        helpInfo.SetActive(helpIsDisplay);
        Deselect();
        //if (helpIsDisplay) Init();  // Reset menu when returning to it
        return 0;
    }

    private void Update()
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
        background.SetActive(true);
        this.enabled = true;     
    }

    public void UnPause()
    {
        PauseMenu.SetActive(false);
        background.SetActive(false);
        this.enabled = false;
    }
}
