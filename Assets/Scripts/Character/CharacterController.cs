using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Attributes")]
    private CharacterInstance[] characerInstances;

    private PlayerInput input;
    private int selectedCharacterIndex;

    private CharacterType lastActiveCharacter;
    private bool wasOnAir = true;

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

        lastActiveCharacter = ActiveChar().CharacterType;
    }


    public void UpdateCharacters()
    {
        var character = ActiveChar();

        if (character.IsDisabled())
            return;

        input.GetInputs();

        var horizontalMOvement = 0f;

        var grounded = character.CheckIfIsOnGround();

        if (wasOnAir && lastActiveCharacter == character.CharacterType && grounded)
        {
            SoundController.instance.PlayAudioEffect(character.SoundKey + "Fall", SoundAction.Play);
        }

        lastActiveCharacter = character.CharacterType;
        wasOnAir = !grounded;

        if (  (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow)) && grounded)
        {
            character.Jump();
            SoundController.instance.PlayAudioEffect(character.SoundKey + "Jump", SoundAction.Play);
        }

        if (input.ChangeCharacter)
        {
            SwapCharacter();
        }
        else if (input.UseSkill)
        {
            ActiveChar().PowerUser();
        }
        else if (input.Horizontal != 0)
        {
            horizontalMOvement = input.Horizontal * character.Speed;

            if (grounded)
                SoundController.instance.PlayAudioEffect(character.SoundKey + "Step", SoundAction.Play);

        }

        for (int i = 0; i < characerInstances.Length; i++)
        {
            if (selectedCharacterIndex == i)
            {
                characerInstances[i].SetXVelocity(horizontalMOvement);
                characerInstances[i].ToogleSelect(true);
            }
            else
            {
                characerInstances[i].SetXVelocity(0);
                characerInstances[i].ToogleSelect(false);
            }
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

    public void ResetCharacterToStartPosition(Transform[] positionRef)
    {
        for(int i = 0; i < characerInstances.Length; i++)
        {
            var character = characerInstances[i];
            character.PowerRelease();
            character.SetMovement(Vector2.zero);
            character.transform.position = positionRef[i].position;
        }
    }

    private CharacterInstance ActiveChar()
    {
        return characerInstances[selectedCharacterIndex];
    }
}
