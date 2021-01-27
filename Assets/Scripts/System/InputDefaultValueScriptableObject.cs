using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InputDefaultValue", menuName = "Custom/System/InputDefaultValue", order = 0)]
public class InputDefaultValueScriptableObject : ScriptableObject
{
    public KeyCode[] KeyCodes;

#if UNITY_EDITOR

    private void OnValidate()
    {
        
    }

#endif
}
