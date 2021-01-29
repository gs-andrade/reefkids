using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public Transform LevelHold;

    public Level[] levels;
    public int currentLevelIndex;

    private CharacterController characters;
    private Camera mainCamera;

    private void Awake()
    {
        if (levels == null || levels.Length == 0)
            levels = LevelHold.GetComponentsInChildren<Level>(true);

        if (characters == null)
            characters = GetComponentInChildren<CharacterController>();

        currentLevelIndex = -1;
        mainCamera = Camera.main;

        characters.Setup();
        StartNextLevel();

       

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

        ResetCameraPos();
        LevelCurrent().Setup(EndLevel);
        LevelCurrent().gameObject.SetActive(true);
    }

    private void ResetCameraPos()
    {
        mainCamera.transform.position = new Vector3(LevelCurrent().CameraPosition.x, LevelCurrent().CameraPosition.y, -10);
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
        LevelCurrent().ResetLevel();
        characters.ResetCharacterToStartPosition(LevelCurrent().CharactersStartPosition.position);
        ResetCameraPos();
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
