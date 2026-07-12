using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Renderer quad_B_Ren;
    private Renderer quad_W_Ren;

    private BaduckRType[,] pan;
    private bool isBlackTurn = true;
    private int currentX;
    private int currentY;
    private Vector3[,] points;

    private void Awake()
    {
        GeneratePoints();
        pan = new BaduckRType[panSize, panSize];

        quad_B_Ren = quad_B.GetComponent<Renderer>();
        quad_W_Ren = quad_W.GetComponent<Renderer>();

        UpdateTurnEffect();
    }

    private void Update()
    {
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

        isBlackTurn = !isBlackTurn;
        UpdateTurnEffect();
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
}
