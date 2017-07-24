using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public WorldManager worldMan;
    public GameObject ingameUI;
    public GameObject pauseScreen;

    public const int INGAME = 0;
    public const int GAME_OVER = 1;  // TODO: Keep this updated
    // Info to be accessed from several scenes
    public int nrRescuedVictims;
    public bool gameSuccess;
    public int gameplayTime;
    

	void Start () {
        // Make sure this class can only be instantiated once
		if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public void SwitchScene(int scene)
    {
        int oldScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(scene));
    }

    public void Pause()
    {
        if (pauseScreen == null) return;
        /*worldMan.player.gameObject.SetActive(false);
        worldMan.gameObject.SetActive(false);*/
        worldMan.player.enabled = false;
        worldMan.enabled = false;
        pauseScreen.SetActive(true);
    }

    public void UnPause()
    {
        if (pauseScreen == null) return;
        /*worldMan.gameObject.SetActive(true);
        worldMan.player.gameObject.SetActive(true);*/
        pauseScreen.SetActive(false);
    }
}
