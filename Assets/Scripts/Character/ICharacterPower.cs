using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterPower
{
    void Setup();
    void Use();
    bool CanUse();

    void Release();

}
