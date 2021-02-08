﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformFalling : MonoBehaviour, IInterctable, IUpdatable
{
    public float TimeBeforeFall;
    public float FallSpeed;

    private float timerBeforeFall;
    private Rigidbody2D rb;
    private InteractiveState state;
    private float startYPos;
    private Vector2 startPosition;


    private Vector3 savedVelocity;
    private float savedAngularVelocity;
    private RigidbodyConstraints2D savedConstraints;

    private bool soundPlayed;
    public void ResetObj()
    {
        if (rb != null)
            rb.gravityScale = 0;

        state = InteractiveState.Locked;
        transform.position = startPosition;
        timerBeforeFall = TimeBeforeFall;
    }

    public void SaveStart()
    {
        startPosition = transform.position;
        timerBeforeFall = TimeBeforeFall;
    }

    public void UpdateObj()
    {
        if (state == InteractiveState.Unlocked)
        {
            if (timerBeforeFall > 0)
            {
                timerBeforeFall -= Time.deltaTime;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = FallSpeed;

                if (!soundPlayed)
                {
                    SoundController.instance.PlayAudioEffect("PlataformFall", SoundAction.Play);
                    soundPlayed = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null &&  (character.transform.position.y - transform.position.y > 0))
        {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();

            startYPos = transform.position.y;

            state = InteractiveState.Unlocked;
        }
    }

}
