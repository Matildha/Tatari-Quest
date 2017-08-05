using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MenuController {

    public GameObject hintToggleIcon;
    public GameObject diffLvlIndicator;
    string[] diffLvls = {"Noob", "Normal", "Nightmare"};
    int diffLvl;
    public Sprite hintTrue;
    public Sprite hintFalse;
    bool hintInfo;

	public void StartMenuInit () {
        Init();

        actions = new List<System.Func<int>>();
        actions.Add(StartGame);
        actions.Add(ToggleHints);
        actions.Add(ChangeDiffLvl);
        actions.Add(Exit);

        hintToggleIcon.GetComponent<Image>().sprite = hintTrue;
        hintInfo = GameController.instance.hintInfo = true;

        diffLvl = 1;
        diffLvlIndicator.GetComponent<Text>().text = diffLvls[diffLvl];
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
}
