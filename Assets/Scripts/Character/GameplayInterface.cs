using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayInterface : MonoBehaviour
{
    public GameObject[] Lifes;

    public void UpdateLifeAmmount(int ammount)
    {
        for(int i = 0; i < Lifes.Length; i++)
        {
            if (ammount - 1 >= i)
                Lifes[i].SetActive(true);
            else
                Lifes[i].SetActive(false);
        }
    }
}
