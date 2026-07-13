using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform panOrigin;
    [SerializeField] private int panSize = 15;
    [SerializeField] private float crossSize = 1f;

    [Header("카메라 관련")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject ChackSuR;
    [SerializeField] private LayerMask panLayer;

    [Header("바둑R")]
    [SerializeField] private GameObject badukr_B;
    [SerializeField] private GameObject badukr_W;

    [Header("턴 표시")]
    [SerializeField] private GameObject quad_B;
    [SerializeField] private GameObject quad_W;

    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text resultTXT;

    private Renderer quad_B_Ren;
    private Renderer quad_W_Ren;

    private BaduckRType[,] pan;
    private bool isBlackTurn = true;
    private int currentX;
    private int currentY;
    private Vector3[,] points;
    private string result;
    public bool isGameOver = false;

    private void Awake()
    {
        GeneratePoints();
        pan = new BaduckRType[panSize, panSize];

        quad_B_Ren = quad_B.GetComponent<Renderer>();
        quad_W_Ren = quad_W.GetComponent<Renderer>();

        UpdateTurnEffect();

        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver)
            return;
        else if (!isGameOver)
            winPanel.SetActive(false);
        
        UpdateChackSuR();

        if (Input.GetMouseButtonDown(0))
        {
            ChakSuBadukR();
        }
    }

    private void UpdateTurnEffect()
    {
        if (isBlackTurn)
        {
            quad_B_Ren.material.SetColor("_EmissionColor", Color.white);
            quad_W_Ren.material.SetColor("_EmissionColor", Color.gray);
        }
        else
        {
            quad_B_Ren.material.SetColor("_EmissionColor", Color.gray);
            quad_W_Ren.material.SetColor("_EmissionColor", Color.white);
        }
    }

    private void UpdateChackSuR()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, panLayer))
        {
            Vector3 nearPoint = GetNearPoint(hit.point);

            ChackSuR.transform.position = nearPoint;
        }
    }

    private Vector3 GetNearPoint(Vector3 worldPos)
    {
        float minDis = Mathf.Infinity;
        Vector3 nearest = Vector3.zero;

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

    private void ChakSuBadukR()
    {
        if (pan[currentX, currentY] != BaduckRType.None)
            return;
        
        GameObject prefab;

        if (isBlackTurn)
        {
            prefab = badukr_B;
            pan[currentX, currentY] = BaduckRType.Black;
        }
        else
        {
            prefab = badukr_W;
            pan[currentX, currentY] = BaduckRType.White;
        }

        Instantiate(prefab, points[currentX, currentY], Quaternion.identity);

        if (CheckWin(currentX, currentY))
        {
            if (isBlackTurn)
                result = "흑 승리";
            else
                result = "백 승리";

            isGameOver = true;

            winPanel.SetActive(true);
            resultTXT.text = result;

            return;
        }

        isBlackTurn = !isBlackTurn;
        UpdateTurnEffect();
    }

    private bool CheckWin(int x, int y)
    {
        BaduckRType type = pan[x, y];

        if (type == BaduckRType.None)
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

    private int CountBadukR(int startX, int startY, int dirX, int dirY, BaduckRType type)
    {
        int count = 0;

        int x = startX;
        int y = startY;

        while (x >= 0 && x < panSize &&
            y >= 0 && y < panSize &&
            pan[x, y] == type)
        {
            count++;

            x += dirX;
            y += dirY;
        }

        return count;
    }

    private void GeneratePoints()
    {
        points = new Vector3[panSize, panSize];

        for (int x = 0; x < panSize; x++)
        {
            for (int y = 0; y < panSize; y++)
            {
                points[x, y] = panOrigin.position + new Vector3(x * crossSize, 0f, y * crossSize);
            }
        }
    }

    public Vector3 GetPoint(int x, int y)
    {
        if (x < 0 || x >= panSize || y < 0 || y >= panSize)
            return Vector3.zero;
        
        return points[x, y];
    }

    private void OnDrawGizmos()
    {
        if (panOrigin == null)
            return;

        Gizmos.color = Color.white;

        for (int y = 0; y < panSize; y++)
        {
            Vector3 start = panOrigin.position + new Vector3(0, 0, y * crossSize);
            Vector3 end = panOrigin.position + new Vector3((panSize - 1) * crossSize, 0, y * crossSize);

            Gizmos.DrawLine(start, end);
        }

        for (int x = 0; x < panSize; x++)
        {
            Vector3 start = panOrigin.position + new Vector3(x * crossSize, 0, 0);
            Vector3 end = panOrigin.position + new Vector3(x * crossSize, 0, (panSize - 1) * crossSize);

            Gizmos.DrawLine(start, end);
        }

        Gizmos.color = Color.red;

        for (int x = 0; x < panSize; x++)
        {
            for (int y = 0; y < panSize; y++)
            {
                Vector3 pos = panOrigin.position + new Vector3(x * crossSize, 0, y * crossSize);
                Gizmos.DrawSphere(pos, 0.05f);
            }
        }
    }

    public void ResetBoard()
    {
        for (int x = 0; x < panSize; x++)
        {
            for (int y = 0; y < panSize; y++)
            {
                pan[x, y] = BaduckRType.None;
            }
        }
        isBlackTurn = true;

        UpdateTurnEffect();
    }
}
