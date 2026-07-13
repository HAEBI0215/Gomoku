using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text resultTXT;

    private void Awake()
    {
        winPanel.SetActive(false);
    }

    public void ShowWin(bool isBlackTurn)
    {
        winPanel.SetActive(true);

        if (isBlackTurn)
            resultTXT.text = "흑 승리";
        else
            resultTXT.text = "백 승리";
    }

    public void HideWin()
    {
        winPanel.SetActive(false);
    }
}