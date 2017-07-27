using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public const int START = 0;
    public const int INGAME = 1;
    public const int GAME_OVER = 2;  // TODO: Keep this updated
    string[] scenePaths = {"start-screen", "ingame", "gameover-screen"};
    // Info to be accessed from several scenes
    public bool hintInfo;
    public int nrRescuedVictims;
    public bool gameSuccess;
    public int gameplayTime;

    StartMenu startMenu;

    WorldManager worldMan;
    GameObject ingameUI;
    PauseScreen pauseScreen;


    void Start () {
        // When testing
        if(SceneManager.GetActiveScene().buildIndex == INGAME)
        {
            GetInGameVariables();
            pauseScreen.UnPause();
        }

        // Make sure this class can only be instantiated once
		if(instance == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().buildIndex == START)
        {
            startMenu = GameObject.Find("Start Menu").GetComponent<StartMenu>();
            startMenu.StartMenuInit();
        }
    }

    void GetInGameVariables()
    {
        worldMan = GameObject.Find("Environment").GetComponent<WorldManager>();
        ingameUI = GameObject.Find("IngameUI");
        pauseScreen = GameObject.Find("Pause Screen").GetComponent<PauseScreen>();
    }

    public void SwitchScene(int scene)
    {
        /*if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);  // So Game Controller can access it if INGAME level is reloaded
        }*/

        int oldScene = SceneManager.GetActiveScene().buildIndex;
        /*SceneManager.LoadScene(scene, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(scene));*/
        SceneManager.LoadScene(scenePaths[scene], LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenePaths[scene]));
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
        //pauseScreen.SetActive(true);
        pauseScreen.Init();
        pauseScreen.Pause();
    }

    public void UnPause()
    {
        if (pauseScreen == null) return;

        worldMan.enabled = true;
        //pauseScreen.SetActive(false);
        pauseScreen.UnPause();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("In game controller scene loaded");
        if (scene.buildIndex == INGAME)
        {
            GetInGameVariables();
            pauseScreen.UnPause();
        }
    }
}
