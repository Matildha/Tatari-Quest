using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * GameController controls the switching of scenes and their primary set up and
 * is therefore also set to DontDestroyOnLoad. 
 * 
 * Contains functionality to restart completely from START, trigger game over and
 * pause, unpause the game. 
 * 
 * GameController contains variables used to pass information between scenes. 
*/

public class GameController : MonoBehaviour {

    public static GameController instance;

    // Scenes
    public const int START = 0;
    public const int INGAME = 1;
    public const int GAME_OVER = 2; 

    public const int MAX_DIFF_LVL = 2;  // 0,1,2 ; where 2 is the most difficult

    public WorldManager worldMan;

    string[] scenePaths = {"start-screen", "ingame", "gameover-screen"};

    // Info to be accessed from several scenes
    public int diffLvl;
    public bool hintInfo;
    public int nrRescuedVictims;
    public bool gameSuccess;
    public int gameplayTime;

    public bool showIntro;
    IntroSeq introSeq;

    StartMenu startMenu;
    BackgroundMusic music;

    GameObject ingameUI;
    PauseScreen pauseScreen;
    GameOverSeq gameOverSeq;


    private void Start()
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

        //print(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == scenePaths[START])
        {
            startMenu = GameObject.Find("Start Menu").GetComponent<StartMenu>();
            music = GameObject.Find("Background Music").GetComponent<BackgroundMusic>();
            music.Init();
            music.StartNormalLoop();
            startMenu.StartMenuInit();
            showIntro = true;
            diffLvl = 1;  // Set default difficult level to 1 (normal)
            //print("In active scene start in gamecontroller start");
        }
    }

    private void GetInGameVariables()
    {
        worldMan = GameObject.Find("Environment").GetComponent<WorldManager>();
        ingameUI = GameObject.Find("IngameUI");
        pauseScreen = GameObject.Find("Pause Screen").GetComponent<PauseScreen>();
        gameOverSeq = GameObject.Find("Game Over Sequence").GetComponent<GameOverSeq>();
        introSeq = GameObject.Find("Intro Sequence").GetComponent<IntroSeq>();
    }

    /* Loads scene as the only active scene. If the given scene is START, the current instance of
     GameController and BackgroundMusic will be destroyed. (to be reinstantiated by the START scene) */
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

    /* Interrupts most parts of the INGAME scene by disabling WorldManager and hiding 
     ingame UI. Calls PauseScreen Pause() to enable pause menu. */
    public void Pause()
    {
        if (pauseScreen == null)
        {
            print("PAUSESCREEN IS NULL in pause");
            return;
        }

        //print("In Pause in game controller");
        worldMan.enabled = false;
        worldMan.player.playerController.unableToMove = true;  // So FootSteps' sound can not be played
        ingameUI.SetActive(false);
        //pauseScreen.Init();
        pauseScreen.Pause();
    }


    /* Resumes activity of all parts of the INGAME scene that was interrupted by Pause() by enabling WorldManager and showing 
     ingame UI. Calls PauseScreen UnPause() to disable pause menu. */
    public void UnPause()
    {
        if (pauseScreen == null) return;

        worldMan.enabled = true;
        worldMan.player.playerController.unableToMove = false;
        ingameUI.SetActive(true);
        pauseScreen.UnPause();
    }

    /* Initiates the end of the game session. Retrives information required by GAME_OVER scene, 
     * disables WorldManager and either switches to GAME_OVER scene or enables GameOverSeq. */
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

    /* Loads the START scene with showIntro set to true. */
    public void LoadStartMenu()
    {
        showIntro = true;
        SwitchScene(START);
    }

    /* Quits the application. */
    public static void ExitGame()
    {
        Application.Quit();
    }

    /* Sets up the INGAME scene. This function is called by SceneManager when a scene
     * has finished loading. */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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

    /* Controls the locked state of the user cursor. */
    private void OnApplicationFocus(bool focus)
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
