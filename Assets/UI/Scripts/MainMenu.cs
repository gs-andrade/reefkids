using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ShowInfo (GameObject info)
    {
        if (info.activeSelf)
            info.SetActive(false);

        else
            info.SetActive(true);


    }


    public void PlayGame ()

    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void QuitGame ()

    {
        Debug.Log("QUIT!");

        Application.Quit();

    }

}
