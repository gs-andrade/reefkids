﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public CheckpointType CheckType = CheckpointType.Normal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if(character != null)
        {
            GameplayController.instance.SaveCheckPoint(transform.position);

            if(CheckType == CheckpointType.NextLevelReady)
            {
                GameplayController.instance.StartNextLevel(false);
                CheckType = CheckpointType.NextLevelDone;
            }
        }
    }


    public enum CheckpointType
    {
        Normal,
        NextLevelReady,
        NextLevelDone,
        
    }

}

