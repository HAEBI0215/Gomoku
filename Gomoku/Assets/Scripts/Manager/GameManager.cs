using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private PanManager panManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private ResultManager resultManager;
    [SerializeField] private UIManager uiManager;

    [Header("바둑알")]
    [SerializeField] private GameObject badukr_B;
    [SerializeField] private GameObject badukr_W;

    public bool isGameOver = false;

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
            panManager.SetStone(x, y, BaduckRType.Black);
        }
        else
        {
            prefab = badukr_W;
            panManager.SetStone(x, y, BaduckRType.White);
        }

        Instantiate(prefab, panManager.GetPoint(x, y), Quaternion.identity);

        if (resultManager.CheckWin(x, y))
        {
            isGameOver = true;
            uiManager.ShowWin(turnManager.IsBlackTurn);
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

        GameObject[] stones = GameObject.FindGameObjectsWithTag("BadukR");

        foreach (GameObject stone in stones)
        {
            Destroy(stone);
        }
    }
}