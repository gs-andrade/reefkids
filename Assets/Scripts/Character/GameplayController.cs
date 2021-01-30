using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance { private set; get; }

    public Transform LevelHold;

    private GameState state;

    private Level[] levels;
    private int currentLevelIndex;

    private CharacterController characters;
    private Camera mainCamera;

    private int lifeCurrent;

    private IUpdatable[] updatables;

    private List<Action> onPause = new List<Action>();
    private List<Action> onUnpause = new List<Action>();

    private void Awake()
    {
        instance = this;

        if (levels == null || levels.Length == 0)
            levels = LevelHold.GetComponentsInChildren<Level>(true);

        if (characters == null)
            characters = GetComponentInChildren<CharacterController>();

        updatables = GetComponentsInChildren<IUpdatable>();

        currentLevelIndex = -1;
        mainCamera = Camera.main;

        characters.Setup();
        StartNextLevel();

        lifeCurrent = 3;

        state = GameState.Game;
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
        if (currentLevelIndex > -1 && currentLevelIndex < levels.Length - 1)
            LevelCurrent().gameObject.SetActive(false);

        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            Debug.Log("FinishedAllLevels. Level index: " + currentLevelIndex);
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
        return levels[currentLevelIndex];
    }


    private void EndLevel()
    {
        StartNextLevel();
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
            RestarLevel();
            return false;
        }

        return true;
    }

    private void Update()
    {
        switch (state)
        {

            case GameState.Game:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        state = GameState.Paused;

                        for (int i = 0; i < onPause.Count; i++)
                            onPause[i]();

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

                    LevelCurrent().UpdateObjs();

                    characters.UpdateCharacters();

                    break;
                }

            case GameState.Paused:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        state = GameState.Game;

                        for (int i = 0; i < onPause.Count; i++)
                            onUnpause[i]();
                    }
                    break;
                }
        }
    }
}


public enum GameState
{
    Game,
    Paused,
}
