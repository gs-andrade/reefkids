using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPoint : MonoBehaviour
{
    public SpriteRenderer[] NextLevelFlags;

    private Action endLevel;
    private List<CharacterType> charactersInRange;

    private bool finished;

    public void Setup(Action endLevel)
    {
        if (charactersInRange == null)
            charactersInRange = new List<CharacterType>();

        charactersInRange.Clear();

        this.endLevel = endLevel;

        finished = false;
    }

    public void ResetLevel()
    {
        charactersInRange.Clear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finished)
            return;

        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            if (!charactersInRange.Contains(character.CharacterType))
            {
                charactersInRange.Add(character.CharacterType);

                if (charactersInRange.Count >= 3)
                {
                    finished = true;
                    endLevel();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            if (charactersInRange.Contains(character.CharacterType))
            {
                charactersInRange.Remove(character.CharacterType);
            }
        }
    }

}
