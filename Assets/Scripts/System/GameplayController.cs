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

    public bool ForceGameplay;

    private GameState state;

    private Level[] levels;
    private int currentLevelIndex;

    private CharacterController characters;
    private Camera mainCamera;

    private int lifeCurrent;

    private void Awake()
    {
        instance = this;

        state = GameState.Game;

        if (levels == null || levels.Length == 0)
            levels = LevelHold.GetComponentsInChildren<Level>(true);

        if (characters == null)
            characters = GetComponentInChildren<CharacterController>();


        currentLevelIndex = -1;
        mainCamera = Camera.main;

        characters.Setup();

        //if (ForceGameplay)
        {
            StartNextLevel();
        }

        lifeCurrent = 3;

    }

    public CharacterInstance GetPlayer()
    {
        return characters.GetPlayer();
    }

    public void StartNextLevel()
    {
        GameplayInterface.gameObject.SetActive(true);

        if (currentLevelIndex > -1 && currentLevelIndex < levels.Length - 1)
            LevelCurrent().gameObject.SetActive(false);

        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
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


            case GameState.Game:
                {
                    GameplayInterface.UpdateLifeAmmount(lifeCurrent);

                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        state = GameState.Paused;

                        Time.timeScale = 0;

                        return;
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        RestarLevel();
                        return;
                    }


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
        }
    }
}


public enum GameState
{
    Menu,
    Game,
    Paused,
    FinishedGame,
}
