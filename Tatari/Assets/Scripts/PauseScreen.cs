using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MenuController {

    public GameObject menu;
    public GameObject helpInfo;
    bool helpIsDisplay;

	void Start () {
        helpIsDisplay = false;

        actions = new List<System.Func<int>>();
        actions.Add(Continue);
        actions.Add(Help);
        actions.Add(Exit);

    }
	
	int Continue()
    {
        GameController.instance.UnPause();
        return 0;
    }

    int Help()
    {
        menu.SetActive(false);
        helpInfo.SetActive(true);
        return 0;
    }

    void Update()
    {
        if(helpIsDisplay || Input.GetKeyDown(KeyCode.Q))
        {
            menu.SetActive(true);
            helpInfo.SetActive(false);
        }
    }
}
