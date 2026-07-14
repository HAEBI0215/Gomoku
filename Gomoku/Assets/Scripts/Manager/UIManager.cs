using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text resultTXT;
    [SerializeField] private TMP_Text countTXT;

    private void Awake()
    {
        winPanel.SetActive(false);
    }

    public void ShowWin(bool isBlackTurn, string reason, int count)
    {
        winPanel.SetActive(true);

        if (isBlackTurn)
            resultTXT.text = "흑 승리";
        else
            resultTXT.text = "백 승리";

        countTXT.text = $"착수 횟수 : {count}수";
    }

    public void HideWin()
    {
        winPanel.SetActive(false);
    }
}