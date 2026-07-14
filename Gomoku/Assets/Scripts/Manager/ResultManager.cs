using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private PanManager panManager;

    public List<Vector2Int> WinList { get; private set;} = new();

    public bool CheckWin(int x, int y)
    {
        BadukRType type = panManager.GetStone(x, y);

        if (type == BadukRType.None)
            return false;

        if (CheckDirection(x, y, 1, 0, type))
            return true;

        if (CheckDirection(x, y, 0, 1, type))
            return true;

        if (CheckDirection(x, y, 1, 1, type))
            return true;

        if (CheckDirection(x, y, 1, -1, type))
            return true;

        return false;
    }

    private bool CheckDirection(int x, int y, int dirX, int dirY, BadukRType type)
    {
        List<Vector2Int> forward = GetLine(x, y, dirX, dirY, type);
        List<Vector2Int> backward = GetLine(x, y, -dirX, -dirY, type);

        backward.Reverse();

        // 현재 돌 중복 제거
        if (backward.Count > 0)
            backward.RemoveAt(backward.Count - 1);

        backward.AddRange(forward);

        if (backward.Count >= 5)
        {
            WinList.Clear();

            for (int i = 0; i < 5; i++)
                WinList.Add(backward[i]);

            return true;
        }

        return false;
    }

    private List<Vector2Int> GetLine(int startX, int startY, int dirX, int dirY, BadukRType type)
    {
        List<Vector2Int> list = new();

        int x = startX;
        int y = startY;

        while (x >= 0 &&
               x < panManager.PanSize &&
               y >= 0 &&
               y < panManager.PanSize &&
               panManager.GetStone(x, y) == type)
        {
            list.Add(new Vector2Int(x, y));

            x += dirX;
            y += dirY;
        }

        return list;
    }
}