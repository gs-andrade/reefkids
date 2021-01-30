using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public GameObject ArrowInRangeSprite;
    public Rigidbody2D Rb;

    private float timer;

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            ToogleArrowRange(false);


    }
    public void ToogleArrowRange(bool toogle)
    {
        ArrowInRangeSprite.SetActive(toogle);
        timer = 0.05f;
    }
}
