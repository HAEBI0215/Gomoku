using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameManager gameManager;

    public void AgainButton()
    {
        gameManager.isGameOver = false;
        gameManager.ResetBoard();

        ClearBadukR();
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ClearBadukR()
    {
        GameObject[] stones = GameObject.FindGameObjectsWithTag("BadukR");

        foreach (GameObject stone in stones)
        {
            Destroy(stone);
        }
    }  
}
