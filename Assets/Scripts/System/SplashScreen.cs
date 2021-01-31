using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Image Cover;
    public float FadeTime;
    public float ShowTitleDelay;
    public Text Title;
    public Text ContinueTxt;

    public MenuInterface Menu;

    private float fadeTimer;
    private float initialDelay = 3f;
    private float titleDelay;
    private float finalDelay = 1.5f;
    private float timerContnue;
    private float anotherFinalTimer = 1.5f;
    private bool isTextShow;
    private bool endThis;

    private void Awake()
    {
        fadeTimer = FadeTime;
        titleDelay = ShowTitleDelay;
    }

    private void FixedUpdate()
    {
        if(initialDelay > 0)
        {
            initialDelay -= Time.deltaTime;
        }
        else if(fadeTimer > 0)
        {
            var alpha = fadeTimer / FadeTime;
            Cover.color = new Color(255, 255, 255, alpha);
            fadeTimer -= Time.deltaTime;
        }
        else if (titleDelay > 0)
        {
            var alpha = ShowTitleDelay / titleDelay;
            Title.color = new Color(255, 255, 255, alpha);
            titleDelay -= Time.deltaTime;
        }
        else if(finalDelay > 0)
        {
            var alpha = finalDelay / 1.5f;
            Cover.color = new Color(255, 255, 255, alpha);
            finalDelay -= Time.deltaTime;
        }
        else if(!endThis)
        {
            if (timerContnue > 0)
            {
                timerContnue -= Time.deltaTime;
            }
            else
            {
                if (!isTextShow)
                    ContinueTxt.color = Color.black;
                else
                    ContinueTxt.color = new Color(0, 0, 0, 0);

                timerContnue = 0.75f;

                isTextShow = !isTextShow;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                endThis = true;
            }
        }
        else if (anotherFinalTimer > 0)
        {
            var alpha = anotherFinalTimer / 1.5f;
            Cover.color = new Color(255, 255, 255, alpha);
            anotherFinalTimer -= Time.deltaTime;

            if(anotherFinalTimer <= 0.75f)
            {
                ContinueTxt.color = new Color(0, 0, 0, 0);
                Title.color = new Color(255, 255, 255, 0);
            }
        }
        else
        {
            Menu.Show();
            enabled = false;
        }
    }

}

