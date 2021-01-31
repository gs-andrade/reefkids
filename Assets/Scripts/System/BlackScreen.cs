using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    private Image screen;
    private float timer;
    private float timeToFillComplete;
    private float timeToFade;
    private BlackScreenState state = BlackScreenState.None;

    public void ShowBlackScreen(float timeToComplete, float timeToFade)
    {
        if (screen == null)
            screen = GetComponentInChildren<Image>();

        timer = 0;
        this.timeToFillComplete = timeToComplete;
        this.timeToFade = timeToFade;

        state = BlackScreenState.Fill;
    }

    private void FixedUpdate()
    {
        switch (state)
        {

            case BlackScreenState.Fill:
                {
                    timer += Time.deltaTime;
                    if (timer < timeToFillComplete)
                    {
                        screen.fillAmount = timer / timeToFillComplete;
                    }
                    else
                    {
                        screen.fillAmount = 1;
                        state = BlackScreenState.TransitionDelay;
                        timer = 0.25f;
                    }

                    break;
                }

            case BlackScreenState.TransitionDelay:
                {
                    timer -= Time.deltaTime;

                    if (timer <= 0)
                    {
                        state = BlackScreenState.Fade;
                        timer = timeToFade;
                    }
                    break;
                }

            case BlackScreenState.Fade:
                {
                    timer -= Time.deltaTime;
                    if (timer > 0)
                    {
                        screen.fillAmount = timer / timeToFade;
                    }
                    else
                    {
                        screen.fillAmount = 0;
                        state = BlackScreenState.Fade;
                        timer = 0;
                        state = BlackScreenState.None;
                    }

                    break;
                }

        }
    }
}


public enum BlackScreenState
{
    None,
    Fill,
    Fade,
    TransitionDelay,
}
