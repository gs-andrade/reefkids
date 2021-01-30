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
        var character = ActiveChar();

        if (character.IsDisabled())
            return;

        input.GetInputs();

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

    public void ResetCharacterToStartPosition(Vector2 position)
    {
        transform.position = position;

        for(int i = 0; i < characerInstances.Length; i++)
        {
            var character = characerInstances[i];
            character.SetMovement(Vector2.zero);
            character.transform.localPosition = new Vector2(2 * i, 0);
        }
    }

    private CharacterInstance ActiveChar()
    {
        return characerInstances[selectedCharacterIndex];
    }
}
