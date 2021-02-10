using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Attributes")]
    public float Speed;
    public float JumpForce;

    [Header("Header")]
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;

    private CharacterInstance character;

    private PlayerInput input;

    private bool wasOnAir = true;

    private CharacterState state;
    private float inputDelay;

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


    public CharacterInstance GetPlayer()
    {
        return character;
    }

    public void UpdateCharacters()
    {
        if (inputDelay > 0)
            inputDelay -= Time.deltaTime;


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
                        if (wasOnAir)
                            SoundController.instance.PlayAudioEffect(character.SoundKey + "Fall", SoundAction.Play);
                    }

                    wasOnAir = !grounded;
                    // JUMP
                    if ((input.JumpPressed || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && grounded  && inputDelay <= 0)
                    {
                        character.Jump(JumpForce);
                        inputDelay = 0.2f;

                        SoundController.instance.PlayAudioEffect(character.SoundKey + "Jump", SoundAction.Play);
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        var inputPositon = Input.mousePosition;
                        inputPositon = Camera.main.ScreenToWorldPoint(inputPositon);

                        var direction = (inputPositon - character.transform.position).normalized;

                        var projectile = Instantiate(ProjectilePrefab, transform).GetComponent<ProjectileForward>();

                        projectile.Setup((Vector2)character.transform.position + (Vector2.one * direction), direction, ProjectileSpeed, character.gameObject);

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

          /*  case CharacterState.Dashing:
                {
                    character.SetMovement( new Vector2(DashForce * (int)character.GetDirection(), 0) );
                    if (inputDelay <= 0)
                    {
                        state = CharacterState.Normal;
                    }
                    break;
                }*/
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
