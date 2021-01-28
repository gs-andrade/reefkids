using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMOvable : MonoBehaviour
{
    public Transform Plataform;
    public float Speed = 5f;

    private DrawPoint[] points;
    private int moveIndex;
    private Vector2 nextLocation;
    private void Awake()
    {
        points = GetComponentsInChildren<DrawPoint>();
        nextLocation = points[0].transform.position;
    }


    private void Update()
    {
        if(Vector2.Distance(Plataform.position, nextLocation) < 0.05f)
        {
            moveIndex++;
            if (moveIndex >= points.Length)
                moveIndex = 0;

            nextLocation = points[moveIndex].transform.position;
        }
        else
        {
            Plataform.position = Vector2.MoveTowards(Plataform.position, nextLocation, Speed * Time.deltaTime);
        }
    }
}
