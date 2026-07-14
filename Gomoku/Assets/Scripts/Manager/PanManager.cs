using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanManager : MonoBehaviour
{
    [Header("바둑판")]
    [SerializeField] private Transform panOrigin;
    [SerializeField] private int panSize = 15;
    [SerializeField] private float crossSize = 1f;

    private BadukRType[,] pan;
    private GameObject[,] badukROBJ;
    private Vector3[,] points;

    public int PanSize => panSize;

    private void Awake()
    {
        GeneratePoints();
        pan = new BadukRType[panSize, panSize];
        badukROBJ = new GameObject[PanSize, PanSize];
    }

    private void GeneratePoints()
    {
        points = new Vector3[panSize, panSize];

        for (int x = 0; x < panSize; x++)
        {
            for (int y = 0; y < panSize; y++)
            {
                points[x, y] =
                    panOrigin.position +
                    new Vector3(x * crossSize, 0f, y * crossSize);
            }
        }
    }

    public Vector3 GetPoint(int x, int y)
    {
        if (x < 0 || x >= panSize || y < 0 || y >= panSize)
            return Vector3.zero;

        return points[x, y];
    }

    public Vector3 GetNearPoint(Vector3 worldPos, out int currentX, out int currentY)
    {
        float minDis = Mathf.Infinity;
        Vector3 nearest = Vector3.zero;

        currentX = 0;
        currentY = 0;

        for (int x = 0; x < panSize; x++)
        {
            for (int y = 0; y < panSize; y++)
            {
                float distance = Vector3.Distance(worldPos, points[x, y]);

                if (distance < minDis)
                {
                    minDis = distance;
                    nearest = points[x, y];

                    currentX = x;
                    currentY = y;
                }
            }
        }

        return nearest;
    }

    public BadukRType GetStone(int x, int y)
    {
        return pan[x, y];
    }

    public void SetR(int x, int y, BadukRType type)
    {
        pan[x, y] = type;
    }

    public bool IsEmpty(int x, int y)
    {
        return pan[x, y] == BadukRType.None;
    }

    public BadukRType[,] GetBoard()
    {
        return pan;
    }

    public void SetBadukRObject(int x, int y, GameObject obj)
    {
        badukROBJ[x, y] = obj;
    }

    public GameObject GetBadukRObject(int x, int y)
    {
        return badukROBJ[x, y];
    }

    public void ResetBoard()
    {
        for (int x = 0; x < panSize; x++)
        {
            for (int y = 0; y < panSize; y++)
            {
                pan[x, y] = BadukRType.None;
            }
        }
    }

    // private void OnDrawGizmos()
    // {
    //     if (panOrigin == null)
    //         return;

    //     Gizmos.color = Color.white;

    //     for (int y = 0; y < panSize; y++)
    //     {
    //         Vector3 start = panOrigin.position + new Vector3(0, 0, y * crossSize);
    //         Vector3 end = panOrigin.position + new Vector3((panSize - 1) * crossSize, 0, y * crossSize);

    //         Gizmos.DrawLine(start, end);
    //     }

    //     for (int x = 0; x < panSize; x++)
    //     {
    //         Vector3 start = panOrigin.position + new Vector3(x * crossSize, 0, 0);
    //         Vector3 end = panOrigin.position + new Vector3(x * crossSize, 0, (panSize - 1) * crossSize);

    //         Gizmos.DrawLine(start, end);
    //     }

    //     Gizmos.color = Color.red;

    //     for (int x = 0; x < panSize; x++)
    //     {
    //         for (int y = 0; y < panSize; y++)
    //         {
    //             Gizmos.DrawSphere(points != null ? points[x, y] :
    //                 panOrigin.position + new Vector3(x * crossSize, 0, y * crossSize), 0.05f);
    //         }
    //     }
    // }
}
