using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPoint : MonoBehaviour
{
    public SpriteRenderer[] NextLevelFlags;

    private Action endLevel;

    private bool finished;

    public void Setup(Action endLevel)
    {
        

        this.endLevel = endLevel;

        finished = false;
    }

    public void ResetLevel()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finished)
            return;

        var character = collision.gameObject.GetComponent<CharacterInstance>();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();
    }

}
