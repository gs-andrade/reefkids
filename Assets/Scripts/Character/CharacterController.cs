using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Attributes")]
    private CharacterInstance character;

    private PlayerInput input;

    private bool wasOnAir = true;

    public void Setup()
    {
        if (character == null)
        {
            character = GetComponentInChildren<CharacterInstance>(true);
            character.Setup();
        }

        if (input == null)
            input = new PlayerInput();

    }


    public void UpdateCharacters()
    {
        if (character.IsDisabled())
            return;

        input.GetInputs();

        var horizontalMOvement = 0f;

        var grounded = character.CheckIfIsOnGround();

        if (wasOnAir && grounded)
        {
            SoundController.instance.PlayAudioEffect(character.SoundKey + "Fall", SoundAction.Play);
        }

        wasOnAir = !grounded;

        if (  (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow)) && grounded)
        {
            character.Jump();
            SoundController.instance.PlayAudioEffect(character.SoundKey + "Jump", SoundAction.Play);
        }

        if (input.UseSkill)
        {
            character.PowerUser();
        }
        else if (input.Horizontal != 0)
        {
            horizontalMOvement = input.Horizontal * character.Speed;

            if (grounded)
                SoundController.instance.PlayAudioEffect(character.SoundKey + "Step", SoundAction.Play);

        }

        character.SetXVelocity(horizontalMOvement);
    }


    public void ResetCharacterToStartPosition(Transform positionRef)
    {
            character.PowerRelease();
            character.SetMovement(Vector2.zero);
            character.transform.position = positionRef.position;
    }

}
