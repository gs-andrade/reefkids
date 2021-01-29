using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Vector2 CameraPosition;
    public Transform CharactersStartPosition;
    public float CamereSize = 13f;

    private IInterctable[] interctables;
    private EndLevelPoint endLevelPoint;

    public void Setup(Action endLevel)
    {
        if (interctables == null)
            interctables = GetComponentsInChildren<IInterctable>(true);

        endLevelPoint = GetComponentInChildren<EndLevelPoint>(true);

        endLevelPoint.Setup(endLevel);

        for (int i = 0; i < interctables.Length; i++)
        {
            interctables[i].SaveStart();
        }
    }

    public void ResetLevel()
    {
        endLevelPoint.ResetLevel();

        for (int i = 0; i < interctables.Length; i++)
        {
            interctables[i].Reset();
        }
    }
}
