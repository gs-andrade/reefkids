using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Attributes")]
    public float Speed;
    private CharacterInstance[] characerInstances;

    private PlayerInput input;
    private int selectedCharacterIndex;

    public void Setup()
    {
        if (characerInstances == null || characerInstances.Length == 0)
        {
            characerInstances = GetComponentsInChildren<CharacterInstance>(true);
        }

        for (int i = 0; i < characerInstances.Length; i++)
            characerInstances[i].Setup();

        if (input == null)
            input = new PlayerInput();

        SwapCharacter();
    }


    public void UpdateCharacters()
    {
        input.GetInputs();

        var activeCharacterMovement = Vector2.zero;

        if (input.ChangeCharacter)
        {
            SwapCharacter();
        }
        else if (input.Horizontal != 0 || input.Vertical != 0)
        {
            var moveDirection = new Vector2(input.Horizontal * Speed, input.Vertical * Speed);
            activeCharacterMovement = Speed * moveDirection.normalized * Time.deltaTime;
        }

        for (int i = 0; i < characerInstances.Length; i++)
        {
            if (selectedCharacterIndex == i)
                characerInstances[i].SetMovement(activeCharacterMovement);
            else
                characerInstances[i].SetMovement(Vector2.zero);
        }
    }

    private void SwapCharacter()
    {
        selectedCharacterIndex++;
        if (selectedCharacterIndex >= characerInstances.Length)
            selectedCharacterIndex = 0;

        for (int i = 0; i < characerInstances.Length; i++)
        {
            if (selectedCharacterIndex == i)
                characerInstances[i].UnlockMovement();
            else
                characerInstances[i].LockkMovement();
        }
    }


    private CharacterInstance ActiveChar()
    {
        return characerInstances[selectedCharacterIndex];
    }
}
