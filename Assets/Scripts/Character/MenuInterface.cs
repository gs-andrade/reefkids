using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInterface : MonoBehaviour
{

    public GameObject SettingMenu;
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        SettingMenu.SetActive(true);
    }

    public void CloseSettingMenu()
    {
        SettingMenu.SetActive(false);
    }

    public void StartGame()
    {
        GameplayController.instance.StartNewGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
