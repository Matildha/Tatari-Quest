using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MenuController {

    public GameObject victimStat;
    public GameObject timeStat;
    public GameObject gameOverText;
    public GameObject horrayText;
    public GameObject resultText;
    const string posResultMsg = "Yokatta! You rescued all victims from the demons.";
    const string negResultMsg = "Curse you coward, fleeing to save only yourself!";
    const string VICTIM_STAT_TEXT = "Victims rescued: ";
    const string TIME_STAT_TEXT = "Time: ";
    

	void Start () {
        Init();

        victimStat.SetActive(true);
        victimStat.GetComponent<Text>().text = VICTIM_STAT_TEXT + GameController.instance.nrRescuedVictims;
        nrMenuItems = 2;
        actions = new List<System.Func<int>>();
        actions.Add(Restart);
        actions.Add(Exit);

        if(GameController.instance.gameSuccess)
        {
            gameOverText.SetActive(false);
            horrayText.SetActive(true);
            timeStat.SetActive(true);
            resultText.GetComponent<Text>().text = posResultMsg;
            int timeMin = GameController.instance.gameplayTime / 60;
            int timeSec = GameController.instance.gameplayTime % 60;
            timeStat.GetComponent<Text>().text = TIME_STAT_TEXT + timeMin + " min " + timeSec + " sec";
        }
        else
        {
            gameOverText.SetActive(true);
            horrayText.SetActive(false);
            timeStat.SetActive(false);
            resultText.GetComponent<Text>().text = negResultMsg;
        }
	}
}
