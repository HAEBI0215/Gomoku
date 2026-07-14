using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RenjuRuleManager : MonoBehaviour
{
    [SerializeField] private PanManager panManager;

    public bool JangMokCheck(int x, int y)
    {
        BadukRType type = panManager.GetStone(x, y);

        if (type != BadukRType.Black)
            return false;
        
        if (CountR(x, y, 1, 0, type) + CountR(x, y, -1, 0, type) - 1 > 5)
            return true;

        if (CountR(x, y, 0, 1, type) + CountR(x, y, 0, -1, type) - 1 > 5)
            return true;

        if (CountR(x, y, 1, 1, type) + CountR(x, y, -1, -1, type) - 1 > 5)
            return true;
            
        if (CountR(x, y, 1, -1, type) + CountR(x, y, -1, 1, type) - 1 > 5)
            return true;
        
        return false;
    }

    private int CountR(int startX, int startY, int dirX, int dirY, BadukRType type)
    {
        int count = 0;

        int x = startX;
        int y = startY;

        while (x >= 0 && x < panManager.PanSize && y >= 0 && y < panManager.PanSize && panManager.GetStone(x, y) == type)
        {
            count++;

            x += dirX;
            y += dirY;
        }

        return count;
    }
}
