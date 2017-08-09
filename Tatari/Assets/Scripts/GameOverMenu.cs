using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * GameOverMenu inherits from MenuController. 
 * 
 * The displayed content depends on the GameController boolean "gameSuccess". 
*/ 

public class GameOverMenu : MenuController {

    public GameObject victimStat;
    public GameObject timeStat;
    public GameObject gameOverText;
    public GameObject horrayText;
    public GameObject resultText;

    const string POS_RESULT_MSG = "Yokatta! You rescued all victims from the demons.";
    const string NEG_RESULT_MSG = "Curse you coward. Fleeing to save only yourself!";
    const string VICTIM_STAT_TEXT = "Victims rescued: ";
    const string TIME_STAT_TEXT = "Time: ";
    

	private void Start () {
        Init();

        victimStat.SetActive(true);
        victimStat.GetComponent<Text>().text = VICTIM_STAT_TEXT + GameController.instance.nrRescuedVictims;
        actions = new List<System.Func<int>>();
        actions.Add(Restart);
        actions.Add(LoadStartMenu);
        actions.Add(Exit);

        if(GameController.instance.gameSuccess)
        {
            gameOverText.SetActive(false);
            horrayText.SetActive(true);
            timeStat.SetActive(true);
            resultText.GetComponent<Text>().text = POS_RESULT_MSG;
            int timeMin = GameController.instance.gameplayTime / 60;
            int timeSec = GameController.instance.gameplayTime % 60;
            timeStat.GetComponent<Text>().text = TIME_STAT_TEXT + timeMin + " min " + timeSec + " sec";
        }
        else
        {
            gameOverText.SetActive(true);
            horrayText.SetActive(false);
            timeStat.SetActive(false);
            resultText.GetComponent<Text>().text = NEG_RESULT_MSG;
        }
	}

    private int LoadStartMenu()
    {
        GameController.instance.LoadStartMenu();
        return 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) 
                                            || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Next();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)  
                                            || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Prev();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            Select();
        }
    }
}
