using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem 
{
    public KeyCode[] KeyCodes;

    void Setup()
    {
        KeyCodes = new KeyCode[Enum.GetNames(typeof(InputKeys)).Length];

        for(int i = 0; i < KeyCodes.Length; i++)
        {
            var code = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString( ((InputKeys)i).ToString(), "Space"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

public enum InputKeys
{
    up = 0,
    down = 1,
    left = 2,
    right = 3,
    fire = 4,
    special = 5
}
