using Cinemachine;
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
    public float BlackScreenDuration;
    public EndGameScreen EndGameScreen;
    public DialogueBox DialogueBox;
    public GameObject PauseMenu;

    public PointToStartGame PointToStartGame;

    public List<ConfigBByLevel> ConfigByLevel;
    private GameState state;

    private Level[] levels;
    private int currentLevelIndex;

    private CharacterController characters;
    private CinemachineVirtualCamera currentVirtualCamera;

    private int lifeCurrent;

    private Vector2 checkPointPosition;
    private float blackScrenTimer;
    private BlackScreenState blackScreenState;
    private void Awake()
    {
        instance = this;

        state = GameState.Game;

        for (int i = 0; i < ConfigByLevel.Count; i++)
        {
            ConfigByLevel[i].CameraToUse.enabled = false;
        }

        if (levels == null || levels.Length == 0)
            levels = LevelHold.GetComponentsInChildren<Level>(true);

        if (characters == null)
            characters = GetComponentInChildren<CharacterController>();


        currentLevelIndex = (int)PointToStartGame;

        characters.Setup();

        StartNextLevel();

        lifeCurrent = 5;

        ActiveDialogue();

        Time.timeScale = 1;
    }

    public CharacterInstance GetPlayer()
    {
        return characters.GetPlayer();
    }

    public void StartNextLevel(bool resetCharacterPosition = true)
    {
        if (GameplayInterface != null)
            GameplayInterface.gameObject.SetActive(true);

        /*if (currentLevelIndex > -1 && currentLevelIndex < levels.Length - 1)
            LevelCurrent().gameObject.SetActive(false);*/

        if (currentVirtualCamera != null)
            currentVirtualCamera.enabled = false;

        for (int i = 0; i < ConfigByLevel.Count; i++)
        {
            var config = ConfigByLevel[i];
            if (config.Level == (PointToStartGame)currentLevelIndex)
            {
                currentVirtualCamera = config.CameraToUse;
                currentVirtualCamera.enabled = true;
                break;
            }
        }



        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            state = GameState.FinishedGame;
            return;
        }

        LevelCurrent().Setup();
        LevelCurrent().gameObject.SetActive(true);
        checkPointPosition = LevelCurrent().CharacterStartPositionReference.position;
        RestarLevel(resetCharacterPosition);
        HealPlayer();
    }

    public void SaveCheckPoint(Vector2 position)
    {
        checkPointPosition = position;
        HealPlayer();
    }


    private Level LevelCurrent()
    {
        if (currentLevelIndex < levels.Length)
            return levels[currentLevelIndex];
        else
            return null;
    }


    public void RestarLevel(bool resetCharacterPosition = true)
    {
        LevelCurrent().ResetLevel();

        HealPlayer();

        if (resetCharacterPosition)
            characters.ResetCharacterToStartPosition(checkPointPosition);
    }

    public void TakeDamageAndCheckIfIsAlive(int ammount)
    {
        if (state != GameState.Game)
            return;

        lifeCurrent -= ammount;

        if (lifeCurrent <= 0)
        {
            state = GameState.ResetLevel;
            blackScrenTimer = BlackScreenDuration;
            BlackScreen.ShowBlackScreen(0.5f, blackScrenTimer);
            blackScreenState = BlackScreen.GetState();
            GameplayInterface.UpdateLifeAmmount(lifeCurrent);
            // SoundController.instance.PlayAudioEffect("LossSound");
        }
    }

    public void HealPlayer()
    {
        lifeCurrent = 5;
    }

    public void ActiveDialogue()
    {
        DialogueBox.ActiveDialoge();
        state = GameState.Dialogue;
    }

    public void DisableDialogue()
    {
        DialogueBox.EndDialog();
    }

    public void FinishGame()
    {
        state = GameState.FinishedGame;
        EndGameScreen.gameObject.SetActive(true);

    }

    public void Unpause()
    {
        state = GameState.Game;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    private void Update()
    {
        switch (state)
        {

            case GameState.Menu:
                {
                    break;
                }


            case GameState.Game:
                {
                    if (GameplayInterface != null)
                        GameplayInterface.UpdateLifeAmmount(lifeCurrent);

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        state = GameState.Paused;

                        Time.timeScale = 0;

                        PauseMenu.SetActive(true);

                        return;
                    }

                   /* if (Input.GetKeyDown(KeyCode.R))
                    {
                        RestarLevel();
                        return;
                    }
                    */

                    if (LevelCurrent() != null)
                        LevelCurrent().UpdateObjs();

                    characters.UpdateCharacters();

                    break;
                }

            case GameState.Paused:
                {
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        state = GameState.Game;
                        Time.timeScale = 1;
                    }
                    break;
                }

            case GameState.ResetLevel:
                {
                    if (blackScreenState != BlackScreen.GetState())
                    {
                        if (BlackScreen.GetState() == BlackScreenState.TransitionDelay)
                        {
                            RestarLevel();
                        }

                        if (BlackScreen.GetState() == BlackScreenState.Complete)
                            state = GameState.Game;

                        blackScreenState = BlackScreen.GetState();
                    }

                    break;
                }

            case GameState.Dialogue:
                {
                    if (DialogueBox.FreezePlayer())
                    {
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            DialogueBox.NextDialogue();
                        }
                    }
                    else
                        state = GameState.Game;

                    break;
                }

            case GameState.FinishedGame:
                {

                    break;
                }
        }
    }
}

//Teemporario
[Serializable]
public class ConfigBByLevel
{
    public PointToStartGame Level;
    public CinemachineVirtualCamera CameraToUse;
}


public enum PointToStartGame
{
    InitialArea = -1,
    Level1 = 0,
    Level2 = 1,
}
public enum GameState
{
    Menu,
    Game,
    Paused,
    ResetLevel,
    Dialogue,
    FinishedGame,
}
