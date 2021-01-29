using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance { private set; get; }

    public Transform LevelHold;

    public Level[] levels;
    public int currentLevelIndex;

    private CharacterController characters;
    private Camera mainCamera;

    private int lifeCurrent;

    private void Awake()
    {
        if (levels == null || levels.Length == 0)
            levels = LevelHold.GetComponentsInChildren<Level>(true);

        if (characters == null)
            characters = GetComponentInChildren<CharacterController>();

        instance = this;

        currentLevelIndex = -1;
        mainCamera = Camera.main;

        characters.Setup();
        StartNextLevel();

        lifeCurrent = 3;
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
        characters.ResetCharacterToStartPosition(LevelCurrent().CharactersStartPosition.position);
        ResetCameraPos();
    }

    public bool TakeDamageAndCheckIfIsAlive(int ammount)
    {
        lifeCurrent -= ammount;

        if(lifeCurrent <= 0)
        {
            RestarLevel();
            return false;
        }

        return true;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestarLevel();
            return;
        }

        characters.UpdateCharacters();
    }
}
