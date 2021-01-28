using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Attributes")]
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

        var character = ActiveChar();
        var horizontalMOvement = 0f;

        if (Input.GetKeyDown(KeyCode.S) && character.CheckIfIsOnGround())
        {
            character.Jump();
        }

        if (input.ChangeCharacter)
        {
            SwapCharacter();
        }
        else if (input.UseSkill)
        {
            ActiveChar().UseSkill();
        }
        else if (input.Horizontal != 0)
        {
            horizontalMOvement = input.Horizontal * character.Speed;
        }

        for (int i = 0; i < characerInstances.Length; i++)
        {
            if (selectedCharacterIndex == i)
                characerInstances[i].SetXVelocity(horizontalMOvement);
            else
                characerInstances[i].SetXVelocity(0);
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
