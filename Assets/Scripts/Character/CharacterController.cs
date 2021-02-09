using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Attributes")]
    public float Speed;
    public float JumpForce;
    public float DashForce;
    public float DashDuration;
    public float DashCooldown;

    private CharacterInstance character;

    private PlayerInput input;

    private bool wasOnAir = true;
    private bool usedSecondJump = false;

    private CharacterState state;
    private float inputDelay;
    private float dashCd;

    public void Setup()
    {
        if (character == null)
        {
            character = GetComponentInChildren<CharacterInstance>(true);
            character.Setup();
        }

        if (input == null)
            input = new PlayerInput();

        state = CharacterState.Normal;
    }


    public void UpdateCharacters()
    {
        if (inputDelay > 0)
            inputDelay -= Time.deltaTime;

        if (dashCd > 0)
            dashCd -= Time.deltaTime;

        if (character.IsDisabled())
            return;

        input.GetInputs();

        switch (state)
        {
            case CharacterState.Normal:
                {
                    var horizontalMOvement = 0f;

                    var grounded = character.CheckIfIsOnGround();

                    if (grounded)
                    {
                        usedSecondJump = false;
                        if (wasOnAir)
                            SoundController.instance.PlayAudioEffect(character.SoundKey + "Fall", SoundAction.Play);
                    }

                    wasOnAir = !grounded;

                    // JUMP
                    if ((input.JumpPressed || Input.GetKeyDown(KeyCode.UpArrow)) && (grounded || !usedSecondJump) && inputDelay <= 0)
                    {
                        if (!grounded)
                            usedSecondJump = true;

                        character.Jump(JumpForce);
                        inputDelay = 0.2f;

                        SoundController.instance.PlayAudioEffect(character.SoundKey + "Jump", SoundAction.Play);
                    }// Dash
                    else if (input.Dash && dashCd <= 0)
                    {
                        inputDelay = DashDuration;
                        dashCd = DashCooldown;
                        state = CharacterState.Dashing;
                        return;
                    }// SHOOT
                    else if (input.Action)
                    {
                        //character.PowerUser();
                    }
                    else if (input.Horizontal != 0)
                    {
                        horizontalMOvement = input.Horizontal * Speed;

                        if (grounded)
                            SoundController.instance.PlayAudioEffect(character.SoundKey + "Step", SoundAction.Play);
                    }

                    character.SetXVelocity(horizontalMOvement);

                    break;
                }

            case CharacterState.Dashing:
                {
                    character.SetMovement( new Vector2(DashForce * (int)character.GetDirection(), 0) );
                    if (inputDelay <= 0)
                    {
                        state = CharacterState.Normal;
                    }
                    break;
                }
        }

       
    }


    public void ResetCharacterToStartPosition(Transform positionRef)
    {
        character.PowerRelease();
        character.SetMovement(Vector2.zero);
        character.transform.position = positionRef.position;
    }

    private enum CharacterState
    {
        Normal,
        Dashing,
    }

}
