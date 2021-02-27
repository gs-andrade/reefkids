using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform CharacterStartPositionReference;

    private IInterctable[] interctables;
    private IUpdatable[] updatables;

    public void Setup()
    {
        if (interctables == null)
            interctables = GetComponentsInChildren<IInterctable>(true);

        updatables = GetComponentsInChildren<IUpdatable>(true);

        for (int i = 0; i < interctables.Length; i++)
        {
            interctables[i].SetupOnStartLevel();
        }

    }

    public void UpdateObjs()
    {
        if (updatables == null)
            return;

        for(int i = 0; i < updatables.Length; i++)
        {
            updatables[i].UpdateObj();
        }
    }

    public void ResetLevel()
    {

        for (int i = 0; i < interctables.Length; i++)
        {
            interctables[i].ResetObj();
        }
    }
}
