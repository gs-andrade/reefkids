using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput 
{

    [HideInInspector] public float Horizontal;
    [HideInInspector] public float Vertical;

    [HideInInspector] public bool JumpPressed;
    [HideInInspector] public bool JumpHolding;

    [HideInInspector] public bool Dash;
    [HideInInspector] public bool Action;


    [HideInInspector] public bool ColorPressed;
    [HideInInspector] public int ColorSwapDirection;

    public bool HorizontalIsLeft()
    {
        if (Horizontal < 0.1f)
            return true;

        return false;
    }

    public void GetInputs()
    {
        Horizontal = GetClampedValue((Input.GetAxis("Horizontal")));
        Vertical = (Input.GetAxis("Vertical"));

        JumpPressed = Input.GetButtonDown("Jump");
        JumpHolding = Input.GetButton("Jump");

        Dash = Input.GetButtonDown("Dash");
        Action = Input.GetButtonDown("Action");





    }

    public bool DiagonalMove()
    {
        return Horizontal != 0 && Vertical != 0;
    }

    private float GetClampedValue(float value)
    {
        return Mathf.Clamp(value, -1, 1);
    }


}
