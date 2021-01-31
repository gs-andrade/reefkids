using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMove : MonoBehaviour, IInterctable, IUpdatable
{
    public Transform Plataform;
    public float Speed = 5f;

    private DrawPoint[] points;
    private int moveIndex;
    private Vector2 nextLocation;

    private Vector2 plataformStartPosition;
    private void Awake()
    {
        points = GetComponentsInChildren<DrawPoint>(true);
        nextLocation = points[0].transform.position;
    }

    public void SetNextDestination()
    {
        moveIndex++;
        if (moveIndex >= points.Length)
            moveIndex = 0;

        nextLocation = points[moveIndex].transform.position;
    }

    public void SaveStart()
    {
        plataformStartPosition = Plataform.transform.position;
    }

    public void ResetObj()
    {
        Plataform.transform.position = plataformStartPosition;
        moveIndex = 0;
    }

    public void UpdateObj()
    {
        if (Vector2.Distance(Plataform.position, nextLocation) < 0.05f)
        {
            SetNextDestination();
        }
        else
        {
            Plataform.position = Vector2.MoveTowards(Plataform.position, nextLocation, Speed * Time.deltaTime);
        }
    }
}
