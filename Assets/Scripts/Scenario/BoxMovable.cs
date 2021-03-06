﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovable : MonoBehaviour, IInterctable, IUpdatable
{
    public InteractiveState boxState;
    private Rigidbody2D rb;
    private float unlockedDelay;
    private void LockkMovement()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        boxState = InteractiveState.Locked;
    }

    private void UnlockMovement()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        boxState = InteractiveState.Unlocked;
        unlockedDelay = 0.05f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collide(collision);

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Collide(collision);
    }

    private void Collide(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            UnlockMovement();
        }
    }

    private Vector2 startPosition;
    public void SetupOnStartLevel()
    {
        startPosition = transform.position;
    }

    public void ResetObj()
    {
        transform.position = startPosition;
        LockkMovement();
    }

    public void UpdateObj()
    {

        if (unlockedDelay > 0)
            unlockedDelay -= Time.deltaTime;
        else
            LockkMovement();
    }
}

public enum InteractiveState
{
    Locked,
    Unlocked
}