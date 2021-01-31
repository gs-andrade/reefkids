using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance { private set; get; }

    public Transform LevelHold;

    [Header("Interface Reference")]
    public GameplayInterface GameplayInterface;
    public BlackScreen BlackScreen;


    [Header("Menu Reference")]
    public GameObject SplashScreen;
    public GameObject Menu;
    public GameObject PauseMenu;
    public GameObject EndGameMenu;

    public bool ForceGameplay;

    private GameState state;

    private Level[] levels;
    private int currentLevelIndex;

    private CharacterController characters;
    private Camera mainCamera;

    private int lifeCurrent;

    private IUpdatable[] updatables;

    private List<Action> onPause = new List<Action>();
    private List<Action> onUnpause = new List<Action>();

    private float animationTimer;

    private void Awake()
    {
        instance = this;

        state = GameState.Menu;

        if (levels == null || levels.Length == 0)
            levels = LevelHold.GetComponentsInChildren<Level>(true);

        if (characters == null)
            characters = GetComponentInChildren<CharacterController>();

        updatables = GetComponentsInChildren<IUpdatable>();

        currentLevelIndex = -1;
        mainCamera = Camera.main;

        characters.Setup();

        if (ForceGameplay)
        {
            StartNewGame();
        }
        else
            SplashScreen.SetActive(true);

        lifeCurrent = 3;

    }

    public void StartNewGame()
    {
        BlackScreen.gameObject.SetActive(true);
        Menu.SetActive(false);
        state = GameState.StartGamePrepare;
    }

    public void RegisterPause(Action action)
    {
        onPause.Add(action);
    }

    public void RegisterUnpause(Action action)
    {
        onUnpause.Add(action);
    }

    public void StartNextLevel()
    {
        GameplayInterface.gameObject.SetActive(true);

        if (currentLevelIndex > -1 && currentLevelIndex < levels.Length - 1)
            LevelCurrent().gameObject.SetActive(false);

        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            EndGameMenu.SetActive(true);
            state = GameState.FinishedGame;
            return;
        }

        LevelCurrent().Setup(EndLevel);
        LevelCurrent().gameObject.SetActive(true);
        RestarLevel();
    }

    private void ResetCameraPos()
    {
        mainCamera.transform.position = new Vector3(LevelCurrent().CameraPosition.x, LevelCurrent().CameraPosition.y, -10);
        mainCamera.orthographicSize = LevelCurrent().CamereSize;
    }

    private Level LevelCurrent()
    {
        if (currentLevelIndex < levels.Length)
            return levels[currentLevelIndex];
        else
            return null;
    }


    private void EndLevel()
    {
        state = GameState.WinAnimationPrepare;
    }

    public void RestarLevel()
    {
        lifeCurrent = 3;
        LevelCurrent().ResetLevel();
        characters.ResetCharacterToStartPosition(LevelCurrent().CharacterStartPositionReference);
        ResetCameraPos();
    }

    public bool TakeDamageAndCheckIfIsAlive(int ammount)
    {
        lifeCurrent -= ammount;

        if (lifeCurrent <= 0)
        {
            state = GameState.LossAnimationPrepare;
            return false;
        }

        return true;
    }

    public void HealPlayer()
    {
        if (lifeCurrent < 3)
            lifeCurrent++;
    }

    private void Update()
    {
        switch (state)
        {

            case GameState.Menu:
                {
                    break;
                }

            case GameState.StartGamePrepare:
                {
                    animationTimer = 2.25f;
                    SoundController.instance.PlayAudioEffect("WinSound");
                    BlackScreen.ShowBlackScreen(2f, 1f);
                    state = GameState.StatGameExecute;

                    break;
                }

            case GameState.StatGameExecute:
                {
                    if (animationTimer > 0)
                    {
                        animationTimer -= Time.deltaTime;
                    }
                    else
                    {
                        Menu.SetActive(false);
                        SplashScreen.SetActive(false);
                        state = GameState.Game;
                        StartNextLevel();
                    }

                    break;
                }

            case GameState.WinAnimationPrepare:
                {
                    animationTimer = 2.25f;
                    SoundController.instance.PlayAudioEffect("WinSound");
                    PauseGame();
                    BlackScreen.ShowBlackScreen(2f, 1f);
                    state = GameState.WinAnimationExecute;
                    break;
                }

            case GameState.WinAnimationExecute:
                {
                    if (animationTimer > 0)
                    {
                        animationTimer -= Time.deltaTime;
                    }
                    else
                    {
                        UnpauseGame();
                        StartNextLevel();
                        state = GameState.Game;
                    }

                    break;
                }

            case GameState.LossAnimationPrepare:
                {
                    animationTimer = 2.25f;
                    SoundController.instance.PlayAudioEffect("LossSound");
                    PauseGame();
                    BlackScreen.ShowBlackScreen(2f, 1f);
                    state = GameState.LossAnimationExecute;
                    break;
                }

            case GameState.LossAnimationExecute:
                {
                    if (animationTimer > 0)
                    {
                        animationTimer -= Time.deltaTime;
                    }
                    else
                    {
                        UnpauseGame();
                        RestarLevel();
                        state = GameState.Game;
                    }

                    break;
                }

            case GameState.Game:
                {
                    GameplayInterface.UpdateLifeAmmount(lifeCurrent);

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        state = GameState.Paused;

                        PauseGameAndShowMenu();

                        return;
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        RestarLevel();
                        return;
                    }

                    for (int i = 0; i < updatables.Length; i++)
                    {
                        updatables[i].UpdateObj();
                    }

                    if (LevelCurrent() != null)
                        LevelCurrent().UpdateObjs();

                    characters.UpdateCharacters();

                    break;
                }

            case GameState.Paused:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        state = GameState.Game;
                        UnpauseGame();

                    }
                    break;
                }
        }
    }

    public void PauseGame()
    {
        for (int i = 0; i < onPause.Count; i++)
            onPause[i]();
    }

    public void PauseGameAndShowMenu()
    {
        PauseGame();
        PauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        for (int i = 0; i < onUnpause.Count; i++)
            onUnpause[i]();

        PauseMenu.SetActive(false);
    }
}


public enum GameState
{
    Menu,
    Game,
    Paused,
    StartGamePrepare,
    StatGameExecute,
    LossAnimationPrepare,
    LossAnimationExecute,
    WinAnimationPrepare,
    WinAnimationExecute,
    FinishedGame,
}
