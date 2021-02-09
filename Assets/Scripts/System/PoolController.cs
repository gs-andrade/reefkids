using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public static PoolController instance { private set; get; }

    public void Awake()
    {
        instance = this;
    }

}
