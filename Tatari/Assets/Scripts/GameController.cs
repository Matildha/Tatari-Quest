using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    WorldManager worldMan;
    GameObject ingameUI;
    GameObject pauseScreen;

    public const int INGAME = 0;
    public const int GAME_OVER = 1;  // TODO: Keep this updated
    // Info to be accessed from several scenes
    public int nrRescuedVictims;
    public bool gameSuccess;
    public int gameplayTime;
    

	void Start () {
        GetInGameVariables();  // TEMPPPPPP!!! 
        SceneManager.sceneLoaded += OnSceneLoaded;
        pauseScreen.SetActive(false);

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

    void GetInGameVariables()
    {
        worldMan = GameObject.Find("Environment").GetComponent<WorldManager>();
        ingameUI = GameObject.Find("IngameUI");
        pauseScreen = GameObject.Find("Pause Screen");
    }

    public void SwitchScene(int scene)
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);  // So Game Controller can access it if INGAME level is reloaded
        }

        int oldScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(scene));
        /*if(scene == INGAME)
        {
            GetInGameVariables();
        }*/
    }

    public void Pause()
    {
        if (pauseScreen == null)
        {
            print("PAUSESCREEN IS NULL in pause");
            return;
        }
        /*worldMan.player.gameObject.SetActive(false);
        worldMan.gameObject.SetActive(false);*/
        print("In Pause in game controller");
        worldMan.enabled = false;
        pauseScreen.SetActive(true);
        pauseScreen.GetComponent<PauseScreen>().Init();
    }

    public void UnPause()
    {
        if (pauseScreen == null) return;
        /*worldMan.gameObject.SetActive(true);
        worldMan.player.gameObject.SetActive(true);*/
        worldMan.enabled = true;
        pauseScreen.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("In game controller scene loaded");
        if (scene.buildIndex == INGAME)
        {
            GetInGameVariables();

        }
    }
}
