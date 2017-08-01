using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public const int START = 0;
    public const int INGAME = 1;
    public const int GAME_OVER = 2;  // TODO: Keep this updated
    public const int MAX_DIFF_LVL = 2;  // 0,1,2

    string[] scenePaths = {"start-screen", "ingame", "gameover-screen"};

    // Info to be accessed from several scenes
    public int diffLvl;
    public bool hintInfo;
    public int nrRescuedVictims;
    public bool gameSuccess;
    public int gameplayTime;

    StartMenu startMenu;
    BackgroundMusic music;

    WorldManager worldMan;
    GameObject ingameUI;
    PauseScreen pauseScreen;
    GameOverSeq gameOverSeq;


    void Start () {
        // When testing
        if(SceneManager.GetActiveScene().buildIndex == INGAME)
        {
            GetInGameVariables();
            pauseScreen.UnPause();
            gameOverSeq.enabled = false;
            music = GameObject.Find("Background Music").GetComponent<BackgroundMusic>();
            music.Init();
            music.StartVariatedLoop();
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
            music = GameObject.Find("Background Music").GetComponent<BackgroundMusic>();
            music.Init();
            music.StartNormalLoop();
            startMenu.StartMenuInit();
        }
    }

    void GetInGameVariables()
    {
        worldMan = GameObject.Find("Environment").GetComponent<WorldManager>();
        ingameUI = GameObject.Find("IngameUI");
        pauseScreen = GameObject.Find("Pause Screen").GetComponent<PauseScreen>();
        gameOverSeq = GameObject.Find("Game Over Sequence").GetComponent<GameOverSeq>();

    }

    public void SwitchScene(int scene)
    {
        int oldScene = SceneManager.GetActiveScene().buildIndex;
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

        print("In Pause in game controller");
        worldMan.enabled = false;
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

    public void GameOver()
    {
        nrRescuedVictims = worldMan.player.nrRescuedVictims;
        gameplayTime = (int)Time.time - worldMan.player.gamePlayStartTime;
        gameSuccess = nrRescuedVictims == VictimFactory.maxVictims[instance.diffLvl];
        gameOverSeq.enabled = true;
        gameOverSeq.Init();
        worldMan.enabled = false;
        worldMan.intManager.ResetInRangeInteractable();
        worldMan.player.infoBox.Close();
        music.StartNormalLoop();
        //SwitchScene(GameController.GAME_OVER);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("In game controller scene loaded");
        if (scene.buildIndex == INGAME)
        {
            GetInGameVariables();
            pauseScreen.UnPause();
            gameOverSeq.enabled = false;
            music.StartVariatedLoop();
        }
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
