using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private PanManager panManager;

    public bool CheckWin(int x, int y)
    {
        BadukRType type = panManager.GetStone(x, y);

        if (type == BadukRType.None)
            return false;

        if (CountBadukR(x, y, 1, 0, type) + CountBadukR(x, y, -1, 0, type) - 1 >= 5)
            return true;

        if (CountBadukR(x, y, 0, 1, type) + CountBadukR(x, y, 0, -1, type) - 1 >= 5)
            return true;

        if (CountBadukR(x, y, 1, 1, type) + CountBadukR(x, y, -1, -1, type) - 1 >= 5)
            return true;

        if (CountBadukR(x, y, 1, -1, type) + CountBadukR(x, y, -1, 1, type) - 1 >= 5)
            return true;

        return false;
    }

    private int CountBadukR(int startX, int startY, int dirX, int dirY, BadukRType type)
    {
        int count = 0;

        int x = startX;
        int y = startY;

        while (x >= 0 && x < panManager.PanSize &&
               y >= 0 && y < panManager.PanSize &&
               panManager.GetStone(x, y) == type)
        {
            count++;

            x += dirX;
            y += dirY;
        }

        return count;
    }
}
