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
    private BlackScreenState state = BlackScreenState.Complete;

    public void ShowBlackScreen(float timeToComplete, float timeToFade)
    {
        if (screen == null)
            screen = GetComponentInChildren<Image>();

        gameObject.SetActive(true);

        timer = 0;
        this.timeToFillComplete = timeToComplete;
        this.timeToFade = timeToFade;

        state = BlackScreenState.Fill;
    }

    public BlackScreenState GetState()
    {
        return state;
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
                        SetScreenColor( timer / timeToFillComplete);
                    }
                    else
                    {
                        SetScreenColor(1);
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
                        SetScreenColor(timer / timeToFade);
                    }
                    else
                    {
                        SetScreenColor(0);
                        state = BlackScreenState.Fade;
                        timer = 0;
                        state = BlackScreenState.Complete;
                        gameObject.SetActive(false);
                    }

                    break;
                }

        }
    }

    private void SetScreenColor(float alpha)
    {
        screen.color = new Color(0, 0, 0, alpha);
    }
}


public enum BlackScreenState
{
    Complete,
    Fill,
    Fade,
    TransitionDelay,
}
