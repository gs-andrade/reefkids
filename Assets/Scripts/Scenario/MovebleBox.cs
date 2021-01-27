﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovebleBox : MonoBehaviour
{
    private BoxState boxState;
    private Rigidbody2D rb;
    private float unlockedDelay;
    private void LockkMovement()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        boxState = BoxState.Locked;
    }

    private void UnlockMovement()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        boxState = BoxState.Unlocked;
        unlockedDelay = 0.05f;
    }

    private void Update()
    {
        if(boxState == BoxState.Unlocked)
        {
            if (unlockedDelay > 0)
                unlockedDelay -= Time.deltaTime;
            else
                LockkMovement();
        }
    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if(character != null && character.CharacterType == CharacterType.Strong)
        {
       
            UnlockMovement();
        }
    }

 
}

public enum BoxState
{
    Locked,
    Unlocked
}