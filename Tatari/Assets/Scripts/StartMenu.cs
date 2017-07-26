using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MenuController {

    public GameObject hintToggleIcon;
    public Sprite hintTrue;
    public Sprite hintFalse;
    bool hintInfo;

	public void StartMenuInit () {
        Init();

        nrMenuItems = 3;
        actions = new List<System.Func<int>>();
        actions.Add(StartGame);
        actions.Add(ToggleHints);
        actions.Add(Exit);

        hintToggleIcon.GetComponent<Image>().sprite = hintTrue;
        hintInfo = GameController.instance.hintInfo = true;
    }
	
    int StartGame()
    {
        //TODO add intro!!!!! 
        Restart();
        return 0;
    }

    int ToggleHints()
    {
        hintInfo = GameController.instance.hintInfo = !hintInfo;
        hintToggleIcon.GetComponent<Image>().sprite = hintInfo ? hintTrue : hintFalse;
        DeSelect();
        return 0;
    }
}
