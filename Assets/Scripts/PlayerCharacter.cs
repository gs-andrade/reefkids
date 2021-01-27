using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Attributes")]
    public float Speed;

    public Vector2 Das;


    private PlayerInput input;

    private void Awake()
    {
        if (input == null)
            input = new PlayerInput();
    }


    private void Update()
    {
        input.GetInputs();

        var movement = new Vector2(input.Horizontal  * Speed, input.Vertical  * Speed);

        transform.Translate(Speed * movement.normalized * Time.deltaTime);
    }
}
