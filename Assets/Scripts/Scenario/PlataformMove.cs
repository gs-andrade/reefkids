using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMove : MonoBehaviour, IInterctable
{
    public Transform Plataform;
    public float Speed = 5f;

    private DrawPoint[] points;
    private int moveIndex;
    private Vector2 nextLocation;

    private Vector2 plataformStartPosition;
    private void Awake()
    {
        points = GetComponentsInChildren<DrawPoint>();
        nextLocation = points[0].transform.position;
    }

    public void SetNextDestination()
    {
        moveIndex++;
        if (moveIndex >= points.Length)
            moveIndex = 0;

        nextLocation = points[moveIndex].transform.position;
    }
    private void Update()
    {
        if(Vector2.Distance(Plataform.position, nextLocation) < 0.05f)
        {
            SetNextDestination();
        }
        else
        {
            Plataform.position = Vector2.MoveTowards(Plataform.position, nextLocation, Speed * Time.deltaTime);
        }
    }

    public void SaveStart()
    {
        plataformStartPosition = Plataform.transform.position;
    }

    public void Reset()
    {
        Plataform.transform.position = plataformStartPosition;
        moveIndex = 0;
    }
}
