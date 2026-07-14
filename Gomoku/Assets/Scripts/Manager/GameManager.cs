using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private PanManager panManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private ResultManager resultManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private RenjuRuleManager renjuManager;

    [Header("바둑알")]
    [SerializeField] private GameObject badukr_B;
    [SerializeField] private GameObject badukr_W;

    public bool isGameOver = false;
    private int badukRCount = 0;

    private void Start()
    {
        uiManager.HideWin();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (inputManager.Click())
        {
            ChakSuBadukR();
        }
    }

    private void ChakSuBadukR()
    {
        int x = inputManager.CurrentX;
        int y = inputManager.CurrentY;

        if (!panManager.IsEmpty(x, y))
            return;

        GameObject prefab;

        if (turnManager.IsBlackTurn)
        {
            prefab = badukr_B;
            panManager.SetR(x, y, BadukRType.Black);
        }
        else
        {
            prefab = badukr_W;
            panManager.SetR(x, y, BadukRType.White);
        }

        GameObject badukR = Instantiate(prefab, panManager.GetPoint(x, y), Quaternion.identity);
        panManager.SetBadukRObject(x, y, badukR);

        badukRCount++;

        if (renjuManager.JangMokCheck(x, y))
        {
            isGameOver = true;

            foreach (Vector2Int pos in resultManager.WinList)
            {
                GameObject stone = panManager.GetBadukRObject(pos.x, pos.y);

                Renderer renderer = stone.GetComponent<Renderer>();

                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.yellow * 5f);
            }

            uiManager.ShowWin(false, "장목", badukRCount);
            return;

        }
        if (resultManager.CheckWin(x, y))
        {
            isGameOver = true;

            foreach (Vector2Int pos in resultManager.WinList)
            {
                GameObject stone = panManager.GetBadukRObject(pos.x, pos.y);

                Renderer renderer = stone.GetComponent<Renderer>();

                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.yellow * 5f);
            }

            uiManager.ShowWin(turnManager.IsBlackTurn, "오목", badukRCount);
            return;
        }

        turnManager.NextTurn();
    }

    public void RestartGame()
    {
        isGameOver = false;

        panManager.ResetBoard();
        turnManager.ResetTurn();
        uiManager.HideWin();

        badukRCount = 0;

        GameObject[] r = GameObject.FindGameObjectsWithTag("BadukR");

        foreach (GameObject stone in r)
        {
            Destroy(stone);
        }
    }
}