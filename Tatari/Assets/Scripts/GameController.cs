using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    // Scenes
    public const int START = 0;
    public const int INGAME = 1;
    public const int GAME_OVER = 2; 

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

    public WorldManager worldMan;
    GameObject ingameUI;
    PauseScreen pauseScreen;
    GameOverSeq gameOverSeq;

    public bool showIntro;
    IntroSeq introSeq;


    void Start()
    {
        // For debuggin and testing (normally game starts at scene START)
        if (SceneManager.GetActiveScene().buildIndex == INGAME)
        {
            GetInGameVariables();
            pauseScreen.UnPause();
            gameOverSeq.enabled = false;
            music = GameObject.Find("Background Music").GetComponent<BackgroundMusic>();
            music.Init();
            music.StartVariatedLoop();
            showIntro = true;
            worldMan.enabled = false;
            introSeq.enabled = true;
        }

        // Make sure this class can only be instantiated once
        if (instance == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        print(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == scenePaths[START])
        {
            startMenu = GameObject.Find("Start Menu").GetComponent<StartMenu>();
            music = GameObject.Find("Background Music").GetComponent<BackgroundMusic>();
            music.Init();
            music.StartNormalLoop();
            startMenu.StartMenuInit();
            print("In active scene start in gamecontroller start");
            showIntro = true;
            diffLvl = 1;
        }
    }

    void GetInGameVariables()
    {
        worldMan = GameObject.Find("Environment").GetComponent<WorldManager>();
        ingameUI = GameObject.Find("IngameUI");
        pauseScreen = GameObject.Find("Pause Screen").GetComponent<PauseScreen>();
        gameOverSeq = GameObject.Find("Game Over Sequence").GetComponent<GameOverSeq>();
        introSeq = GameObject.Find("Intro Sequence").GetComponent<IntroSeq>();
    }

    public void SwitchScene(int scene)
    {
        int oldScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scenePaths[scene], LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenePaths[scene]));
        if(scene == START)
        {
            Destroy(music.gameObject);
            Destroy(this.gameObject);
        }
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
        worldMan.player.playerController.unableToMove = true;
        ingameUI.SetActive(false);
        //pauseScreen.Init();
        pauseScreen.Pause();
    }

    public void UnPause()
    {
        if (pauseScreen == null) return;

        worldMan.enabled = true;
        worldMan.player.playerController.unableToMove = false;
        ingameUI.SetActive(true);
        pauseScreen.UnPause();
    }

    public void GameOver()
    {
        nrRescuedVictims = worldMan.player.nrRescuedVictims;
        gameplayTime = (int)Time.time - worldMan.player.gamePlayStartTime;
        gameSuccess = nrRescuedVictims == VictimFactory.maxVictims[instance.diffLvl];
        worldMan.enabled = false;
        worldMan.intManager.ResetInRangeInteractable();
        worldMan.player.infoBox.Close();
        music.StartNormalLoop();

        if(gameSuccess)
        {
            SwitchScene(GAME_OVER);
        }
        else
        {
            gameOverSeq.enabled = true;
            gameOverSeq.Init();
        }
        
    }

    public void LoadStartMenu()
    {
        showIntro = true;
        SwitchScene(START);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("In game controller scene loaded");
        if (scene.name == scenePaths[INGAME])
        {
            GetInGameVariables();
            pauseScreen.UnPause();
            gameOverSeq.enabled = false;
            music = GameObject.Find("Background Music").GetComponent<BackgroundMusic>();
            music.Init();
            music.StartVariatedLoop();
            if (showIntro)
            {
                worldMan.enabled = false;
                introSeq.enabled = true;
            }
            else
            {
                worldMan.enabled = true;
                introSeq.enabled = false;
            }
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
