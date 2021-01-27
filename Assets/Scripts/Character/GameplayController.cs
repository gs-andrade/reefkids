using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    private CharacterController characters;

    private void Awake()
    {
        if (characters == null)
            characters = GetComponentInChildren<CharacterController>();

        characters.Setup();
    }


    private void Update()
    {
        characters.UpdateCharacters();
    }
}
