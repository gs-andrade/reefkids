using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput 
{

    [HideInInspector] public float Horizontal;
    [HideInInspector] public float Vertical;
    [HideInInspector] public bool JumpPressed;
    [HideInInspector] public bool JumpHolding;

    [HideInInspector] public bool FireOnePressed;
    [HideInInspector] public bool FireTwoPressed;

    [HideInInspector] public bool FireOneHolding;
    [HideInInspector] public bool FireTwoHolding;

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
        Horizontal = (Input.GetAxis("Horizontal"));
        Vertical = (Input.GetAxis("Vertical"));

        JumpPressed = Input.GetButtonDown("Jump");
        JumpHolding = Input.GetButton("Jump");

        FireOnePressed = Input.GetButtonDown("Fire1");
        FireTwoPressed = Input.GetButtonDown("Fire2");

        FireOneHolding = Input.GetButton("Fire1");
        FireTwoHolding = Input.GetButton("Fire2");
            
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
