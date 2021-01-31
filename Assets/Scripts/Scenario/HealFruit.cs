using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFruit : MonoBehaviour, IInterctable
{
    private Vector2 startPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            GameplayController.instance.HealPlayer();
            gameObject.SetActive(false);  
        }

    }
    public void ResetObj()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void SaveStart()
    {
        startPosition = transform.position;
    }
}
